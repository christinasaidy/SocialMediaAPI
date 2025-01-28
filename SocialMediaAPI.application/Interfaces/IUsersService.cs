using SocialMediaAPI.domain.entities;

namespace SocialMediaAPI.application.Interfaces
{
    public interface IUsersService
    {
        Task<Users> GetUserByIdAsync(int userId);
        Task<Users> CreateUserAsync(Users user);
        Task<Users> UpdateUserAsync(Users user);
        Task<bool> DeleteUserAsync(int userId, string password);
        Task<Users?> GetUserByUsernameAsync(string username);
        Task<string?> GetUsernameByIdAsync(int userId);
        Task<string?> GetEmailByIdAsync(int userId);
        Task<List<Posts>> GetPostsByUserIdAsync(int userId);
        Task<string?> GetBioByIdAsync(int userId);
        Task<bool> AddBioAsync(int userId, string bio);
        Task<string?> GetProfilePictureByIdAsync(int userId); 
        Task<bool> AddProfilePictureAsync(int userId, string profilePictureUrl);
        Task<int> GetPostCountByUserIdAsync(int userId);
        Task<int> GetCommentCountByUserIdAsync(int userId);
        Task<int> GetEngagementCountByUserIdAsync(int userId);
        Task<bool> PatchUsernameAsync(int userId, string newUsername);
        Task<bool> PatchEmailAsync(int userId, string newEmail);
        Task<bool> PatchPasswordAsync(int userId, string currentPassword, string newPassword);

    }
}
