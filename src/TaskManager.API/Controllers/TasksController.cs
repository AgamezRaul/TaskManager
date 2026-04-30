using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.API.Extensions;
using TaskManager.Application.Common.Models;
using TaskManager.Application.DTOs.Tasks;
using TaskManager.Application.Features.Tasks.Queries;

namespace TaskManager.API.Controllers;

[ApiController]
[Route("api/tasks")]
[Authorize]
[Produces("application/json")]
public class TasksController(
    ITaskService taskService,
    IValidator<CreateTaskRequest> createValidator,
    IValidator<UpdateTaskRequest> updateValidator) : ControllerBase
{
    /// <summary>Devuelve las tareas del usuario autenticado con paginación y filtros opcionales.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<TaskResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] GetTasksRequest request,
        CancellationToken ct)
    {
        var userId = User.GetUserId();
        var result = await taskService.GetPagedAsync(userId, request, ct);
        return Ok(result.Value);
    }

    /// <summary>Devuelve una tarea por ID (solo si pertenece al usuario autenticado).</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(TaskResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var userId = User.GetUserId();
        var result = await taskService.GetByIdAsync(id, userId, ct);

        return result.IsFailure
            ? NotFound(new { error = result.Error })
            : Ok(result.Value);
    }

    /// <summary>Crea una nueva tarea para el usuario autenticado.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(TaskResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromBody] CreateTaskRequest request,
        CancellationToken ct)
    {
        var validation = await createValidator.ValidateAsync(request, ct);
        if (!validation.IsValid)
            return BadRequest(validation.Errors.Select(e => e.ErrorMessage));

        var userId = User.GetUserId();
        var result = await taskService.CreateAsync(userId, request, ct);

        return CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value);
    }

    /// <summary>Actualiza una tarea completa (reemplaza todos sus campos).</summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(TaskResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateTaskRequest request,
        CancellationToken ct)
    {
        var validation = await updateValidator.ValidateAsync(request, ct);
        if (!validation.IsValid)
            return BadRequest(validation.Errors.Select(e => e.ErrorMessage));

        var userId = User.GetUserId();
        var result = await taskService.UpdateAsync(id, userId, request, ct);

        return result.IsFailure
            ? NotFound(new { error = result.Error })
            : Ok(result.Value);
    }

    /// <summary>Cambia únicamente el estado de una tarea.</summary>
    [HttpPatch("{id:guid}/status")]
    [ProducesResponseType(typeof(TaskResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PatchStatus(
        Guid id,
        [FromBody] PatchTaskStatusRequest request,
        CancellationToken ct)
    {
        var userId = User.GetUserId();
        var result = await taskService.PatchStatusAsync(id, userId, request, ct);

        return result.IsFailure
            ? NotFound(new { error = result.Error })
            : Ok(result.Value);
    }

    /// <summary>Elimina una tarea.</summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var userId = User.GetUserId();
        var result = await taskService.DeleteAsync(id, userId, ct);

        return result.IsFailure
            ? NotFound(new { error = result.Error })
            : NoContent();
    }
}
