using CleanArchitecture.Application.DTOs;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Enums;
using CleanArchitecture.Domain.Interfaces;
using MediatR;

namespace CleanArchitecture.Application.Commands.Tasks.CreateTask;

public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, TaskDto>
{
    private readonly ITaskRepository _taskRepository;
    private readonly IProjectRepository _projectRepository;

    public CreateTaskCommandHandler(ITaskRepository taskRepository, IProjectRepository projectRepository)
    {
        _taskRepository = taskRepository;
        _projectRepository = projectRepository;
    }

    public async Task<TaskDto> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        // Ensure the parent project exists before creating a task
        _ = await _projectRepository.GetByIdAsync(request.ProjectId)
            ?? throw new KeyNotFoundException($"Project with id '{request.ProjectId}' was not found.");

        var task = new ProjectTask
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            Status = ProjectTaskStatus.Todo,
            Priority = request.Priority,
            DueDate = request.DueDate,
            CreatedAt = DateTime.UtcNow,
            ProjectId = request.ProjectId
        };

        var created = await _taskRepository.AddAsync(task);

        return new TaskDto(
            created.Id,
            created.Title,
            created.Description,
            created.Status.ToString(),
            created.Priority.ToString(),
            created.DueDate,
            created.CreatedAt,
            created.ProjectId);
    }
}
