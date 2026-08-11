using CleanArchitecture.Application.DTOs;
using MediatR;

namespace CleanArchitecture.Application.Queries.Tasks.GetTaskById;

public record GetTaskByIdQuery(Guid Id) : IRequest<TaskDto?>;
