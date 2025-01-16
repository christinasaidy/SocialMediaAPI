using SocialMediaAPI.domain.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaAPI.application.Interfaces
{
    public interface IPostsRepository
    {
        Task<Posts> GetPostByIdAsync(int id);
        Task<IEnumerable<Posts>> GetPostsByCategoryIdAsync(int categoryId);
        Task<IEnumerable<Posts>> GetPostsByUserIdAsync(int userId);
        Task<Posts> AddPostAsync(Posts post);
        Task<Posts> UpdatePostAsync(Posts post);
        Task DeletePostAsync(int id);
        Task<IEnumerable<Posts>> GetPostsSortedByUpvotesAsync(int count);
        Task<IEnumerable<Posts>> GetLatestPostsAsync(int count, int offset);

        Task<int> GetTotalPostsCountAsync();

    }
}
