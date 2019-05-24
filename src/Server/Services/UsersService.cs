using FestoVideoStream.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using FestoVideoStream.Models.Entities;

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

        public async Task<User> CreateUser(User user)
        {
            user.PasswordHash = ConvertPassword(user.Password);
            var insertedUser = await this.context.Users.AddAsync(user);
            await this.context.SaveChangesAsync();

            return insertedUser.Entity;
        }

        public async Task<User> UpdateUser(Guid id, User user)
        {
            user.PasswordHash = ConvertPassword(user.Password);

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

        public bool CheckUser(string login, string password)
        {
            var user = this.GetUser(login).Result;
            return user != default(User) && !CheckPassword(password, user.PasswordHash);
        }

        private static string ConvertPassword(string password)
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 3000);
            byte[] hash = pbkdf2.GetBytes(20);
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);
            string hashedPassword = Convert.ToBase64String(hashBytes);

            return hashedPassword;
        }

        private static bool CheckPassword(string password, string savedPasswordHash)
        {
            byte[] hashBytes = Convert.FromBase64String(savedPasswordHash);
            var salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 3000);
            byte[] hash = pbkdf2.GetBytes(20);
            /* Compare the results */
            for (int i = 0; i < 20; i++)
                if (hashBytes[i + 16] != hash[i])
                    return false;
            return true;
        }

        private bool UserExists(Guid id)
        {
            return this.context.Users.Any(e => e.Id == id);
        }
    }
}