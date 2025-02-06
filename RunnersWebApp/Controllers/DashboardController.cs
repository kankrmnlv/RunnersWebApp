using Microsoft.AspNetCore.Mvc;
using RunnersWebApp.Data;
using RunnersWebApp.Interfaces;
using RunnersWebApp.ViewModels;

namespace RunnersWebApp.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IUserInterface _userInterface;

        public DashboardController(IUserInterface userInterface)
        {
            _userInterface = userInterface;
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
    }
}
