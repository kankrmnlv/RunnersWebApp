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
        public ClubController(IClubInterface clubInterface, IPhotoInterface photoInterface)
        {
            _clubInterface = clubInterface;
            _photoInterface = photoInterface;
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
    }
}
