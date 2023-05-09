using Domain.Entities;

namespace Infrastructure.Repositories.UserRepository
{
    public interface IUserRepository
    {
        Task<RegisterModel> AddUserAsync(RegisterModel user);
         Task<int?> GetUserByUsernameAsync(string username, string password);

    }
}
