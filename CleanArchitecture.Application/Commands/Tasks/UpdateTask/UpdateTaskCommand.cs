using CleanArchitecture.Domain.Enums;
using MediatR;

namespace CleanArchitecture.Application.Commands.Tasks.UpdateTask;

public record UpdateTaskCommand(
    Guid Id,
    string Title,
    string? Description,
    ProjectTaskStatus Status,
    TaskPriority Priority,
    DateTime? DueDate
) : IRequest;
