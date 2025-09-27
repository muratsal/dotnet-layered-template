using App.Application.DTOs.Permission;
using App.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Interfaces
{
    public  interface ILoginService
    {
        Task<User?> AuthenticateAsync(string username, string password);

        Task<IEnumerable<PermissionDto>> GetProcessPermissionsAsync(int userId);

        // API JWT token üretimi
        string GenerateJwtToken(User user);

        // MVC için ClaimsPrincipal
        ClaimsPrincipal GenerateClaimsPrincipal(User user);
    }
}
