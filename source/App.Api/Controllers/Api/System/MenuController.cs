using App.Application.DTOs.Menu;
using App.Application.DTOs.Role;
using App.Application.Interfaces;
using App.Application.Services;
using App.Web.Common;
using App.Web.Shared.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.Web.Controllers.Api.System
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly IMenuService _menuService;
        private readonly ILogger<MenuController> _logger;

        public MenuController(IMenuService menuService, ILogger<MenuController> logger)
        {
            _menuService = menuService;
            _logger = logger;
        }

        [Authorize(Policy = "menus.read")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MenuDto>>> GetMenus()
        {
            var menus = await _menuService.GetAllAsync();
            return Ok(menus);
        }

        [Authorize(Policy = "menus.read")]
        [HttpGet("{id}")]
        public async Task<ActionResult<MenuDto>> GetMenu(int id)
        {
            try
            {
                var menu = await _menuService.GetByIdAsync(id);
                return Ok(menu);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("Menu not found Id {MenuId}", id);
                return NotFound(new { Message = ex.Message });
            }
        }

        [Authorize(Policy = "menus.read")]
        [HttpPost]
        [ValidateModel]
        public async Task<ActionResult<MenuDto>> CreateMenu([FromBody] CreateMenuDto menuDto)
        {
            MenuDto menu = await _menuService.CreateMenuAsync(menuDto, User.GetUserId()!.Value);
            return CreatedAtAction(nameof(GetMenu), new { id = menu.Id }, menu);
        }

        [Authorize(Policy = "menus.read")]
        [HttpPut("{id}")]
        [ValidateModel]
        public async Task<ActionResult> UpdateMenu(int id ,[FromBody] UpdateMenuDto menuDto)
        {
            if (id != menuDto.Id)
                return BadRequest();

            try
            {
                await _menuService.UpdateMenuAsync(menuDto, User.GetUserId()!.Value);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("Menu not found. Id {MenuId}", id);
                return NotFound(new { message = ex.Message });
            }
        
        }

        [Authorize(Policy = "menu.read")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMenu(int id)
        {
            try
            {
                var menu = await _menuService.GetByIdAsync(id);
                if (menu is not null)
                {
                    await _menuService.DeleteMenuAsync(id);
                }
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("Menu not found. Id {MenuId}", id);
                return NotFound(new { message = ex.Message });
            }

            return NoContent();
        }
    }
}
