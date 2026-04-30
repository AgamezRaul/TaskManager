using TaskManager.Application.Common.Models;
using TaskManager.Application.DTOs.Tasks;
using TaskManager.Domain.Common;

namespace TaskManager.Application.Features.Tasks.Queries;

public interface ITaskService
{
    Task<Result<PagedResult<TaskResponse>>> GetPagedAsync(Guid userId, GetTasksRequest request, CancellationToken ct = default);
    Task<Result<TaskResponse>> GetByIdAsync(Guid taskId, Guid userId, CancellationToken ct = default);
    Task<Result<TaskResponse>> CreateAsync(Guid userId, CreateTaskRequest request, CancellationToken ct = default);
    Task<Result<TaskResponse>> UpdateAsync(Guid taskId, Guid userId, UpdateTaskRequest request, CancellationToken ct = default);
    Task<Result<TaskResponse>> PatchStatusAsync(Guid taskId, Guid userId, PatchTaskStatusRequest request, CancellationToken ct = default);
    Task<Result> DeleteAsync(Guid taskId, Guid userId, CancellationToken ct = default);
}
