using BloggingPlatform.Application.Repositories.Posts;
using BloggingPlatform.Domain.Entities;
using MediatR;

namespace BloggingPlatform.Application.CommandsAndQueries.Commands.Posts
{
    public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, int>
    {
        private readonly IPostRepository _postRepository;
		private readonly IMediator _mediator;

		public CreatePostCommandHandler(IPostRepository postRepository, IMediator mediator)
        {
            _postRepository = postRepository;
            _mediator = mediator;

		}

        public async Task<int> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            var post = new Post
            {
                Title = request.Title,
                Content = request.Content,
                AuthorId = request.AuthorId,
            };

            await _postRepository.AddAsync(post);

			var notification = new PublishedPostNotification
			{
				PostId = post.Id,
				Title = post.Title,
				Content = post.Content,
				AuthorId = post.AuthorId
			};

			await _mediator.Publish(notification);

			return post.Id;
        }
    }
}
