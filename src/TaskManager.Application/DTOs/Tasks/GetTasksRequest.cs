using TaskManager.Domain.Enums;

namespace TaskManager.Application.DTOs.Tasks;

public record GetTasksRequest(
    int Page = 1,
    int PageSize = 10,
    WorkTaskStatus? Status = null,
    TaskPriority? Priority = null);
