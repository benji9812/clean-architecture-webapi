using MediatR;

namespace CleanArchitecture.Application.Commands.Projects.DeleteProject;

public record DeleteProjectCommand(Guid Id) : IRequest;
