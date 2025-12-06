using MediatR;

namespace BloggingPlatform.Application.CQRS.Commands.Posts
{
    public class CreatePostCommand : IRequest<int>
    {
        public string Title { get; set; }
        public string Content { get; set; }

        public string AuthorId;
    }
}
