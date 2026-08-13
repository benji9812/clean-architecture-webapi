namespace CleanArchitecture.Application.DTOs;

public record LoginResponse(
    string Token,
    string Username,
    string Role,
    DateTime ExpiresAt
);
