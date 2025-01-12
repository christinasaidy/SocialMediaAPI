using SocialMediaAPI.application.Interfaces;
using SocialMediaAPI.domain.entities;

namespace SocialMediaAPI.application.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _usersRepository;

        public UsersService(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
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

    }
}
