using AutoMapper;
using BloggingPlatform.Application.CQRS.Queries.Posts;
using BloggingPlatform.Application.Repositories.Posts;
using BloggingPlatform.Domain.Entities;
using BloggingPlatform.Persistence.Data;
using Microsoft.Extensions.Logging;

namespace BloggingPlatform.Persistence.Repositories.Posts
{
    public class PostRepository : IPostRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<PostRepository> _logger;

        public PostRepository(ApplicationDbContext context, IMapper mapper, ILogger<PostRepository> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task AddAsync(Post post)
        {
            try
            {
                _logger.LogError($"Start:: Application ==> PostRepository ==> AddAsync. Model: {post}");

                _context.Posts.Add(post);
                await _context.SaveChangesAsync();
                
                _logger.LogError($"End:: In Application ==> PostRepository ==> AddAsync. Model: {post}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error:: Application ==> PostRepository ==> AddAsync. Model: {post}");
            }
        }
        
        public async Task<GetPostQuery> GetByIdAsync(int postId)
        {
            try
            {
                _logger.LogError($"Start:: Application ==> PostRepository ==> GetByIdAsync. postId: {postId}");

                var findPost = await _context.Posts.FindAsync(postId);
                if (findPost == null) {
                    _logger.LogError($"Post with Id {postId} not found.");
                    return null;
                }

                var postDto = _mapper.Map<GetPostQuery>(findPost);

                _logger.LogError($"End:: In Application ==> PostRepository ==> GetByIdAsync. postId: {postId}");
                return postDto;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error:: Application ==> PostRepository ==> GetByIdAsync. postId: {postId}");
                return null;
            }
        }
    }
}
