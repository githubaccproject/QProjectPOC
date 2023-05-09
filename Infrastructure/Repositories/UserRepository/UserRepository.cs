using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

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
          
          
            _dbContext.User.Add(newModel);
            await _dbContext.SaveChangesAsync();

            return user;
        }

        public async Task<int?> GetUserByUsernameAsync(string username, string password)
        {
            var user = await _dbContext.User.FirstOrDefaultAsync(u => u.Username == username);
           
            if (user == null)
            {
                return null;
            }

   
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                return null;
            }

         
            return user.Id;
        }


     
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



