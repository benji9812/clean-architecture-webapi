using AutoMapper;
using CleanArchitecture.Application.DTOs;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Project → ProjectDto
        // TaskCount is derived from the navigation property; never exposes the entity directly.
        CreateMap<Project, ProjectDto>()
            .ForMember(d => d.TaskCount, o => o.MapFrom(s => s.Tasks.Count));

        // ProjectTask → TaskDto
        // Enums are mapped to their string names for a readable API response.
        CreateMap<ProjectTask, TaskDto>()
            .ForMember(d => d.Status,   o => o.MapFrom(s => s.Status.ToString()))
            .ForMember(d => d.Priority, o => o.MapFrom(s => s.Priority.ToString()));
    }
}
