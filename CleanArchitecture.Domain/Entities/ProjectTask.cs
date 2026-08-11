using CleanArchitecture.Domain.Enums;

namespace CleanArchitecture.Domain.Entities;

public class ProjectTask
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public ProjectTaskStatus Status { get; set; }
    public TaskPriority Priority { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime CreatedAt { get; set; }

    public Guid ProjectId { get; set; }
    public Project Project { get; set; } = null!;
}
