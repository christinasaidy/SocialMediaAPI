using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialMediaAPI.domain.entities;
namespace SocialMediaAPI.application.Interfaces
{
    public interface ICommentsService
    {
        Task<Comments> GetCommentByIdAsync(int id);
        Task<IEnumerable<Comments>> GetCommentsByPostIdAsync(int postId);
        Task<Comments> AddCommentAsync(Comments comment);
        Task<Comments> UpdateCommentAsync(Comments comment);
        Task DeleteCommentAsync(int id);
        Task<Posts> GetPostByIdAsync(int postId);
        Task<Users> GetUserByIdAsync(int userId);
    }
}
