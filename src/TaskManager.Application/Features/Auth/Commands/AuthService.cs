using Microsoft.Extensions.Logging;
using TaskManager.Application.Common.Interfaces;
using TaskManager.Application.DTOs.Auth;
using TaskManager.Domain.Common;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Application.Features.Auth.Commands;

public class AuthService(
    IUnitOfWork unitOfWork,
    IPasswordHasher passwordHasher,
    IJwtService jwtService,
    ILogger<AuthService> logger) : IAuthService
{
    public async Task<Result<AuthResponse>> RegisterAsync(RegisterRequest request, CancellationToken ct = default)
    {
        var emailExists = await unitOfWork.Users.EmailExistsAsync(request.Email, ct);
        if (emailExists)
        {
            logger.LogWarning("Intento de registro con email duplicado: {Email}", request.Email);
            return Result.Failure<AuthResponse>("El email ya está registrado.");
        }

        var hash = passwordHasher.Hash(request.Password);
        var user = User.Create(request.Email, hash);

        await unitOfWork.Users.AddAsync(user, ct);
        await unitOfWork.SaveChangesAsync(ct);

        logger.LogInformation("Usuario registrado: {UserId}", user.Id);

        return Result.Success(BuildAuthResponse(user));
    }

    public async Task<Result<AuthResponse>> LoginAsync(LoginRequest request, CancellationToken ct = default)
    {
        var user = await unitOfWork.Users.GetByEmailAsync(request.Email, ct);
        if (user is null || !passwordHasher.Verify(request.Password, user.PasswordHash))
        {
            logger.LogWarning("Intento de login fallido para: {Email}", request.Email);
            return Result.Failure<AuthResponse>("Credenciales inválidas.");
        }

        logger.LogInformation("Login exitoso: {UserId}", user.Id);

        return Result.Success(BuildAuthResponse(user));
    }

    private AuthResponse BuildAuthResponse(User user)
    {
        var accessToken = jwtService.GenerateAccessToken(user);
        var refreshToken = jwtService.GenerateRefreshToken();
        // expiry sincrono con la config del JWT — la capa API lo lee de IOptions si necesita
        var expiresAt = DateTime.UtcNow.AddHours(1);

        return new AuthResponse(accessToken, refreshToken, expiresAt, user.Email);
    }
}
