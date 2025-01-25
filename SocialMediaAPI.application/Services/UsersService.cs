using SocialMediaAPI.application.Interfaces;
using SocialMediaAPI.Application.Interfaces;
using SocialMediaAPI.domain.entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMediaAPI.application.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IPostsRepository _postsRepository;

        public UsersService(IUsersRepository usersRepository, IPostsRepository postsRepository)
        {
            _usersRepository = usersRepository;
            _postsRepository = postsRepository;
        }

        public async Task<Users> GetUserByIdAsync(int userId)
        {
            return await _usersRepository.GetUserByIdAsync(userId);
        }

        public async Task<Users> CreateUserAsync(Users user)
        {
            return await _usersRepository.AddUserAsync(user);
        }

        public async Task<Users> UpdateUserAsync(Users user)
        {
            return await _usersRepository.UpdateUserAsync(user);
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            return await _usersRepository.DeleteUserAsync(userId);
        }

        public async Task<Users?> GetUserByUsernameAsync(string username)
        {
            return await _usersRepository.GetUserByUsernameAsync(username);
        }

        public async Task<string?> GetUsernameByIdAsync(int userId)
        {
            var user = await _usersRepository.GetUserByIdAsync(userId);
            return user?.UserName;
        }

        public async Task<List<Posts>> GetPostsByUserIdAsync(int userId)
        {
            var posts = await _postsRepository.GetPostsByUserIdAsync(userId);
            return posts.ToList();
        }

        public async Task<string?> GetBioByIdAsync(int userId)
        {
            return await _usersRepository.GetBioByIdAsync(userId);
        }

        public async Task<bool> AddBioAsync(int userId, string bio)
        {
            return await _usersRepository.AddBioAsync(userId, bio);
        }

        public async Task<string?> GetProfilePictureByIdAsync(int userId)
        {
            return await _usersRepository.GetProfilePictureByIdAsync(userId);
        }

        public async Task<bool> AddProfilePictureAsync(int userId, string profilePictureUrl)
        {
            return await _usersRepository.AddProfilePictureAsync(userId, profilePictureUrl);
        }

        public async Task<int> GetPostCountByUserIdAsync(int userId)
        {
            return await _usersRepository.GetPostCountByUserIdAsync(userId);
        }

        public async Task<int> GetCommentCountByUserIdAsync(int userId)
        {
            return await _usersRepository.GetCommentCountByUserIdAsync(userId);
        }

        public async Task<int> GetEngagementCountByUserIdAsync(int userId)
        {
            return await _usersRepository.GetEngagementCountByUserIdAsync(userId);
        }

        public async Task<bool> PatchUsernameAsync(int userId, string newUsername)
        {
            return await _usersRepository.PatchUsernameAsync(userId, newUsername);
        }

    }
}
