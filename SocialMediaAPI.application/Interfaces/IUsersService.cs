using SocialMediaAPI.domain.entities;

namespace SocialMediaAPI.application.Interfaces
{
    public interface IUsersService
    {
        Task<Users> GetUserByIdAsync(int userId);
        Task<Users> CreateUserAsync(Users user);
        Task<Users> UpdateUserAsync(Users user);
        Task<bool> DeleteUserAsync(int userId);

        // New Method
        Task<Users?> GetUserByUsernameAsync(string username);
    }
}
