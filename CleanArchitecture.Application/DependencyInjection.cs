using CleanArchitecture.Application.Behaviors;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // AutoMapper – explicitly add maps from the marker type's assembly
        services.AddAutoMapper(cfg => cfg.AddMaps(typeof(AssemblyReference).Assembly));

        // MediatR – scans this assembly for all IRequestHandler<> implementations
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(AssemblyReference).Assembly));

        // Pipeline order matters: outer behavior runs first.
        // ValidationBehavior is OUTER → invalid requests are rejected before
        // they ever reach LoggingBehavior, so failed requests are never
        // logged as "Handled".
        //
        //   Request → ValidationBehavior → LoggingBehavior → Handler
        //              ↑ throws if invalid; Logging never reached
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

        services.AddValidatorsFromAssembly(typeof(AssemblyReference).Assembly);

        return services;
    }
}
