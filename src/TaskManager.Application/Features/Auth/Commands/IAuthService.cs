using TaskManager.Application.DTOs.Auth;
using TaskManager.Domain.Common;

namespace TaskManager.Application.Features.Auth.Commands;

public interface IAuthService
{
    Task<Result<AuthResponse>> RegisterAsync(RegisterRequest request, CancellationToken ct = default);
    Task<Result<AuthResponse>> LoginAsync(LoginRequest request, CancellationToken ct = default);
}
