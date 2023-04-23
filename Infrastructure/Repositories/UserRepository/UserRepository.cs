using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories.UserRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.UserRepository
{
    public class UserRepository : IUserRepository
    {

        private readonly ApplicationDBContext _dbContext; // Inject your DbContext as a dependency

        public UserRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<RegisterModel> AddUserAsync(RegisterModel user)
        {
            // Generate password salt and hash
            byte[] passwordSalt, passwordHash;
            GeneratePasswordHash(user.Password, out passwordSalt, out passwordHash);

            var newModel = new User()
            {
                Username = user.Username,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                LastName = user.LastName,
                FirstName = user.FirstName,
            };
          
            // Add the user to the database
            _dbContext.User.Add(newModel);
            await _dbContext.SaveChangesAsync();

            return user;
        }

        public async Task<int?> GetUserByUsernameAsync(string username, string password)
        {
            var user = await _dbContext.User.FirstOrDefaultAsync(u => u.Username == username);
            // If no user found, return null
            if (user == null)
            {
                return null;
            }

            // Validate the password
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                return null;
            }

            // Return the user ID if username and password are valid
            return user.Id;
        }


        // Method to verify the password hash
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i])
                    {
                        return false;
                    }
                }
            }
            return true;
        }


        // Method to generate password salt and hash
        private void GeneratePasswordHash(string password, out byte[] passwordSalt, out byte[] passwordHash)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

    }


}



