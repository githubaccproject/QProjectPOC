using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.UserRepository
{
    public interface IUserRepository
    {
        Task<RegisterModel> AddUserAsync(RegisterModel user);
         Task<int?> GetUserByUsernameAsync(string username, string password);

    }
}
