namespace CleanArchitecture.Infrastructure.Configuration;

/// <summary>
/// Represents a hard-coded test user loaded from appsettings.
/// In production, replace with a real User entity + password hashing (BCrypt/Argon2).
/// </summary>
public class TestUserConfig
{
    public string Username { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public string Role { get; init; } = string.Empty;
}
