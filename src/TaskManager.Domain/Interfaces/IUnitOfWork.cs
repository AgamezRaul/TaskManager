using TaskManager.Domain.Interfaces.Repositories;

namespace TaskManager.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IUserRepository Users { get; }
    ITaskRepository Tasks { get; }
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
