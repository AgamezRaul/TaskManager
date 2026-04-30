using TaskManager.Domain.Common;
using TaskManager.Domain.Enums;

namespace TaskManager.Domain.Entities;

public class TaskItem : BaseEntity
{
    public string Title { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public WorkTaskStatus Status { get; private set; } = WorkTaskStatus.Pending;
    public TaskPriority Priority { get; private set; } = TaskPriority.Medium;
    public DateTime? DueDate { get; private set; }
    public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;

    public Guid UserId { get; private set; }
    public User User { get; private set; } = null!;

    private TaskItem() { }

    public static TaskItem Create(
        string title,
        string? description,
        TaskPriority priority,
        DateTime? dueDate,
        Guid userId)
    {
        return new TaskItem
        {
            Title = title.Trim(),
            Description = description?.Trim(),
            Priority = priority,
            DueDate = dueDate,
            UserId = userId
        };
    }

    public void Update(string title, string? description, TaskPriority priority, DateTime? dueDate)
    {
        Title = title.Trim();
        Description = description?.Trim();
        Priority = priority;
        DueDate = dueDate;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ChangeStatus(WorkTaskStatus newStatus)
    {
        Status = newStatus;
        UpdatedAt = DateTime.UtcNow;
    }
}
