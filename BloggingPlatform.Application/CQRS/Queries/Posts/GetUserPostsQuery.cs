using System;
using BloggingPlatform.Application.Helpers.Response;
using BloggingPlatform.DTO.DTO.Post;
using MediatR;
namespace BloggingPlatform.Application.CQRS.Queries.Posts
{
    public class GetUserPostsQuery : IRequest<ResponseModel<List<PostDTO>>>
    {
        public string Email { get; set; }

        public GetUserPostsQuery(string email)
        {
            Email = email;
        }
    }
}
