namespace CleanArchitecture.Application.Interfaces;

/// <summary>
/// Generates a signed JWT token for an authenticated user.
/// Defined in Application, implemented in Infrastructure.
/// </summary>
public interface IJwtTokenService
{
    string GenerateToken(string username, string role);
}
