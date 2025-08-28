using App.Application.Interfaces;
using App.Core.Repository.Interfaces;
using App.Web.ViewModels.Account;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace App.Web.Controllers.Mvc
{
    public class HomeController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public HomeController(IUserService userService,
            IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var userDtos = await _userService.GetAllAsync();
            UserListViewModel viewModel = new UserListViewModel
            {
                Users = _mapper.Map<List<UserViewModel>>(userDtos)
            };
            return View("Index", viewModel);
        }
    }
}
