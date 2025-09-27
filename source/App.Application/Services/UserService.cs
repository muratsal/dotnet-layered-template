using App.Application.DTOs.Menu;
using App.Application.DTOs.Permission;
using App.Application.DTOs.User;
using App.Application.Interfaces;
using App.Core.Domain;
using App.Core.Enums;
using App.Core.Repository.Interfaces;
using AutoMapper;
using System;
using System.Collections;
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
        public async Task<UserDto> CreateUserAsync(CreateUserDto userDto, int currentUserId)
        {
            var user = _mapper.Map<User>(userDto);
            user.PasswordHash = "1234";

            await _unitOfWork.Users.AddAsync(user, currentUserId);
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

        public async Task UpdateUserAsync(UpdateUserDto userDto, int currentUserId)
        {
            var dbUser = await _unitOfWork.Users.GetAsync(userDto.Id);
            _mapper.Map<UpdateUserDto, User>(userDto, dbUser);
            await _unitOfWork.Users.UpdateAsync(dbUser, currentUserId);
            await _unitOfWork.CompleteAsync();
        }

        //public async Task GetUserMenuInfo(int userId)
        //{
        //    var menuPermissions = await _unitOfWork.Users.GetUserPermissionsAsync(userId, (int)PermissionType.Menu);
        //}

        public async Task<IEnumerable<PermissionDto>> GetUserPermissionsAsync(int userId, int? permissionType = null)
        {
            var permissions = await _unitOfWork.Users.GetUserPermissionsAsync(userId, permissionType);
            return _mapper.Map<IEnumerable<PermissionDto>>(permissions);
        }


        public async Task<List<UserMenuItemDto>> GetUserMenuInfo(int userId)
        {
            var menuPermissions = await _unitOfWork.Users.GetUserPermissionsAsync(userId, (int)PermissionType.Menu);
            var allMenus = (await _unitOfWork.Menus.GetAllAsync()).ToList();

            allMenus = allMenus.Where(x => menuPermissions.Any(y => y.Key.Contains(x.Name))).ToList();

            var userMenuItemListDto = new List<UserMenuItemDto>();

            allMenus.ForEach(x =>
            {
                var pageItems = menuPermissions.Where(y => y.Key.Contains(x.Name)).OrderBy(x => x.Key).ToList();
                pageItems.ForEach(m =>
                {
                    var parts = m.Key.Split(".");
                    if (userMenuItemListDto.Any(y => y.MenuName == parts[0]))
                    {
                        var menuItem = userMenuItemListDto.FirstOrDefault(y => y.MenuName == parts[0]);
                        if (parts.Length == 2)
                        {
                            menuItem.SubMenuItems.Add(new SubMenuItemDTO
                            {
                                MenuName = parts[1],
                                TranslateName = $"{parts[1].ToUpperInvariant()}.{parts[1].ToUpperInvariant()}MENU",
                                State = parts[0] + "/" + parts[1]
                            });
                        }
                        else if (parts.Length == 3)
                        {
                            var item = new SubMenuItemDTO
                            {
                                MenuName = parts[2],
                                TranslateName = $"{parts[2].ToUpperInvariant()}.{parts[2].ToUpperInvariant()}MENU",
                                State = parts[0] + "/" + parts[2]
                            };

                            if (menuItem.SubMenuItems.Any(k => k.MenuName == parts[1]))
                            {
                                var submenu = menuItem.SubMenuItems.First(k => k.MenuName == parts[1]);
                                submenu.SubMenuItems.Add(item);
                            }
                            else
                            {
                                menuItem.SubMenuItems.Add(new SubMenuItemDTO
                                {
                                    MenuName = parts[1],
                                    TranslateName = $"SUBMENU.{menuItem.MenuName.ToUpperInvariant()}.{parts[1].ToUpperInvariant()}",
                                    SubMenuItems =new List<SubMenuItemDTO> { item }
                                });
                            }
                        }
                    }
                    else
                    {
                        UserMenuItemDto menuItem = new UserMenuItemDto();
                        menuItem.MenuIcon = x.Icon;
                        menuItem.MenuName = x.Name;
                        menuItem.TranslateName = $"MAINMENU.{x.Name.ToUpperInvariant()}";
                        menuItem.SubMenuItems = new List<SubMenuItemDTO>();
                        userMenuItemListDto.Add(menuItem);
                        if (parts.Length == 2)
                        {
                            menuItem.SubMenuItems.Add(new SubMenuItemDTO
                            {
                                MenuName = parts[1],
                                TranslateName = $"{parts[1].ToUpperInvariant()}.{parts[1].ToUpperInvariant()}MENU",
                                State = parts[0] + "/" +parts[1]
                            });
                        }
                        else if (parts.Length == 3)
                        {
                            var item = new SubMenuItemDTO
                            {
                                MenuName = parts[2],
                                TranslateName = $"{parts[2].ToUpperInvariant()}.{parts[2].ToUpperInvariant()}MENU",
                                State = parts[0] + "/" + parts[2]
                            };

                            if (menuItem.SubMenuItems.Any(k => k.MenuName == parts[1]))
                            {
                                var submenu = menuItem.SubMenuItems.First(k => k.MenuName ==  parts[1]);
                                submenu.SubMenuItems.Add(item);
                            }
                            else
                            {
                                menuItem.SubMenuItems.Add(new SubMenuItemDTO
                                {
                                    MenuName = parts[1],
                                    TranslateName = $"SUBMENU.{menuItem.MenuName}.{parts[1]}",
                                    SubMenuItems = { item }
                                });
                            }
                        }
                    }
                });
            });
            return userMenuItemListDto;
        }


    }
}
