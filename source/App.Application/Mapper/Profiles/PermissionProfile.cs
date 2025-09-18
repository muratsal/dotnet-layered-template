using App.Application.DTOs.Permission;
using App.Core.Domain;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Mapper.Profiles
{
    public class PermissionProfile: Profile
    {
        public PermissionProfile()
        {
            CreateMap<Permission, PermissionDto>();
            CreateMap<PermissionDto, Permission>();
            CreateMap<CreatePermissionDto, Permission>();
            CreateMap<UpdatePermissionDto, Permission>();
        }
    }
}
