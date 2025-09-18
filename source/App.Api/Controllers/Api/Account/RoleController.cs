using App.Application.DTOs.Role;
using App.Application.DTOs.User;
using App.Application.Interfaces;
using App.Application.Services;
using App.Web.Shared.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.Web.Controllers.Api.Account
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly ILogger<RoleController> _logger;

        public RoleController(IRoleService roleService, ILogger<RoleController> logger)
        {
            _roleService = roleService;
            _logger = logger;
        }

        [Authorize(Policy = "roles.read")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoleDto>>> GetRoles()
        {
            var roles = await _roleService.GetAllAsync();
            return Ok(roles);
        }

        [Authorize(Policy = "roles.read")]
        [HttpGet("{id}")]
        public async Task<ActionResult<RoleDto>> GetRole(int id)
        {
            try
            {
                var role = await _roleService.GetByIdAsync(id);
                return Ok(role);
            }
            catch (KeyNotFoundException ex)
            {

                _logger.LogWarning("Role not found. Id {RoleId}", id);
                return NotFound(new { message = ex.Message });
            }
        }

        [Authorize(Policy = "roles.read")]
        [HttpPost]
        public async Task<ActionResult<RoleDto>> CreateRole([FromBody] CreateRoleDto roleDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid CreateRole request. Errors{Errors}",
                    string.Join(",", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
                return BadRequest();
            }
            RoleDto role = await _roleService.CreateRoleAsync(roleDto, User.GetUserId()!.Value);
            return CreatedAtAction(nameof(GetRole), new { id = role.Id }, role);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateRole(int id, [FromBody] UpdateRoleDto roleDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid UpdateRole request. Errors: {Errors}",
                    string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
                return BadRequest(ModelState);
            }

            if (id != roleDto.Id)
                return BadRequest();
            try
            {
                var role = await _roleService.GetByIdAsync(id);
                if (role == null)
                    return NotFound();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("Role not found. Id {RoleId}", id);
                return NotFound(new { message = ex.Message });
            }

            await _roleService.UpdateRoleAsync(roleDto, User.GetUserId()!.Value);
            return NoContent();
        }

        [Authorize(Policy ="roles.read")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteRole(int id)
        {
            try
            {
                var role = await _roleService.GetByIdAsync(id);
                if(role is not null)
                {
                    await _roleService.DeleteRoleAsync(id);
                }
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("Role not found. Id {RoleId}", id);
                return NotFound(new { message = ex.Message });
            }

            return NoContent();
        }


    }
}
