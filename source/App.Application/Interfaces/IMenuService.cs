using App.Application.DTOs.Menu;
using App.Application.DTOs.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Interfaces
{
    public  interface IMenuService
    {
        Task<MenuDto?> GetByIdAsync(int id);
        Task<IEnumerable<MenuDto>> GetAllAsync();
        Task<MenuDto> CreateMenuAsync(CreateMenuDto MenuDto, int currentUserId);
        Task UpdateMenuAsync(UpdateMenuDto menu, int currentUserId);
        Task DeleteMenuAsync(int id);
    }
}
