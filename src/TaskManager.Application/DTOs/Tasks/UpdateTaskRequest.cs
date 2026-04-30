using TaskManager.Domain.Enums;

namespace TaskManager.Application.DTOs.Tasks;

public record UpdateTaskRequest(
    string Title,
    string? Description,
    TaskPriority Priority,
    DateTime? DueDate);
