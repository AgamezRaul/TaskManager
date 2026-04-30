using Microsoft.Extensions.Logging;
using TaskManager.Application.Common.Models;
using TaskManager.Application.DTOs.Tasks;
using TaskManager.Domain.Common;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Application.Features.Tasks.Queries;

public class TaskService(
    IUnitOfWork unitOfWork,
    ILogger<TaskService> logger) : ITaskService
{
    public async Task<Result<PagedResult<TaskResponse>>> GetPagedAsync(
        Guid userId, GetTasksRequest request, CancellationToken ct = default)
    {
        var (items, total) = await unitOfWork.Tasks.GetPagedByUserAsync(
            userId, request.Page, request.PageSize, request.Status, request.Priority, ct);

        var result = new PagedResult<TaskResponse>
        {
            Items = items.Select(MapToResponse),
            TotalCount = total,
            Page = request.Page,
            PageSize = request.PageSize
        };

        return Result.Success(result);
    }

    public async Task<Result<TaskResponse>> GetByIdAsync(Guid taskId, Guid userId, CancellationToken ct = default)
    {
        var task = await unitOfWork.Tasks.GetByIdAndUserAsync(taskId, userId, ct);
        if (task is null)
        {
            return Result.Failure<TaskResponse>("Tarea no encontrada.");
        }

        return Result.Success(MapToResponse(task));
    }

    public async Task<Result<TaskResponse>> CreateAsync(
        Guid userId, CreateTaskRequest request, CancellationToken ct = default)
    {
        var task = TaskItem.Create(request.Title, request.Description, request.Priority, request.DueDate, userId);

        await unitOfWork.Tasks.AddAsync(task, ct);
        await unitOfWork.SaveChangesAsync(ct);

        logger.LogInformation("Tarea creada: {TaskId} por usuario {UserId}", task.Id, userId);

        return Result.Success(MapToResponse(task));
    }

    public async Task<Result<TaskResponse>> UpdateAsync(
        Guid taskId, Guid userId, UpdateTaskRequest request, CancellationToken ct = default)
    {
        var task = await unitOfWork.Tasks.GetByIdAndUserAsync(taskId, userId, ct);
        if (task is null)
        {
            return Result.Failure<TaskResponse>("Tarea no encontrada.");
        }

        task.Update(request.Title, request.Description, request.Priority, request.DueDate);
        unitOfWork.Tasks.Update(task);
        await unitOfWork.SaveChangesAsync(ct);

        logger.LogInformation("Tarea actualizada: {TaskId}", taskId);

        return Result.Success(MapToResponse(task));
    }

    public async Task<Result<TaskResponse>> PatchStatusAsync(
        Guid taskId, Guid userId, PatchTaskStatusRequest request, CancellationToken ct = default)
    {
        var task = await unitOfWork.Tasks.GetByIdAndUserAsync(taskId, userId, ct);
        if (task is null)
        {
            return Result.Failure<TaskResponse>("Tarea no encontrada.");
        }

        task.ChangeStatus(request.Status);
        unitOfWork.Tasks.Update(task);
        await unitOfWork.SaveChangesAsync(ct);

        logger.LogInformation("Estado de tarea {TaskId} cambiado a {Status}", taskId, request.Status);

        return Result.Success(MapToResponse(task));
    }

    public async Task<Result> DeleteAsync(Guid taskId, Guid userId, CancellationToken ct = default)
    {
        var task = await unitOfWork.Tasks.GetByIdAndUserAsync(taskId, userId, ct);
        if (task is null)
        {
            return Result.Failure("Tarea no encontrada.");
        }

        unitOfWork.Tasks.Remove(task);
        await unitOfWork.SaveChangesAsync(ct);

        logger.LogInformation("Tarea eliminada: {TaskId}", taskId);

        return Result.Success();
    }

    private static TaskResponse MapToResponse(TaskItem task) => new(
        task.Id,
        task.Title,
        task.Description,
        task.Status,
        task.Priority,
        task.DueDate,
        task.CreatedAt,
        task.UpdatedAt);
}
