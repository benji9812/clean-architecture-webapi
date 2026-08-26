namespace CleanArchitecture.Application.DTOs;

public record ProjectDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public DateTime CreatedAt { get; init; }
    public int TaskCount { get; init; }
}
