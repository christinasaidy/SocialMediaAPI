using SocialMediaAPI.application.Interfaces;
using SocialMediaAPI.domain.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaAPI.application.Services
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
            return await _postsRepository.GetPostsSortedByUpvotesAsync(count);  // Call the repository to fetch posts
        }
    }
}
