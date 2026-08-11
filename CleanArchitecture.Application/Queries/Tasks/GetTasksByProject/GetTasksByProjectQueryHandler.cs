using CleanArchitecture.Application.DTOs;
using CleanArchitecture.Domain.Interfaces;
using MediatR;

namespace CleanArchitecture.Application.Queries.Tasks.GetTasksByProject;

public class GetTasksByProjectQueryHandler : IRequestHandler<GetTasksByProjectQuery, IEnumerable<TaskDto>>
{
    private readonly ITaskRepository _repository;

    public GetTasksByProjectQueryHandler(ITaskRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<TaskDto>> Handle(GetTasksByProjectQuery request, CancellationToken cancellationToken)
    {
        var tasks = await _repository.GetByProjectIdAsync(request.ProjectId);

        return tasks.Select(t => new TaskDto(
            t.Id,
            t.Title,
            t.Description,
            t.Status.ToString(),
            t.Priority.ToString(),
            t.DueDate,
            t.CreatedAt,
            t.ProjectId));
    }
}
