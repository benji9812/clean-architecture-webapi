using CleanArchitecture.Domain.Interfaces;
using MediatR;

namespace CleanArchitecture.Application.Commands.Projects.UpdateProject;

public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand>
{
    private readonly IProjectRepository _repository;

    public UpdateProjectCommandHandler(IProjectRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await _repository.GetByIdAsync(request.Id)
            ?? throw new KeyNotFoundException($"Project with id '{request.Id}' was not found.");

        project.Name = request.Name;
        project.Description = request.Description;

        await _repository.UpdateAsync(project);
    }
}
