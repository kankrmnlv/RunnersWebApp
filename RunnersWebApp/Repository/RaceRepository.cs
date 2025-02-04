using Microsoft.EntityFrameworkCore;
using RunnersWebApp.Data;
using RunnersWebApp.Interfaces;
using RunnersWebApp.Models;

namespace RunnersWebApp.Repository
{
    public class RaceRepository : IRaceInterface
    {
        private readonly ApplicationDbContext _context;
        public RaceRepository(ApplicationDbContext context)
        {
            _context = context;

        }

        public bool Add(Race race)
        {
            _context.Add(race);
            return Save();
        }

        public bool Delete(Race race)
        {
            _context.Remove(race);
            return Save();
        }

        public async Task<IEnumerable<Race>> GetAll()
        {
            return await _context.Races.ToListAsync();
        }

        public async Task<IEnumerable<Race>> GetAllRacesByCity(string city)
        {
            return await _context.Races.Where(i => i.Address.City.Contains(city)).ToArrayAsync();
        }

        public async Task<Race> GetByIdAsync(int id)
        {
            return await _context.Races.Include(i => i.Address).FirstOrDefaultAsync();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Race race)
        {
            _context.Update(race);
            return Save();
        }
    }
}
