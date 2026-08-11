using CleanArchitecture.Application.DTOs;
using MediatR;

namespace CleanArchitecture.Application.Queries.Tasks.GetTasksByProject;

public record GetTasksByProjectQuery(Guid ProjectId) : IRequest<IEnumerable<TaskDto>>;
