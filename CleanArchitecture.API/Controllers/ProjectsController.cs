using CleanArchitecture.Application.Commands.Projects.CreateProject;
using CleanArchitecture.Application.Commands.Projects.DeleteProject;
using CleanArchitecture.Application.Commands.Projects.UpdateProject;
using CleanArchitecture.Application.DTOs;
using CleanArchitecture.Application.Queries.Projects.GetAllProjects;
using CleanArchitecture.Application.Queries.Projects.GetProjectById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Authorize]  // All endpoints require a valid JWT
public class ProjectsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProjectsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>Returns all projects with their task count. Accessible by Admin and User.</summary>
    [HttpGet]
    [Authorize(Roles = "Admin,User")]
    [ProducesResponseType(typeof(IEnumerable<ProjectDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetAllProjectsQuery());
        return Ok(result);
    }

    /// <summary>Returns a single project by ID. Accessible by Admin and User.</summary>
    [HttpGet("{id:guid}")]
    [Authorize(Roles = "Admin,User")]
    [ProducesResponseType(typeof(ProjectDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetProjectByIdQuery(id));
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Creates a new project. Admin only.</summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ProjectDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Create([FromBody] CreateProjectCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>Updates an existing project. Admin only.</summary>
    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProjectRequest request)
    {
        await _mediator.Send(new UpdateProjectCommand(id, request.Name, request.Description));
        return NoContent();
    }

    /// <summary>Deletes a project and all its tasks (cascade). Admin only.</summary>
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteProjectCommand(id));
        return NoContent();
    }
}

public record UpdateProjectRequest(string Name, string? Description);
