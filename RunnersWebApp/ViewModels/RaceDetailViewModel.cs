using RunnersWebApp.Models;

namespace RunnersWebApp.ViewModels
{
    public class RaceDetailViewModel
    {
        public Race Race { get; set; }
        public List<Race> OtherRaces { get; set; }

    }
}
