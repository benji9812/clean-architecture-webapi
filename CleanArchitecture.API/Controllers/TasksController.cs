using CleanArchitecture.Application.Commands.Tasks.CreateTask;
using CleanArchitecture.Application.Commands.Tasks.DeleteTask;
using CleanArchitecture.Application.Commands.Tasks.UpdateTask;
using CleanArchitecture.Application.DTOs;
using CleanArchitecture.Application.Queries.Tasks.GetTaskById;
using CleanArchitecture.Application.Queries.Tasks.GetTasksByProject;
using CleanArchitecture.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Authorize]  // All endpoints require a valid JWT
public class TasksController : ControllerBase
{
    private readonly IMediator _mediator;

    public TasksController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>Returns all tasks for a project. Accessible by Admin and User.</summary>
    [HttpGet("project/{projectId:guid}")]
    [Authorize(Roles = "Admin,User")]
    [ProducesResponseType(typeof(IEnumerable<TaskDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByProject(Guid projectId)
    {
        var result = await _mediator.Send(new GetTasksByProjectQuery(projectId));
        return Ok(result);
    }

    /// <summary>Returns a single task by ID. Accessible by Admin and User.</summary>
    [HttpGet("{id:guid}")]
    [Authorize(Roles = "Admin,User")]
    [ProducesResponseType(typeof(TaskDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetTaskByIdQuery(id));
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Creates a new task inside a project. Admin only.</summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(TaskDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Create([FromBody] CreateTaskCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>Updates an existing task. Admin only.</summary>
    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTaskRequest request)
    {
        await _mediator.Send(new UpdateTaskCommand(
            id,
            request.Title,
            request.Description,
            request.Status,
            request.Priority,
            request.DueDate));
        return NoContent();
    }

    /// <summary>Deletes a task. Admin only.</summary>
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteTaskCommand(id));
        return NoContent();
    }
}

public record UpdateTaskRequest(
    string Title,
    string? Description,
    ProjectTaskStatus Status,
    TaskPriority Priority,
    DateTime? DueDate);
