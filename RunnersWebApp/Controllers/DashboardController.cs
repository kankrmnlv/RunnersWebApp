using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using RunnersWebApp.Data;
using RunnersWebApp.Interfaces;
using RunnersWebApp.Models;
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
        
        private void MapUserEdit(AppUser user, EditUserProfileViewModel editVM, ImageUploadResult photoResult)
        {
            user.Id = editVM.Id;
            user.Pace = editVM.Pace;
            user.Mileage = editVM.Mileage;
            user.ProfilePictureUrl = photoResult.Url.ToString();
            user.City = editVM.City;
            user.Country = editVM.Country;
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

        [HttpPost]
        public async Task<IActionResult> EditUserProfile(EditUserProfileViewModel editUserVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit profile");
                return View("EditUserProfile", editUserVM);
            }

            AppUser user = await _userInterface.GetUserByIdNoTracking(editUserVM.Id);

            if(user.ProfilePictureUrl == "" || user.ProfilePictureUrl == null)
            {
                var photoResult = await _photoInterface.AddPhotoAsync(editUserVM.Image);
                MapUserEdit(user, editUserVM, photoResult);

                _userInterface.Update(user);
                return RedirectToAction("Index");
            }
            else
            {
                try
                {
                    await _photoInterface.DeletePhotoAsync(user.ProfilePictureUrl);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Could not delete photo");
                    return View(editUserVM);
                }
                var photoResult = await _photoInterface.AddPhotoAsync(editUserVM.Image);
                MapUserEdit(user, editUserVM, photoResult);

                _userInterface.Update(user);
                return RedirectToAction("Index");
            }
        }
    }
}
