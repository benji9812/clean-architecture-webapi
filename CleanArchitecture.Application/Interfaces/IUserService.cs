using CleanArchitecture.Application.DTOs;

namespace CleanArchitecture.Application.Interfaces;

/// <summary>
/// Validates user credentials and returns user information.
/// Defined in Application, implemented in Infrastructure.
/// </summary>
public interface IUserService
{
    UserDto? ValidateCredentials(string username, string password);
}
