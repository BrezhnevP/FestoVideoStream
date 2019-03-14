using FestoVideoStream.Data;
using FestoVideoStream.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FestoVideoStream.Services
{
    public class UsersService
    {
        private readonly AppDbContext _context;

        public UsersService(AppDbContext context)
        {
            _context = context;
        }

        public IQueryable<User> GetUsers()
        {
            var users = _context.Users;

            return users;
        }

        public async Task<User> GetUser(Guid id)
        {
            var user = await _context.Users.SingleOrDefaultAsync(d => d.Id == id);

            return user;
        }

        public async Task<User> GetUser(string login)
        {
            var user = await _context.Users.SingleOrDefaultAsync(d => d.Login == login);

            return user;
        }

        public async Task<User> GetUser(string login, string password)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Login == login && u.Password == password);

            return user;
        }

        public async Task<User> CreateUser(User user)
        {
            var insertedUser = await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return insertedUser.Entity;
        }

        public async Task<User> UpdateUser(Guid id, User user)
        {
            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }

            return user;
        }

        public async Task<bool> DeleteUser(Guid id)
        {
            var User = await _context.Users.FindAsync(id);
            if (User == null)
            {
                return false;
            }

            _context.Users.Remove(User);
            await _context.SaveChangesAsync();

            return true;
        }

        private bool UserExists(Guid id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}