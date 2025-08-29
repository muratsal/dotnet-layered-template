using App.Application.DTOs.User;
using App.Application.Interfaces;
using App.Application.Services;
using AutoMapper;
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
        public UserController(IUserService userService, ILogger<UserController>  logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user is null)
            {
                _logger.LogWarning("User not found. Id: {UserId}", id);
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> CreateUser([FromBody] CreateUserDto userDto)
        {
            UserDto user = await _userService.CreateUserAsync(userDto,1);
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateUser(int id, [FromBody] UpdateUserDto userDto)
        {
            if (id != userDto.Id)
                return BadRequest();

            var user = await _userService.GetByIdAsync(id);
            if (user == null)
                return NotFound();

            await _userService.UpdateUserAsync(userDto,1);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
                return NotFound();

            await _userService.DeleteUserAsync(user.Id);

            return NoContent();
        }

    }
}
