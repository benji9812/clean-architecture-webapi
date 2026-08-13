using CleanArchitecture.Application.DTOs;
using CleanArchitecture.Application.Interfaces;
using MediatR;

namespace CleanArchitecture.Application.Commands.Auth.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly IUserService _userService;
    private readonly IJwtTokenService _jwtTokenService;

    public LoginCommandHandler(IUserService userService, IJwtTokenService jwtTokenService)
    {
        _userService = userService;
        _jwtTokenService = jwtTokenService;
    }

    public Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = _userService.ValidateCredentials(request.Username, request.Password)
            ?? throw new UnauthorizedAccessException("Invalid username or password.");

        var expiresAt = DateTime.UtcNow.AddHours(1);
        var token = _jwtTokenService.GenerateToken(user.Username, user.Role);

        return Task.FromResult(new LoginResponse(token, user.Username, user.Role, expiresAt));
    }
}
