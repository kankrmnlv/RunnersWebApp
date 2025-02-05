using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunnersWebApp.Data;
using RunnersWebApp.Interfaces;
using RunnersWebApp.Models;
using RunnersWebApp.ViewModels;

namespace RunnersWebApp.Controllers
{
    public class RaceController : Controller
    {
        private readonly IRaceInterface _raceInterface;
        private readonly IPhotoInterface _photoInterface;
        public RaceController(IRaceInterface raceInterface, IPhotoInterface photoInterface)
        {
            _raceInterface = raceInterface;
            _photoInterface = photoInterface;
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
        public async Task<IActionResult> Create(CreateRaceViewModel raceVM)
        {
            if (ModelState.IsValid)
            {
                var result = await _photoInterface.AddPhotoAsync(raceVM.Image);
                var race = new Race
                {
                    Title = raceVM.Title,
                    Description = raceVM.Description,
                    Image = result.Url.ToString(),
                    Address = new Address
                    {
                        Street = raceVM.Address.Street,
                        City = raceVM.Address.City,
                        Country = raceVM.Address.Country,
                    }
                };

                _raceInterface.Add(race);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Photo upload failed");
            }

            return View(raceVM);
        }
    }
}
