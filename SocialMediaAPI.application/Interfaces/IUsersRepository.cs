using SocialMediaAPI.domain.entities;

namespace SocialMediaAPI.application.Interfaces
{
    public interface IUsersRepository
    {
        Task<Users> GetUserByIdAsync(int userId);
        Task<IEnumerable<Users>> GetAllUsersAsync();
        Task<Users> AddUserAsync(Users user);
        Task<Users> UpdateUserAsync(Users user);
        Task<bool> DeleteUserAsync(int userId);
        Task<Users?> GetUserByUsernameAsync(string username);
        Task<string?> GetUsernameByIdAsync(int userId);
        Task<IEnumerable<Posts>> GetPostsByUserIdAsync(int userId);
        Task<string?> GetBioByIdAsync(int userId);
        Task<bool> AddBioAsync(int userId, string bio);
        Task<string?> GetProfilePictureByIdAsync(int userId); 
        Task<bool> AddProfilePictureAsync(int userId, string profilePictureUrl);
        Task<int> GetPostCountByUserIdAsync(int userId);
        Task<int> GetCommentCountByUserIdAsync(int userId);
        Task<int> GetEngagementCountByUserIdAsync(int userId);
        Task<bool> PatchUsernameAsync(int userId, string newUsername);
    }
}
