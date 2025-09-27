using App.Application.DTOs.Menu;
using App.Application.DTOs.Role;
using App.Application.Interfaces;
using App.Core.Domain;
using App.Core.Repository.Interfaces;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Services
{
    public class MenuService : IMenuService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MenuService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<MenuDto> CreateMenuAsync(CreateMenuDto MenuDto, int currentUserId)
        {
            var menu = _mapper.Map<Menu>(MenuDto);
            await _unitOfWork.Menus.AddAsync(menu, currentUserId);
            await _unitOfWork.CompleteAsync();
            return _mapper.Map<MenuDto>(menu);
        }

        public async Task DeleteMenuAsync(int id)
        {
            var menu = await _unitOfWork.Menus.GetAsync(id);
            if (menu is not null)
            {
                await _unitOfWork.Menus.RemoveAsync(menu);
                await _unitOfWork.CompleteAsync();
            }
        }

        public async Task<IEnumerable<MenuDto>> GetAllAsync()
        {
            var menus = await _unitOfWork.Menus.GetAllAsync();
            return _mapper.Map<IEnumerable<MenuDto>>(menus);
        }

        public async Task<MenuDto?> GetByIdAsync(int id)
        {
            var menu = await _unitOfWork.Menus.GetAsync(id);
            var menuDto = _mapper.Map<MenuDto>(menu);
            return menuDto;
        }

        public async Task UpdateMenuAsync(UpdateMenuDto menuDto, int currentUserId)
        {
            var dbMenu = await _unitOfWork.Menus.GetAsync(menuDto.Id);
            _mapper.Map<UpdateMenuDto, Menu>(menuDto, dbMenu);
            await _unitOfWork.Menus.UpdateAsync(dbMenu, currentUserId);
            await _unitOfWork.CompleteAsync();
        }
    }
}
