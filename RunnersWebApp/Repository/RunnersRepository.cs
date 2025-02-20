﻿using Microsoft.EntityFrameworkCore;
using RunnersWebApp.Data;
using RunnersWebApp.Interfaces;
using RunnersWebApp.Models;

namespace RunnersWebApp.Repository
{
    public class RunnersRepository : IRunnersInterface
    {
        private readonly ApplicationDbContext _context;

        public RunnersRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<AppUser>> GetAllUsers()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<AppUser> GetUserById(string id)
        {
            return await _context.Users.FindAsync(id);
        }
        public bool Add(AppUser user)
        {
            throw new NotImplementedException();
        }

        public bool Delete(AppUser user)
        {
            throw new NotImplementedException();
        }


        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(AppUser user)
        {
            _context.Update(user);
            return Save();
        }
    }
}
