using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunnersWebApp.Data;
using RunnersWebApp.Interfaces;
using RunnersWebApp.Models;

namespace RunnersWebApp.Controllers
{
    public class ClubController : Controller
    {
        private readonly IClubInterface _clubInterface;
        public ClubController(IClubInterface clubInterface)
        {
            _clubInterface = clubInterface;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Club> clubs = await _clubInterface.GetAll();
            return View(clubs);
        }
        public async Task<IActionResult> Detail(int id)
        {
            Club club = await _clubInterface.GetByIdAsync(id);
            return View(club);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Club club)
        {
            if (!ModelState.IsValid)
            {
                return View(club);
            }
            _clubInterface.Add(club);
            return RedirectToAction("Index");
        }
    }
}
