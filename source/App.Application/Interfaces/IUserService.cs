using App.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Application.DTOs.User;

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

    }
}
