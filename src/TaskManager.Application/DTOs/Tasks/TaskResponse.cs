using TaskManager.Domain.Enums;

namespace TaskManager.Application.DTOs.Tasks;

public record TaskResponse(
    Guid Id,
    string Title,
    string? Description,
    WorkTaskStatus Status,
    TaskPriority Priority,
    DateTime? DueDate,
    DateTime CreatedAt,
    DateTime UpdatedAt);
