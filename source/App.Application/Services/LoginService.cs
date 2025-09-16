using App.Application.Interfaces;
using App.Core.Domain;
using App.Core.Repository.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Services
{
    public class LoginService : ILoginService
    {
        private readonly IUserRepository _userRepository;

        public LoginService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User?> AuthenticateAsync(string username, string password)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            if (user == null) return null;

            
            if (password != user.PasswordHash) return null;

            return user;
        }

        public async Task<IEnumerable<string>> GetPermissionsAsync(int userId)
        {
            return await _userRepository.GetUserPermissionsAsync(userId);
        }

        public string GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName)
        };

            var permissions = GetPermissionsAsync(user.Id).Result;
            claims.AddRange(permissions.Select(p => new Claim("permissions", p)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("lfıoehasşlfhdşslajfhşdlsajfbşldsajbhfşdsljbf"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public ClaimsPrincipal GenerateClaimsPrincipal(User user)
        {
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName)
        };

            var permissions = GetPermissionsAsync(user.Id).Result;
            claims.AddRange(permissions.Select(p => new Claim("permissions", p)));

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            return new ClaimsPrincipal(identity);
        }

        private bool VerifyPassword(string password, string hash)
        {
            // Hash karşılaştırma logic
            return password == hash; // örnek, gerçek hayatta hash check
        }
    }

}
