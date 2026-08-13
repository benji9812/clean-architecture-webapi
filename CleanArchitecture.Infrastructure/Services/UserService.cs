using CleanArchitecture.Application.DTOs;
using CleanArchitecture.Application.Interfaces;
using CleanArchitecture.Infrastructure.Configuration;
using Microsoft.Extensions.Configuration;

namespace CleanArchitecture.Infrastructure.Services;

/// <summary>
/// Validates credentials against a list of test users from configuration
/// (User Secrets in development / environment variables in production).
/// Replace with a real database-backed implementation (hashed passwords) in production.
/// </summary>
public class UserService : IUserService
{
    private readonly List<TestUserConfig> _users;

    public UserService(IConfiguration configuration)
    {
        _users = configuration
            .GetSection("TestUsers")
            .Get<List<TestUserConfig>>() ?? [];
    }

    public UserDto? ValidateCredentials(string username, string password)
    {
        var match = _users.FirstOrDefault(u =>
            string.Equals(u.Username, username, StringComparison.OrdinalIgnoreCase) &&
            u.Password == password);

        return match is null ? null : new UserDto(match.Username, match.Role);
    }
}
