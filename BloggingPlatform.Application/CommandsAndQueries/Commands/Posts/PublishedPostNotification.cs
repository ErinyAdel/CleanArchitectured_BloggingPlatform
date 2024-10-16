using BloggingPlatform.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloggingPlatform.Application.CommandsAndQueries.Commands.Posts
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
