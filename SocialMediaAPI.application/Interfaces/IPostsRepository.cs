using SocialMediaAPI.domain.entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialMediaAPI.Application.Interfaces
{
    public interface IPostsRepository
    {
        // Existing methods for handling posts
        Task<Posts> GetPostByIdAsync(int id);
        Task<IEnumerable<Posts>> GetPostsByCategoryIdAsync(int categoryId);
        Task<IEnumerable<Posts>> GetPostsByUserIdAsync(int userId);
        Task<Posts> AddPostAsync(Posts post);
        Task<Posts> UpdatePostAsync(Posts post);
        Task DeletePostAsync(int id);
        Task<IEnumerable<Posts>> GetPostsSortedByUpvotesAsync(int count);
        Task<IEnumerable<Posts>> GetLatestPostsAsync(int count, int offset);
        Task<int> GetTotalPostsCountAsync();

        // New methods for handling images
        Task AddImagesToPostAsync(int postId, IEnumerable<string> imagePaths);  // Adds images to a post
        Task<IEnumerable<Images>> GetImagesByPostIdAsync(int postId);  // Retrieves images for a specific post
    }
}
