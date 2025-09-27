using App.Application.DTOs.Permission;
using App.Application.DTOs.User;
using App.Application.Interfaces;
using App.Core.Domain;
using App.Core.Enums;
using App.Core.Repository.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public LoginService(IUserRepository userRepository,IConfiguration configuration, IMapper mapper)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<User?> AuthenticateAsync(string username, string password)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            if (user == null) return null;

            
            if (password != user.PasswordHash) return null;

            return user;
        }

        public async Task<IEnumerable<PermissionDto>> GetProcessPermissionsAsync(int userId)
        {
            var permissions = await _userRepository.GetUserPermissionsAsync(userId,(int)PermissionType.Process);
            return _mapper.Map<IEnumerable<PermissionDto>>(permissions);
        }

        public string GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName)
        };

            var permissions = GetProcessPermissionsAsync(user.Id).Result;
            claims.AddRange(permissions.Select(p => new Claim("permissions", p.Key)));

            var secret = _configuration["JwtSettings:Secret"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret!));
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

            var permissions = GetProcessPermissionsAsync(user.Id).Result;
            claims.AddRange(permissions.Select(p => new Claim("permissions", p.Key)));

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
