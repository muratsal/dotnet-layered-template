using App.Application.DTOs;
using App.Web.ViewModels.Account;
using AutoMapper;

namespace App.Web.Mapper.Account
{
    public class UserViewModelProfile : Profile
    {
        public UserViewModelProfile()
        {
            CreateMap<UserDto, UserViewModel>();
        }
    }
}