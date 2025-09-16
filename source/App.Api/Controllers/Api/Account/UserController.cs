using App.Application.DTOs.User;
using App.Application.Interfaces;
using App.Application.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.Web.Controllers.Api.Account
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;
        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [Authorize(Policy = "users.read")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        [Authorize(Policy = "users.read")]
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            try
            {
                var user = await _userService.GetByIdAsync(id);
                return Ok(user);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("User not found. Id: {UserId}", id);
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> CreateUser([FromBody] CreateUserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid CreateUser request. Errors: {Errors}",
                    string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
                return BadRequest(ModelState);
            }

            UserDto user = await _userService.CreateUserAsync(userDto, 1);
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateUser(int id, [FromBody] UpdateUserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid UpdateUser request. Errors: {Errors}",
                    string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
                return BadRequest(ModelState);
            }

            if (id != userDto.Id)
                return BadRequest();

            try
            {
                var user = await _userService.GetByIdAsync(id);
                if (user == null)
                    return NotFound();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("User not found. Id: {UserId}", id);
                return NotFound(new { message = ex.Message });
            }

            await _userService.UpdateUserAsync(userDto, 1);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            try
            {
                var user = await _userService.GetByIdAsync(id);
                if (user is not null)
                {
                    await _userService.DeleteUserAsync(user.Id);
                }
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("User not found. Id: {UserId}", id);
                return NotFound(new { message = ex.Message });
            }

            return NoContent();
        }

    }
}
