using App.Application.DTOs.Menu;
using App.Application.DTOs.Permission;
using App.Application.DTOs.User;
using App.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserDto?> GetByIdAsync(int id);
        Task<IEnumerable<UserDto>> GetAllAsync();
        Task<UserDto?> GetUserWithDetailAsync(int id);
        Task<IEnumerable<UserDto>> GetUserBy(string userName,string email);
        Task<UserDto> CreateUserAsync(CreateUserDto userDto,int currentUserId);
        Task UpdateUserAsync(UpdateUserDto user, int currentUserId);
        Task DeleteUserAsync(int id);
        Task<IEnumerable<PermissionDto>> GetUserPermissionsAsync(int userId, int? permissionType = null);
        Task<List<UserMenuItemDto>> GetUserMenuInfo(int userId);

    }
}
