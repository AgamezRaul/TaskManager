using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;

namespace TaskManager.Domain.Interfaces.Repositories;

public interface ITaskRepository : IRepository<TaskItem>
{
    Task<TaskItem?> GetByIdAndUserAsync(Guid taskId, Guid userId, CancellationToken ct = default);

    Task<(IEnumerable<TaskItem> Items, int TotalCount)> GetPagedByUserAsync(
        Guid userId,
        int page,
        int pageSize,
        WorkTaskStatus? statusFilter,
        TaskPriority? priorityFilter,
        CancellationToken ct = default);
}
