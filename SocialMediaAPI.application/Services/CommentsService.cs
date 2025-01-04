using SocialMediaAPI.application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialMediaAPI.domain.entities;


namespace SocialMediaAPI.application.Services
{
    public class CommentsService : ICommentsService
    {
        private readonly IPostsService _postService;
        private readonly IUsersService _userService;
        private readonly ICommentsRepository _commentsRepository;

        public CommentsService(IPostsService postService, IUsersService userService, ICommentsRepository commentRepository)
        {
            _postService = postService;
            _userService = userService;
            _commentsRepository = commentRepository;
        }

        public async Task<Comments> GetCommentByIdAsync(int id)
        {
            return await _commentsRepository.GetCommentByIdAsync(id);
        }

        public async Task<IEnumerable<Comments>> GetCommentsByPostIdAsync(int postId)
        {
            return await _commentsRepository.GetCommentsByPostIdAsync(postId);
        }

        public async Task<Comments> AddCommentAsync(Comments comment)
        {
            return await _commentsRepository.AddCommentAsync(comment);
        }

        public async Task<Comments> UpdateCommentAsync(Comments comment)
        {
            return await _commentsRepository.UpdateCommentAsync(comment);
        }

        public async Task DeleteCommentAsync(int id)
        {
            await _commentsRepository.DeleteCommentAsync(id);
        }


        public async Task<Posts> GetPostByIdAsync(int postId)
        {
            return await _postService.GetPostByIdAsync(postId); // Call the method in IPostService
        }

        public async Task<Users> GetUserByIdAsync(int userId)
        {
            return await _userService.GetUserByIdAsync(userId); // Call the method in IUserService
        }
    }
}
