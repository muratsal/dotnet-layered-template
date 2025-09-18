using App.Application.DTOs.Permission;
using App.Application.DTOs.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Interfaces
{
    public  interface IPermissionService
    {
        Task<PermissionDto?> GetByIdAsync(int id);
        Task<IEnumerable<PermissionDto>> GetAllAsync();
        Task<PermissionDto> CreatePermissionAsync(CreatePermissionDto permissionDto, int currentUserId);
        Task UpdatePermissionAsync(UpdatePermissionDto permissionDto, int currentUserId);
        Task DeletePermissionAsync(int id);
    }
}
