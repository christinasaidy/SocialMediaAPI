using SocialMediaAPI.application.Interfaces;
using SocialMediaAPI.domain.entities;
using System.Collections.Generic;
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

    }
}


















