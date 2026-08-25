namespace CleanArchitecture.Application.DTOs;

public record TaskDto
{
    public Guid Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string? Description { get; init; }
    public string Status { get; init; } = string.Empty;
    public string Priority { get; init; } = string.Empty;
    public DateTime? DueDate { get; init; }
    public DateTime CreatedAt { get; init; }
    public Guid ProjectId { get; init; }
}
