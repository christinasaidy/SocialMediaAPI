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
        private readonly ICommentsRepository _commentsRepository;

        public CommentsService(ICommentsRepository commentsRepository)
        {
            _commentsRepository = commentsRepository;
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
    }
}
