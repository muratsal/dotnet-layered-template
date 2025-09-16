using App.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> Authenticate(string userName ,string password)
        {
            var user = await _loginService.AuthenticateAsync(userName, password);
            if(user is null)
            {
                return NotFound();
            }
            string token =  _loginService.GenerateJwtToken(user);

            return Ok(new {token});
        }
    }
}
