using CleanArchitecture.Application.DTOs;
using MediatR;

namespace CleanArchitecture.Application.Commands.Auth.Login;

public record LoginCommand(string Username, string Password) : IRequest<LoginResponse>;
