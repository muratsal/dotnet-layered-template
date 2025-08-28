using App.Application.DTOs;
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
        public UserService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<UserDto> CreateUserAsync(User user)
        {
            await _unitOfWork.Users.AddAsync(user);
            return _mapper.Map<UserDto>(user); 
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await _unitOfWork.Users.GetAsync(id);
            if (user is not null)
            {
                await _unitOfWork.Users.RemoveAsync(user);
            }
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var users = await _unitOfWork.Users.GetAllAsync();
            return _mapper.Map<IEnumerable<UserDto>>(users); 
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return  await _unitOfWork.Users.GetAsync(id);
        }

        public async Task<IEnumerable<UserDto>> GetUserBy(string userName, string email)
        {
            var users = await _unitOfWork.Users.FindAsync(x=> (string.IsNullOrEmpty(userName) || x.UserName == userName)
                                                              && (string.IsNullOrEmpty(email) || x.Email == email));
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<User?> GetUserWithDetailAsync(int id)
        {
           return  await _unitOfWork.Users.GetUserWithDetailAsync(id);
        }

        public async Task UpdateUserAsync(User user)
        {
            await _unitOfWork.Users.UpdateAsync(user);
        }
    }
}
