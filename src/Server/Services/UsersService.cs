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
            this._context = context;
        }

        public IQueryable<User> GetUsers()
        {
            var users = this._context.Users;

            return users;
        }

        public async Task<User> GetUser(Guid id)
        {
            var user = await this._context.Users.SingleOrDefaultAsync(d => d.Id == id);

            return user;
        }

        public async Task<User> GetUser(string login)
        {
            var user = await this._context.Users.SingleOrDefaultAsync(d => d.Login == login);

            return user;
        }

        public async Task<User> GetUser(string login, string password)
        {
            var user = await this._context.Users.SingleOrDefaultAsync(u => u.Login == login && u.Password == password);

            return user;
        }

        public async Task<User> CreateUser(User user)
        {
            var insertedUser = await this._context.Users.AddAsync(user);
            await this._context.SaveChangesAsync();

            return insertedUser.Entity;
        }

        public async Task<User> UpdateUser(Guid id, User user)
        {
            this._context.Entry(user).State = EntityState.Modified;

            try
            {
                await this._context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!this.UserExists(id))
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
            var user = await this._context.Users.FindAsync(id);
            if (user == null)
            {
                return false;
            }

            this._context.Users.Remove(user);
            await this._context.SaveChangesAsync();

            return true;
        }

        private bool UserExists(Guid id)
        {
            return this._context.Users.Any(e => e.Id == id);
        }
    }
}