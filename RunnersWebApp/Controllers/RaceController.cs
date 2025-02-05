using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunnersWebApp.Data;
using RunnersWebApp.Interfaces;
using RunnersWebApp.Models;

namespace RunnersWebApp.Controllers
{
    public class RaceController : Controller
    {
        private readonly IRaceInterface _raceInterface;
        public RaceController(IRaceInterface raceInterface)
        {
            _raceInterface = raceInterface;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Race> races = await _raceInterface.GetAll();

            return View(races);
        }

        public async Task<IActionResult> Detail(int id)
        {
            Race race = await _raceInterface.GetByIdAsync(id);
            return View(race);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Race race)
        {
            if (!ModelState.IsValid)
            {
                return View(race);
            }
            _raceInterface.Add(race);
            return RedirectToAction("Index");
        }
    }
}
