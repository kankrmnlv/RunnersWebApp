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

        public async Task<IActionResult> Edit(int id)
        {
            var race = await _raceInterface.GetByIdAsync(id);
            if (race == null) return View("Error");
            var raceVM = new EditRaceViewModel
            {
                Title = race.Title,
                Description = race.Description,
                AddressId = race.AddressId,
                Address = race.Address,
                URL = race.Image,
                RaceCategory = race.RaceCategory
            };
            return View(raceVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditRaceViewModel raceVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit race");
                return View("Edit", raceVM);
            }

            var userClub = await _raceInterface.GetByIdAsyncNoTracking(id);
            if (userClub != null)
            {
                try
                {
                    await _photoInterface.DeletePhotoAsync(userClub.Image);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Could not delete photo");
                    return View(raceVM);
                }
                var photoResult = await _photoInterface.AddPhotoAsync(raceVM.Image);
                var race = new Race
                {
                    Id = id,
                    Title = raceVM.Title,
                    Description = raceVM.Description,
                    Image = photoResult.Url.ToString(),
                    AddressId = raceVM.AddressId,
                    Address = raceVM.Address
                };
                _raceInterface.Update(race);
                return RedirectToAction("Index");
            }
            else
            {
                return View(raceVM);
            }
        }
    }
}
