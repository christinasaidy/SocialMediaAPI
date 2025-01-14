using SocialMediaAPI.domain.entities;

namespace SocialMediaAPI.application.Interfaces
{
    public interface IUsersService
    {
        Task<Users> GetUserByIdAsync(int userId);
        Task<Users> CreateUserAsync(Users user);
        Task<Users> UpdateUserAsync(Users user);
        Task<bool> DeleteUserAsync(int userId);
        Task<Users?> GetUserByUsernameAsync(string username);
        Task <String?> GetUsernameByIdAsync(int userId);
        Task<List<Posts>> GetPostsByUserIdAsync(int userId);

        Task<string?> GetBioByIdAsync(int userId); 
        Task<bool> AddBioAsync(int userId, string bio); 
    }
}
