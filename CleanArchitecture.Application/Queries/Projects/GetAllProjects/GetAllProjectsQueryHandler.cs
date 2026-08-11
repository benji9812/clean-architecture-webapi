using CleanArchitecture.Application.DTOs;
using CleanArchitecture.Domain.Interfaces;
using MediatR;

namespace CleanArchitecture.Application.Queries.Projects.GetAllProjects;

public class GetAllProjectsQueryHandler : IRequestHandler<GetAllProjectsQuery, IEnumerable<ProjectDto>>
{
    private readonly IProjectRepository _repository;

    public GetAllProjectsQueryHandler(IProjectRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ProjectDto>> Handle(GetAllProjectsQuery request, CancellationToken cancellationToken)
    {
        var projects = await _repository.GetAllAsync();

        return projects.Select(p => new ProjectDto(
            p.Id,
            p.Name,
            p.Description,
            p.CreatedAt,
            p.Tasks.Count));
    }
}
