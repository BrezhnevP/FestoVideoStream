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
        private readonly AppDbContext context;

        public UsersService(AppDbContext context)
        {
            this.context = context;
        }

        public IQueryable<User> GetUsers()
        {
            var users = this.context.Users;

            return users;
        }

        public async Task<User> GetUser(Guid id)
        {
            var user = await this.context.Users.SingleOrDefaultAsync(d => d.Id == id);

            return user;
        }

        public async Task<User> GetUser(string login)
        {
            var user = await this.context.Users.SingleOrDefaultAsync(d => d.Login == login);

            return user;
        }

        public async Task<User> GetUser(string login, string password)
        {
            var user = await this.context.Users.SingleOrDefaultAsync(u => u.Login == login && u.Password == password);

            return user;
        }

        public async Task<User> CreateUser(User user)
        {
            var insertedUser = await this.context.Users.AddAsync(user);
            await this.context.SaveChangesAsync();

            return insertedUser.Entity;
        }

        public async Task<User> UpdateUser(Guid id, User user)
        {
            this.context.Entry(user).State = EntityState.Modified;

            try
            {
                await this.context.SaveChangesAsync();
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
            var user = await this.context.Users.FindAsync(id);
            if (user == null)
            {
                return false;
            }

            this.context.Users.Remove(user);
            await this.context.SaveChangesAsync();

            return true;
        }

        private bool UserExists(Guid id)
        {
            return this.context.Users.Any(e => e.Id == id);
        }
    }
}