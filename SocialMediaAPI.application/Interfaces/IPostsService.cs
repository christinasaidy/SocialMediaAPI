using SocialMediaAPI.domain.entities;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialMediaAPI.Application.Interfaces
{
    public interface IPostsService
    {
        // Existing methods for handling posts
        Task<Posts> GetPostByIdAsync(int id);
        Task<IEnumerable<Posts>> GetPostsByCategoryIdAsync(int categoryId);
        Task<IEnumerable<Posts>> GetPostsByUserIdAsync(int userId);
        Task<Posts> AddPostAsync(Posts post);
        Task<Posts> UpdatePostAsync(Posts post);
        Task DeletePostAsync(int id);
        Task<IEnumerable<Posts>> GetTopPostsByUpvotesAsync(int count);
        Task<IEnumerable<Posts>> GetLatestPostsAsync(int count, int offset);
        Task<int> GetTotalPostsCountAsync();

        // New methods for handling images
        Task AddImagesToPostAsync(int postId, IEnumerable<string> imagePaths);  
        Task<IEnumerable<Images>> GetImagesByPostIdAsync(int postId);

        //New method for handling search bar
        Task<IEnumerable<Posts>> SearchPostsAsync(string query);
    }
}
