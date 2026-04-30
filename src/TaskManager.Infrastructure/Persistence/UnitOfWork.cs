using TaskManager.Domain.Interfaces;
using TaskManager.Domain.Interfaces.Repositories;
using TaskManager.Infrastructure.Persistence.Repositories;

namespace TaskManager.Infrastructure.Persistence;

public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    private IUserRepository? _users;
    private ITaskRepository? _tasks;

    public IUserRepository Users => _users ??= new UserRepository(context);
    public ITaskRepository Tasks => _tasks ??= new TaskRepository(context);

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        => await context.SaveChangesAsync(ct);

    public void Dispose() => context.Dispose();
}
