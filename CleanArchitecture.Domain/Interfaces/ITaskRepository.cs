using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Domain.Interfaces;

public interface ITaskRepository
{
    Task<IEnumerable<ProjectTask>> GetByProjectIdAsync(Guid projectId);
    Task<ProjectTask?> GetByIdAsync(Guid id);
    Task<ProjectTask> AddAsync(ProjectTask task);
    Task UpdateAsync(ProjectTask task);
    Task DeleteAsync(Guid id);
}
