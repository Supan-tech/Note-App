using Microsoft.AspNetCore.Mvc;
using Backend.Interfaces;
using Backend.DTOs;
using Microsoft.AspNetCore.Identity;
using Backend.Entity;
using Microsoft.AspNetCore.Authorization;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtTokenService _jwtTokenService;

        public AuthController(IUserService userService, IJwtTokenService jwtTokenService)
        {
            _userService = userService;
            _jwtTokenService = jwtTokenService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
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

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDTO LoginUser)
        {
            var _user = await _userService.GetByEmailAsync(LoginUser.Email);

            if (_user == null)
            {
                return Unauthorized(new ErrorResponseDto
                {
                    Message = "Login failed",
                    StatusCode = StatusCodes.Status401Unauthorized,
                    Errors = ["Invalid credentials"]
                });
            }

            var hasher = new PasswordHasher<User>();
            var result = hasher.VerifyHashedPassword(
                _user,
                _user.Password,
                LoginUser.Password
            );

            if (result == PasswordVerificationResult.Failed)
                return Unauthorized(new ErrorResponseDto
                {
                    Message = "Login failed",
                    StatusCode = StatusCodes.Status401Unauthorized,
                    Errors = ["Invalid credentials"]
                });

            var token = _jwtTokenService.GenerateToken(_user);

            return Ok(new SuccessResponseDto<object>
            {
                Message = "Login successful",
                StatusCode = StatusCodes.Status200OK,
                Data = new
                {
                    accessToken = token,
                    tokenType = "Bearer",
                    expiresIn = 3600,
                    user = new UserResponse
                    {
                        UUid = _user.UUid,
                        Email = _user.Email,
                        Role = _user.Role,
                        FirstName = _user.FirstName,
                        LastName =_user.LastName
                    }
                }
            });
        }

    }
}
