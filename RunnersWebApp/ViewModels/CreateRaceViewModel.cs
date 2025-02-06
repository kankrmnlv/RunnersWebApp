using Microsoft.Extensions.FileProviders;
using RunnersWebApp.Data.Enum;
using RunnersWebApp.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace RunnersWebApp.ViewModels
{
    public class CreateRaceViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Address Address { get; set; }
        public IFormFile Image { get; set; }
        public RaceCategory RaceCategory { get; set; }
        public string AppUserId { get; set; }
    }
}
