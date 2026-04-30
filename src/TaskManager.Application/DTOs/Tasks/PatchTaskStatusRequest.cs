using TaskManager.Domain.Enums;

namespace TaskManager.Application.DTOs.Tasks;

public record PatchTaskStatusRequest(WorkTaskStatus Status);
