using RunnersWebApp.Models;

namespace RunnersWebApp.Interfaces
{
    public interface IUserInterface
    {
        Task<List<Race>> GetAllUserRaces();
        Task<List<Club>> GetAllUserClubs();
    }
}
