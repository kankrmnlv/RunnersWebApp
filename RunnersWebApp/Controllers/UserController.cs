﻿using Microsoft.AspNetCore.Mvc;
using RunnersWebApp.Interfaces;
using RunnersWebApp.ViewModels;

namespace RunnersWebApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IRunnersInterface _runnersInterface;
        public UserController(IRunnersInterface runnersInterface)
        {
            _runnersInterface = runnersInterface;
        }
        [HttpGet("users")]
        public async Task<IActionResult> Index()
        {
            var users = await _runnersInterface.GetAllUsers();
            List<RunnersViewModel> result = new List<RunnersViewModel>();
            foreach (var user in users)
            {
                var runnersViewModel = new RunnersViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Pace = user.Pace,
                    Mileage = user.Mileage
                };
                result.Add(runnersViewModel);
            }
            return View(result);
        }
    }
}
