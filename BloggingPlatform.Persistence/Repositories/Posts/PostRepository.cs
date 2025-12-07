using AutoMapper;
using BloggingPlatform.Application.CQRS.Queries.Posts;
using BloggingPlatform.Application.Repositories.Posts;
using BloggingPlatform.Domain.Entities;
using BloggingPlatform.DTO.DTO.Post;
using BloggingPlatform.Persistence.Data;
using Microsoft.EntityFrameworkCore;
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

        public async Task<(List<PostDTO>, string)> GetAllByEmailAsync(string email)
        {
            try
            {
                _logger.LogError($"Start:: Application ==> PostRepository ==> GetAllByEmailAsync. email: {email}");
                var message = string.Empty;

                var userId = _context.Users.Where(u => u.Email == email).Select(u => u.Id).FirstOrDefault();
                if(userId == null)
                {
                    message = $"User with email {{email}} not found.\"";
                    _logger.LogError(message);
                    return (null, message);
                }

                var posts = await _context.Posts.Where(p => p.AuthorId == userId).ToListAsync();
                if (posts.Count() == 0)
                {
                    message = $"User With Email '{email}' Doesn't Has Posts.";
                    _logger.LogError(message);
                    return (null, message);
                }

                var postDto = _mapper.Map<List<PostDTO>>(posts);

                _logger.LogError($"End:: In Application ==> PostRepository ==> GetByIdAsync. email: {email}");
                return (postDto, "");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error:: Application ==> PostRepository ==> GetByIdAsync. email: {email}.\n{ex.Message}");
                return (null, ex.Message);
            }
        }
    }
}
