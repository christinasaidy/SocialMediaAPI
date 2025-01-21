using SocialMediaAPI.Application.Interfaces;
using SocialMediaAPI.domain.entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialMediaAPI.Application.Services
{
    public class PostsService : IPostsService
    {
        private readonly IPostsRepository _postsRepository;

        public PostsService(IPostsRepository postsRepository)
        {
            _postsRepository = postsRepository;
        }

        public async Task<Posts> GetPostByIdAsync(int id)
        {
            return await _postsRepository.GetPostByIdAsync(id);
        }

        public async Task<IEnumerable<Posts>> GetPostsByCategoryIdAsync(int categoryId)
        {
            return await _postsRepository.GetPostsByCategoryIdAsync(categoryId);
        }

        public async Task<IEnumerable<Posts>> GetPostsByUserIdAsync(int userId)
        {
            return await _postsRepository.GetPostsByUserIdAsync(userId);
        }

        public async Task<Posts> AddPostAsync(Posts post)
        {
            return await _postsRepository.AddPostAsync(post);
        }

        public async Task<Posts> UpdatePostAsync(Posts post)
        {
            return await _postsRepository.UpdatePostAsync(post);
        }

        public async Task DeletePostAsync(int id)
        {
            await _postsRepository.DeletePostAsync(id);
        }

        public async Task<IEnumerable<Posts>> GetTopPostsByUpvotesAsync(int count)
        {
            return await _postsRepository.GetPostsSortedByUpvotesAsync(count);
        }

        public async Task<IEnumerable<Posts>> GetLatestPostsAsync(int count, int offset)
        {
            return await _postsRepository.GetLatestPostsAsync(count, offset);
        }

        public async Task<int> GetTotalPostsCountAsync()
        {
            return await _postsRepository.GetTotalPostsCountAsync();
        }

        // New methods for handling images
        public async Task AddImagesToPostAsync(int postId, IEnumerable<string> imagePaths)
        {
            await _postsRepository.AddImagesToPostAsync(postId, imagePaths);
        }

        public async Task<IEnumerable<Images>> GetImagesByPostIdAsync(int postId)
        {
            return await _postsRepository.GetImagesByPostIdAsync(postId);
        }
        public async Task<IEnumerable<Posts>> SearchPostsAsync(string query)
        {
            return await _postsRepository.SearchPostsAsync(query);
        }

    }
}
