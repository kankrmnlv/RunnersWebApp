using Microsoft.AspNetCore.Mvc;
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
                    Image = user.ProfilePictureUrl,
                    Pace = user.Pace,
                    Mileage = user.Mileage
                };
                result.Add(runnersViewModel);
            }
            return View(result);
        }

        public async Task<IActionResult> Detail(string id)
        {
            var user = await _runnersInterface.GetUserById(id);
            var runnersDetailViewModel = new RunnersDetailViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Pace = user.Pace,
                Mileage = user.Mileage,
                Image = user.ProfilePictureUrl,
                Country = user.Country,
                City = user.City
            };

            return View(runnersDetailViewModel);
        }
    }
}
