using App.Application.DTOs.Role;
using App.Application.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Interfaces
{
    public interface  IRoleService
    {
        Task<RoleDto?> GetByIdAsync(int id);
        Task<IEnumerable<RoleDto>> GetAllAsync();
        Task<RoleDto> CreateRoleAsync(CreateRoleDto roleDto, int currentUserId);
        Task UpdateRoleAsync(UpdateRoleDto role, int currentUserId);
        Task DeleteRoleAsync(int id);

    }
}
