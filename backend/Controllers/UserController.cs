using Microsoft.AspNetCore.Mvc;
using Backend.Interfaces;
using Backend.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllAsync();
            var response = new SuccessResponseDto<IEnumerable<UserResponse>>
            {
                Message = "Success",
                StatusCode = StatusCodes.Status200OK,
                Data = users
            };

            return Ok(response);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateUser([FromBody] RegisterUserDTO user)
        {

            var _user = await _userService.CreateAsync(user);
            var response = new SuccessResponseDto<UserResponse>
            {
                Message = "success",
                StatusCode = StatusCodes.Status201Created,
                Data = _user
            };
            return StatusCode(201, response);
        }

        [HttpGet("{uuid}")]
        public async Task<IActionResult> GetUserByUUID(Guid uuid)
        {

            if (uuid == Guid.Empty)
            {
                var response = new ErrorResponseDto
                {
                    Message = "Can't get user",
                    StatusCode = StatusCodes.Status400BadRequest,
                    Errors = ["Invalid user id"]
                };

                return BadRequest(response);
            }

            var _user = await _userService.GetByIdAsync(uuid);
            if (_user == null)
            {
                return NotFound(new ErrorResponseDto
                {
                    Message = "Can't get user",
                    StatusCode = StatusCodes.Status404NotFound,
                    Errors = ["User Not Found"]
                });
            }
            return Ok(new SuccessResponseDto<UserResponse>
            {
                Message = "User retrieved successfully",
                StatusCode = StatusCodes.Status200OK,
                Data = _user
            });
        }
    }
}
