using AutoMapper;
using CleanArchitecture.Application.DTOs;
using CleanArchitecture.Domain.Interfaces;
using MediatR;

namespace CleanArchitecture.Application.Queries.Tasks.GetTasksByProject;

public class GetTasksByProjectQueryHandler : IRequestHandler<GetTasksByProjectQuery, IEnumerable<TaskDto>>
{
    private readonly ITaskRepository _repository;
    private readonly IMapper _mapper;

    public GetTasksByProjectQueryHandler(ITaskRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TaskDto>> Handle(GetTasksByProjectQuery request, CancellationToken cancellationToken)
    {
        var tasks = await _repository.GetByProjectIdAsync(request.ProjectId);
        return _mapper.Map<IEnumerable<TaskDto>>(tasks);
    }
}
