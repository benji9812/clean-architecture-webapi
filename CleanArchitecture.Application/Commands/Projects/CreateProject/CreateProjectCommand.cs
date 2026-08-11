using CleanArchitecture.Application.DTOs;
using MediatR;

namespace CleanArchitecture.Application.Commands.Projects.CreateProject;

public record CreateProjectCommand(string Name, string? Description) : IRequest<ProjectDto>;
