using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using TaskManager.Application.Features.Auth.Commands;
using TaskManager.Application.Features.Tasks.Queries;

namespace TaskManager.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ITaskService, TaskService>();

        // registra todos los validators de este assembly automáticamente
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        return services;
    }
}
