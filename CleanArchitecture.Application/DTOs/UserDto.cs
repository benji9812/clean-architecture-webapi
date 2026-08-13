namespace CleanArchitecture.Application.DTOs;

/// <summary>Represents an authenticated user passed between Application services.</summary>
public record UserDto(string Username, string Role);
