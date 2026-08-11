using CleanArchitecture.Application.DTOs;
using CleanArchitecture.Domain.Enums;
using MediatR;

namespace CleanArchitecture.Application.Commands.Tasks.CreateTask;

public record CreateTaskCommand(
    string Title,
    string? Description,
    TaskPriority Priority,
    DateTime? DueDate,
    Guid ProjectId
) : IRequest<TaskDto>;
