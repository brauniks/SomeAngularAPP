using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class DatingRepository : IDatingRepository

    {
        private readonly DataContext _context ;
        public DatingRepository(DataContext context)
        {
            this._context = context;
        }


        public void Add<T>(T enity) where T : class
        {
            _context.Add(enity);
        }

        public void Delete<T>(T enity) where T : class
        {
           _context.Remove(enity);
        }

        public Task<User> GetUser(int id)
        {
            var user = _context.Users.Include(p => p.Photos).FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            var users = await _context.Users.Include(p => p.Photos).ToListAsync();
            return users;
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}