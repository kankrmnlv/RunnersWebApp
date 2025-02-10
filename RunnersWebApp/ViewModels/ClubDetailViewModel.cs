using RunnersWebApp.Models;

namespace RunnersWebApp.ViewModels
{
    public class ClubDetailViewModel
    {
        public Club Club { get; set; }
        public List<Club> OtherClubs { get; set; } // List of other clubs
    }

}
