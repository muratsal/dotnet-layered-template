using App.Application.DTOs.Role;
using App.Application.DTOs.User;
using App.Application.Interfaces;
using App.Core.Domain;
using App.Core.Repository.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Services
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public RoleService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<RoleDto> CreateRoleAsync(CreateRoleDto roleDto, int currentUserId)
        {
            var role = _mapper.Map<Role>(roleDto);
            await _unitOfWork.Roles.AddAsync(role, currentUserId);
            await _unitOfWork.CompleteAsync();
            return _mapper.Map<RoleDto>(role);
        }

        public async Task DeleteRoleAsync(int id)
        {
           var role = await _unitOfWork.Roles.GetAsync(id);
            if(role is not null)
            {
                await _unitOfWork.Roles.RemoveAsync(role);
                await _unitOfWork.CompleteAsync();
            }
        }

        public async Task<IEnumerable<RoleDto>> GetAllAsync()
        {
            var roles = await _unitOfWork.Roles.GetAllAsync();
            return _mapper.Map<IEnumerable<RoleDto>>(roles);
        }

        public async Task<RoleDto?> GetByIdAsync(int id)
        {
            var role = await _unitOfWork.Roles.GetAsync(id);
            var roleDto = _mapper.Map<RoleDto>(role);
            return roleDto;
        }

        public async Task UpdateRoleAsync(UpdateRoleDto roleDto, int currentUserId)
        {
            var dbRole = await _unitOfWork.Roles.GetAsync(roleDto.Id);
            _mapper.Map<UpdateRoleDto, Role>(roleDto, dbRole);
            await _unitOfWork.Roles.UpdateAsync(dbRole, currentUserId);
            await _unitOfWork.CompleteAsync();
        }
    }
}
