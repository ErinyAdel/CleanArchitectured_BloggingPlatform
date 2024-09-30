using BloggingPlatform.Application.Repositories.Posts;
using BloggingPlatform.Domain.Entities;
using BloggingPlatform.Persistence.Data;
using Microsoft.Build.Framework;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloggingPlatform.Persistence.Repositories.Posts
{
    public class PostRepository : IPostRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PostRepository> _logger;

        public PostRepository(ApplicationDbContext context, ILogger<PostRepository> logger)
        {
            _context = context;
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
    }
}
