using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManagement.Application.Contracts.TasksEndpoints;
using TaskManagement.Application.Interfaces.Services;
using TaskManagement.Application.ViewModels;

namespace TaskManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TasksController : ControllerBase
    {
        private readonly ITasksService _tasksService;

        public TasksController(ITasksService tasksService)
        {
            _tasksService = tasksService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> CreateTask(CreateTaskRequest request, CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue("userId");

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID not found in token.");
            }
            Guid userIdGuid;
            if (!Guid.TryParse(userId, out userIdGuid))
            {
                return BadRequest("Broken User ID.");
            }

            await _tasksService.CreateTask(request, userIdGuid);
            return Ok();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PagedResult<TaskShortInfo>>> GetTasks([FromQuery] GetAllTasksRequest request, CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue("userId");

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID not found in token.");
            }
            Guid userIdGuid;
            if (!Guid.TryParse(userId, out userIdGuid))
            {
                return BadRequest("Broken User ID.");
            }

            var result = await _tasksService.GetTasks(request, userIdGuid);
            if (result == null || result.Items.Count() == 0)
            {
                return NotFound("User do not have any tasks.");
            }
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TaskDto>> GetTaskById(Guid id, CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue("userId");

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID not found in token.");
            }
            Guid userIdGuid;
            if (!Guid.TryParse(userId, out userIdGuid))
            {
                return BadRequest("Broken User ID.");
            }
            var result = await _tasksService.GetTaskById(id, userIdGuid);
            if (result == null)
            {
                return NotFound("Task with provided ID was not found or user have no access to it");
            }
            return Ok(result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TaskDto>> UpdateTask(Guid id, [FromBody] UpdateTaskRequest request, CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue("userId");

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID not found in token.");
            }
            Guid userIdGuid;
            if (!Guid.TryParse(userId, out userIdGuid))
            {
                return BadRequest("Broken User ID.");
            }

            var task = await _tasksService.GetTaskById(id, userIdGuid);
            if (task == null)
            {
                return NotFound("Task with provided ID was not found or user have no access to it");
            }
            var updatedTask = await _tasksService.UpdateTask(id, request, userIdGuid);
            return Ok(updatedTask);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteTask(Guid id, CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue("userId");

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID not found in token.");
            }
            Guid userIdGuid;
            if (!Guid.TryParse(userId, out userIdGuid))
            {
                return BadRequest("Broken User ID.");
            }
            var result = await _tasksService.GetTaskById(id, userIdGuid);
            if (result == null)
            {
                return NotFound("Task with provided ID was not found or user have no access to it");
            }

            await _tasksService.DeleteTask(id, userIdGuid);

            return NoContent();
        }
    }
}
