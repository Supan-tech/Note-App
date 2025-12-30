using Microsoft.AspNetCore.Mvc;
using Backend.Interfaces;
using Backend.DTOs;
using Backend.Entity;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NoteController : ControllerBase
    {
        private readonly INoteService _noteService;
        private readonly IUserService _userService;

        public NoteController(INoteService noteService, IUserService userService)
        {
            _noteService = noteService;
            _userService = userService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllNote([FromQuery] QueryNoteDto query)
        {
            var _userSid = User.FindFirst(ClaimTypes.Sid)?.Value;

            Guid userId;
            if (!Guid.TryParse(_userSid, out userId))
            {
                return Unauthorized(new ErrorResponseDto
                {
                    Message = "Invalid user identifier",
                    StatusCode = StatusCodes.Status401Unauthorized
                });
            }

            var _user = await _userService.GetByUUIDAsync(userId);
            if (_user == null)
            {
                return Unauthorized(new ErrorResponseDto
                {
                    Message = "Invalid user identifier",
                    StatusCode = StatusCodes.Status401Unauthorized
                });
            }


            var notes = await _noteService.GetAllAsync(_user.Uid, query);
            var response = new SuccessResponseDto<IEnumerable<Note>>
            {
                Message = "Note retrieved successfully",
                StatusCode = StatusCodes.Status200OK,
                Data = notes
            };

            return Ok(response);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateNote(CreateNoteDto note)
        {
            var _userSid = User.FindFirst(ClaimTypes.Sid)?.Value;

            Guid userId;
            if (!Guid.TryParse(_userSid, out userId))
            {
                return Unauthorized(new ErrorResponseDto
                {
                    Message = "Invalid user identifier",
                    StatusCode = StatusCodes.Status401Unauthorized
                });
            }

            var _user = await _userService.GetByUUIDAsync(userId);
            if (_user == null)
            {
                return Unauthorized(new ErrorResponseDto
                {
                    Message = "Invalid user identifier",
                    StatusCode = StatusCodes.Status401Unauthorized
                });
            }

            var _note = await _noteService.CreateAsync(_user.Uid, note);

            var response = new SuccessResponseDto<Note>
            {
                Message = "Note is created successfully",
                StatusCode = StatusCodes.Status201Created,
                Data = _note
            };
            return StatusCode(201, response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetNoteById(int id)
        {

            var _userSid = User.FindFirst(ClaimTypes.Sid)?.Value;

            Guid userId;
            if (!Guid.TryParse(_userSid, out userId))
            {
                return Unauthorized(new ErrorResponseDto
                {
                    Message = "Invalid user identifier",
                    StatusCode = StatusCodes.Status401Unauthorized
                });
            }

            var _user = await _userService.GetByUUIDAsync(userId);
            if (_user == null)
            {
                return Unauthorized(new ErrorResponseDto
                {
                    Message = "Invalid user identifier",
                    StatusCode = StatusCodes.Status401Unauthorized
                });
            }

            var note = await _noteService.GetByIdAsync(id);
            if (note == null)
            {
                return NotFound(new ErrorResponseDto
                {
                    Message = "Can't retreive Note",
                    StatusCode = StatusCodes.Status404NotFound,
                    Errors = ["Note Not Found"]
                });
            }

            if (note.UserUID != _user.Uid)
            {
                return StatusCode(403, new ErrorResponseDto
                {
                    Message = "Access denied",
                    StatusCode = StatusCodes.Status403Forbidden,
                    Errors = ["You are not allowed to access this resource"]
                });
            }

            return Ok(new SuccessResponseDto<Note>
            {
                Message = "Note retrieved successfully",
                StatusCode = StatusCodes.Status200OK,
                Data = note
            });
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateNote(int id, [FromBody] CreateNoteDto newnote)
        {
            var _userSid = User.FindFirst(ClaimTypes.Sid)?.Value;

            Guid userId;
            if (!Guid.TryParse(_userSid, out userId))
            {
                return Unauthorized(new ErrorResponseDto
                {
                    Message = "Invalid user identifier",
                    StatusCode = StatusCodes.Status401Unauthorized
                });
            }

            var _user = await _userService.GetByUUIDAsync(userId);
            if (_user == null)
            {
                return Unauthorized(new ErrorResponseDto
                {
                    Message = "Invalid user identifier",
                    StatusCode = StatusCodes.Status401Unauthorized
                });
            }

            var _newNote = await _noteService.UpdateAsync(_user.Uid,id,newnote);

            var response = new SuccessResponseDto<Note>
            {
                Message = "Note is updated successfully",
                StatusCode = StatusCodes.Status201Created,
                Data = _newNote
            };
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNote(int id)
        {
            var _userSid = User.FindFirst(ClaimTypes.Sid)?.Value;

            Guid userId;
            if (!Guid.TryParse(_userSid, out userId))
            {
                return Unauthorized(new ErrorResponseDto
                {
                    Message = "Invalid user identifier",
                    StatusCode = StatusCodes.Status401Unauthorized
                });
            }

            var _user = await _userService.GetByUUIDAsync(userId);
            if (_user == null)
            {
                return Unauthorized(new ErrorResponseDto
                {
                    Message = "Invalid user identifier",
                    StatusCode = StatusCodes.Status401Unauthorized
                });
            }

            var result = await _noteService.DeleteAsync(_user.Uid,id);

            var response = new SuccessResponseDto<bool>
            {
                Message = "Note is deleted successfully",
                StatusCode = StatusCodes.Status200OK,
                Data = result
            };
            return Ok(response);
        }

    }
}
