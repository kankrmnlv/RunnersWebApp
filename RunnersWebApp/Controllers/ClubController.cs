using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunnersWebApp.Data;
using RunnersWebApp.Interfaces;
using RunnersWebApp.Models;
using RunnersWebApp.ViewModels;

namespace RunnersWebApp.Controllers
{
    public class ClubController : Controller
    {
        private readonly IClubInterface _clubInterface;
        private readonly IPhotoInterface _photoInterface;
        private readonly IHttpContextAccessor _contextAccessor;
        public ClubController(IClubInterface clubInterface, IPhotoInterface photoInterface, IHttpContextAccessor contextAccessor)
        {
            _clubInterface = clubInterface;
            _photoInterface = photoInterface;
            _contextAccessor = contextAccessor;
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
            var currentUserId = _contextAccessor.HttpContext?.User.GetUserId();
            var createClubViewModel = new CreateClubViewModel
            {
                AppUserId = currentUserId,
            };
            return View(createClubViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateClubViewModel clubVM)
        {
            if (ModelState.IsValid)
            {
                var result = await _photoInterface.AddPhotoAsync(clubVM.Image);

                var club = new Club
                {
                    Title = clubVM.Title,
                    Description = clubVM.Description,
                    Image = result.Url.ToString(),
                    AppUserId = clubVM.AppUserId,
                    Address = new Address
                    {
                        Street = clubVM.Address.Street,
                        City = clubVM.Address.City,
                        Country = clubVM.Address.Country
                    }
                };

                _clubInterface.Add(club);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Photo upload failed");
            }

            return View(clubVM);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var club = await _clubInterface.GetByIdAsync(id);
            if(club == null) return View("Error");
            var clubVM = new EditClubViewModel
            {
                Title = club.Title,
                Description = club.Description,
                AddressId = club.AddressId,
                Address = club.Address,
                URL = club.Image,
                ClubCategory = club.ClubCategory
            };
            return View(clubVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditClubViewModel clubVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit club");
                return View("Edit", clubVM);
            }

            var userClub = await _clubInterface.GetByIdAsyncNoTracking(id);
            if(userClub!= null)
            {
                try
                {
                    await _photoInterface.DeletePhotoAsync(userClub.Image);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Could not delete photo");
                    return View(clubVM);
                }
                var photoResult = await _photoInterface.AddPhotoAsync(clubVM.Image);
                var club = new Club
                {
                    Id = id,
                    Title = clubVM.Title,
                    Description = clubVM.Description,
                    Image = photoResult.Url.ToString(),
                    AddressId = clubVM.AddressId,
                    Address = clubVM.Address
                };
                _clubInterface.Update(club);
                return RedirectToAction("Index");
            }
            else
            {
                return View(clubVM);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var clubDetails = await _clubInterface.GetByIdAsync(id);
            if(clubDetails == null) return View("Error");
            return View(clubDetails);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteClub(int id)
        {
            var clubDetails = await _clubInterface.GetByIdAsync(id);
            if (clubDetails == null) return View("Error");

            _clubInterface.Delete(clubDetails);
            return RedirectToAction("Index");
        }
    }
}
