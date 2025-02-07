using RunnersWebApp.Models;

namespace RunnersWebApp.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Club> Clubs { get; set; }
        public string Country { get; set; }
        public string City { get; set; }

    }
}
