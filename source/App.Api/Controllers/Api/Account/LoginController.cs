using App.Application.Interfaces;
using App.Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace App.Web.Controllers.Api.Account
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILoginService _loginService;
        private readonly ILogger<UserController> _logger;
        public LoginController(
            IUserService userService,
            ILoginService loginService,
            ILogger<UserController> logger)
        {
            _userService = userService;
            _loginService = loginService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Authenticate(LoginRequest loginRequest)
        {
            var user = await _loginService.AuthenticateAsync(loginRequest.UserName, loginRequest.Password);
            if(user is null)
            {
                return NotFound();
            }
            string token =  _loginService.GenerateJwtToken(user);
            var permissions =await _userService.GetUserPermissionsAsync(user.Id);

            return Ok(new {token,user.Id, user.UserName ,permissions = permissions.Select(x=> new {x.Key ,x.PermissionType}).ToList() });
        }

        [HttpGet("Profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized();
            var user = await _userService.GetByIdAsync(int.Parse(userIdClaim.Value));
            if (user == null)
                return NotFound();
            var permissions = await _userService.GetUserPermissionsAsync(user.Id);
            return Ok(new { user.Id, user.UserName, permissions = permissions.Select(x => new { x.Key, x.PermissionType }).ToList() });
        }

        [HttpGet("GetUserMenu")]
        public async Task<IActionResult> GetUserMenu()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized();

            var userMenuInfo = await _userService.GetUserMenuInfo(int.Parse(userIdClaim.Value));
            return Ok(new {userMenuInfo});
        }


        public class LoginRequest
        {
            public required string UserName { get; set; }
            public required string Password { get; set; }
        }
    }
}
