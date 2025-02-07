using Microsoft.AspNetCore.Mvc;
using RunnersWebApp.Data;
using RunnersWebApp.Interfaces;
using RunnersWebApp.ViewModels;

namespace RunnersWebApp.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IUserInterface _userInterface;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPhotoInterface _photoInterface;

        public DashboardController(IUserInterface userInterface, IHttpContextAccessor httpContextAccessor, IPhotoInterface photoInterface)
        {
            _userInterface = userInterface;
            _httpContextAccessor = httpContextAccessor;
            _photoInterface = photoInterface;
        }
        public async Task<IActionResult> Index()
        {
            var userRaces = await _userInterface.GetAllUserRaces();
            var userClubs = await _userInterface.GetAllUserClubs();
            var dashboardViewModel = new DashboardViewModel()
            {
                Races = userRaces,
                Clubs = userClubs
            };
            return View(dashboardViewModel);
        }

        public async Task<IActionResult> EditUserProfile()
        {
            var currentUserId = _httpContextAccessor.HttpContext.User.GetUserId();
            var user = await _userInterface.GetUserById(currentUserId);
            if(user == null) return View("Error");
            var editUserProfileViewModel = new EditUserProfileViewModel
            {
                Id = currentUserId,
                Pace = user.Pace,
                Mileage = user.Mileage,
                ProfilePictureUrl = user.ProfilePictureUrl,
                City = user.City,
                Country = user.Country
            };
            return View(editUserProfileViewModel);
        }
    }
}
