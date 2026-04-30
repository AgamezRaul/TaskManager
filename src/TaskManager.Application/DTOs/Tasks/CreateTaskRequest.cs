using TaskManager.Domain.Enums;

namespace TaskManager.Application.DTOs.Tasks;

public record CreateTaskRequest(
    string Title,
    string? Description,
    TaskPriority Priority,
    DateTime? DueDate);
