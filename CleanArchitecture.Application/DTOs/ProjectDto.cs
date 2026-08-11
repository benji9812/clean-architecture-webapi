namespace CleanArchitecture.Application.DTOs;

public record ProjectDto(
    Guid Id,
    string Name,
    string? Description,
    DateTime CreatedAt,
    int TaskCount
);
