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

        public async  Task DeletePermissionAsync(int id)
        {
           var permission =await _unitOfWork.Roles.GetAsync(id);
            if(permission is not null)
            {
                await _unitOfWork.Permissions.RemoveAsync(permission);
                await _unitOfWork.CompleteAsync();
            }
        }

        public Task<IEnumerable<PermissionDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<PermissionDto?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdatePermissionAsync(UpdatePermissionDto role, int currentUserId)
        {
            throw new NotImplementedException();
        }
    }
}
