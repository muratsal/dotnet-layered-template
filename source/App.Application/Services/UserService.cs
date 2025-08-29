using App.Application.DTOs.User;
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
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<UserDto> CreateUserAsync(CreateUserDto userDto,int currentUserId)
        {
            var user = _mapper.Map<User>(userDto);
            user.PasswordHash = "1234";
           
            await _unitOfWork.Users.AddAsync(user,currentUserId);
            await _unitOfWork.CompleteAsync();
            return _mapper.Map<UserDto>(user);
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await _unitOfWork.Users.GetAsync(id);
            if (user is not null)
            {
                await _unitOfWork.Users.RemoveAsync(user);
                await _unitOfWork.CompleteAsync();
            }
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var users = await _unitOfWork.Users.GetAllAsync();
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<UserDto?> GetByIdAsync(int id)
        {
            var user = await _unitOfWork.Users.GetAsync(id);
            var userDto = _mapper.Map<UserDetailedDto>(user);
            return userDto;
        }

        public async Task<IEnumerable<UserDto>> GetUserBy(string userName, string email)
        {
            var users = await _unitOfWork.Users.FindAsync(x => (string.IsNullOrEmpty(userName) || x.UserName == userName)
                                                              && (string.IsNullOrEmpty(email) || x.Email == email));
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<UserDto?> GetUserWithDetailAsync(int id)
        {
            var user = await _unitOfWork.Users.GetUserWithDetailAsync(id);
            return _mapper.Map<UserDetailedDto>(user);
        }

        public async Task UpdateUserAsync(UpdateUserDto userDto,int currentUserId)
        {
            var dbUser = _unitOfWork.Users.Get(userDto.Id);
            var user = _mapper.Map<UpdateUserDto,User>(userDto,dbUser);
            await _unitOfWork.Users.UpdateAsync(dbUser,currentUserId);
            await _unitOfWork.CompleteAsync();
        }
    }
}
