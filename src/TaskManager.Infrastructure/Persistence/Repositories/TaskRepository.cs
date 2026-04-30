using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;
using TaskManager.Domain.Interfaces.Repositories;

namespace TaskManager.Infrastructure.Persistence.Repositories;

public class TaskRepository(AppDbContext context) : Repository<TaskItem>(context), ITaskRepository
{
    public async Task<TaskItem?> GetByIdAndUserAsync(Guid taskId, Guid userId, CancellationToken ct = default)
        => await DbSet
            .FirstOrDefaultAsync(t => t.Id == taskId && t.UserId == userId, ct);

    public async Task<(IEnumerable<TaskItem> Items, int TotalCount)> GetPagedByUserAsync(
        Guid userId,
        int page,
        int pageSize,
        WorkTaskStatus? statusFilter,
        TaskPriority? priorityFilter,
        CancellationToken ct = default)
    {
        var query = DbSet
            .AsNoTracking()
            .Where(t => t.UserId == userId);

        if (statusFilter.HasValue)
            query = query.Where(t => t.Status == statusFilter.Value);

        if (priorityFilter.HasValue)
            query = query.Where(t => t.Priority == priorityFilter.Value);

        var total = await query.CountAsync(ct);

        var items = await query
            .OrderByDescending(t => t.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (items, total);
    }
}
