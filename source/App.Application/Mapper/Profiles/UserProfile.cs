using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Application.DTOs.User;
using App.Core.Domain;
using AutoMapper;

namespace App.Application.Mapper.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>();  
            CreateMap<UserDto, User>(); 
            CreateMap<CreateUserDto, User>(); 
            CreateMap<UpdateUserDto, User>(); 

            CreateMap<User, UserDetailedDto>()
               .ForMember(dest => dest.Roles, opt => opt.MapFrom(src =>
                   src.UserRoles.Select(ur => ur.Role.Name))) 
               .ForMember(dest => dest.Permissions, opt => opt.MapFrom(src =>
                   src.UserRoles
                      .SelectMany(ur => ur.Role.RolePermissions)
                      .Select(rp => rp.Permission.Key)
                      .Distinct()
               ));
        }
    }
}
