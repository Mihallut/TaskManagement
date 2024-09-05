using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.Contracts.UsersEndpoints;
using TaskManagement.Application.Interfaces.Services;

namespace TaskManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;
        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterUserRequest request, CancellationToken cancellationToken)
        {
            await _usersService.Register(request.Username, request.Email, request.Password);
            return Ok();
        }

        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(LoginUserRequest request, CancellationToken cancellationToken)
        {
            var token = await _usersService.Login(request.Email, request.Password);
            if (token == null)
            {
                return BadRequest("Invalid password.");
            }
            return Ok(token);
        }
    }
}
