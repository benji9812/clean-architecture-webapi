using CleanArchitecture.Application.DTOs;
using MediatR;

namespace CleanArchitecture.Application.Queries.Projects.GetAllProjects;

public record GetAllProjectsQuery : IRequest<IEnumerable<ProjectDto>>;
