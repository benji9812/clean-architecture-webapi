using CleanArchitecture.Application.DTOs;
using MediatR;

namespace CleanArchitecture.Application.Queries.Projects.GetProjectById;

public record GetProjectByIdQuery(Guid Id) : IRequest<ProjectDto?>;
