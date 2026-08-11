using MediatR;

namespace CleanArchitecture.Application.Commands.Tasks.DeleteTask;

public record DeleteTaskCommand(Guid Id) : IRequest;
