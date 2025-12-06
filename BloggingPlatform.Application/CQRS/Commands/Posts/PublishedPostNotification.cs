using BloggingPlatform.Domain.Entities;
using MediatR;

namespace BloggingPlatform.Application.CQRS.Commands.Posts
{
	public class PublishedPostNotification : INotification
	{
		public int PostId { get; set; }
		public string Title { get; set; }
		public string Content { get; set; }
		public string AuthorId { get; set; }
		public List<Follower> Followers { get; set; }
	}
}
