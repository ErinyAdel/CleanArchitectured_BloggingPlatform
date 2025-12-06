using BloggingPlatform.Application.CQRS.Queries.Posts;
using BloggingPlatform.Domain.Entities;

namespace BloggingPlatform.Application.Repositories.Posts
{
    public interface IPostRepository
    {
        Task AddAsync(Post post);
        Task<GetPostQuery> GetByIdAsync(int postId);
    }
}
