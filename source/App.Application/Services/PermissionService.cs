using App.Application.DTOs.Permission;
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
    public class PermissionService : IPermissionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public PermissionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PermissionDto> CreatePermissionAsync(CreatePermissionDto permissionDto, int currentUserId)
        {
            var permission = _mapper.Map<Permission>(permissionDto);
            await _unitOfWork.Permissions.AddAsync(permission, currentUserId);
            await _unitOfWork.CompleteAsync();
            return _mapper.Map<PermissionDto>(permission);
        }

        public async Task DeletePermissionAsync(int id)
        {
            var permission = await _unitOfWork.Permissions.GetAsync(id);
            if (permission is not null)
            {
                await _unitOfWork.Permissions.RemoveAsync(permission);
                await _unitOfWork.CompleteAsync();
            }
        }

        public async Task<IEnumerable<PermissionDto>> GetAllAsync()
        {
            var permissions = await _unitOfWork.Permissions.GetAllAsync();
            return _mapper.Map<IEnumerable<PermissionDto>>(permissions);
        }

        public async Task<PermissionDto?> GetByIdAsync(int id)
        {
           var permission = await _unitOfWork.Permissions.GetAsync(id);
            return _mapper.Map<PermissionDto>(permission);
        }

        public async Task UpdatePermissionAsync(UpdatePermissionDto permissionDto, int currentUserId)
        {
           var dbPermission =await _unitOfWork.Permissions.GetAsync(permissionDto.Id);
            _mapper.Map<UpdatePermissionDto, Permission>(permissionDto, dbPermission);
            await _unitOfWork.Permissions.UpdateAsync(dbPermission, currentUserId);
            await _unitOfWork.CompleteAsync();
        }
    }
}
