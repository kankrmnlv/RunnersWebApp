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
        private readonly IHttpContextAccessor _contextAccessor;
        public RaceController(IRaceInterface raceInterface, IPhotoInterface photoInterface, IHttpContextAccessor contextAccessor)
        {
            _raceInterface = raceInterface;
            _photoInterface = photoInterface;
            _contextAccessor = contextAccessor;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Race> races = await _raceInterface.GetAll();

            return View(races);
        }

        public async Task<IActionResult> Detail(int id)
        {
            Race race = await _raceInterface.GetByIdAsync(id);
            if(race == null)
            {
                return NotFound();
            }

            List<Race> otherRaces = (await _raceInterface.GetAll()).Where(c => c.Id != id).Take(3).ToList();

            var viewModel = new RaceDetailViewModel
            {
                Race = race,
                OtherRaces = otherRaces
            };

            return View(viewModel);
        }

        public IActionResult Create()
        {
            var currentUserId = _contextAccessor.HttpContext?.User.GetUserId();
            var createRaceViewModel = new CreateRaceViewModel
            {
                AppUserId = currentUserId,
            };
            return View(createRaceViewModel);
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
                    AppUserId = raceVM.AppUserId,
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

        public async Task<IActionResult> Delete(int id)
        {
            var raceDetails = await _raceInterface.GetByIdAsync(id);
            if(raceDetails == null) return View("Error");
            return View(raceDetails);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteRace(int id)
        {
            var raceDetails = await _raceInterface.GetByIdAsync(id);
            if (raceDetails == null) return View("Error");

            _raceInterface.Delete(raceDetails);
            return RedirectToAction("Index");
        }
    }
}
