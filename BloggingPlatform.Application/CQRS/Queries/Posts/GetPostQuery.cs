using MediatR;

namespace BloggingPlatform.Application.CQRS.Queries.Posts
{
    public class GetPostQuery : IRequest<GetPostQuery>
    {
        public int PostId { get; set; }

        public string Title { get; set; }
        public string Content { get; set; }

        public GetPostQuery()
        {
            
        }

        public GetPostQuery(int postId)
        {
            PostId = postId;
        }
    }
}
