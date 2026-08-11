using MediatR;

namespace CleanArchitecture.Application.Commands.Projects.UpdateProject;

public record UpdateProjectCommand(Guid Id, string Name, string? Description) : IRequest;
