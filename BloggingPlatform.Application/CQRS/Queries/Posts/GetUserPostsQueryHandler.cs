using BloggingPlatform.Application.Helpers.Response;
using BloggingPlatform.Application.Repositories.Posts;
using BloggingPlatform.DTO.DTO.Post;
using MediatR;

namespace BloggingPlatform.Application.CQRS.Queries.Posts
{
    public class GetUserPostsQueryHandler : IRequestHandler<GetUserPostsQuery, ResponseModel<List<PostDTO>>>
    {
        private readonly IPostRepository _postRepository;

        public GetUserPostsQueryHandler(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task<ResponseModel<List<PostDTO>>> Handle(GetUserPostsQuery request, CancellationToken cancellationToken)
        {
            var posts = await _postRepository.GetAllByEmailAsync(request.Email);
            ResponseModel<List<PostDTO>> res = new ResponseModel<List<PostDTO>>()
            {
                IsSuccess = posts.Item1.Count() > 0 ? true : false,
                Data = posts.Item1,
                ErrorMessage = posts.Item2,
            };

            return res;
        }
    } 
}
