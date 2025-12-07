using BloggingPlatform.Application.CQRS.Queries.Posts;
using BloggingPlatform.Domain.Entities;
using BloggingPlatform.DTO.DTO.Post;

namespace BloggingPlatform.Application.Repositories.Posts
{
    public interface IPostRepository
    {
        Task AddAsync(Post post);
        Task<GetPostQuery> GetByIdAsync(int postId);
        Task<(List<PostDTO>, string)> GetAllByEmailAsync(string email);
    }
}
