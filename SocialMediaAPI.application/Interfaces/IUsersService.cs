using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialMediaAPI.domain.entities;


namespace SocialMediaAPI.application.Interfaces
{
    public interface IUsersService
    {
        Task<Users> GetUserByIdAsync(int userId);
        Task<Users> CreateUserAsync(Users user);
        Task<Users> UpdateUserAsync(Users user);
        Task<bool> DeleteUserAsync(int userId);
    }
}
