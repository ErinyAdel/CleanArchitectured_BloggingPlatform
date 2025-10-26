﻿using MediatR;

namespace BloggingPlatform.Application.CommandsAndQueries.Commands.Posts
{
	public class SendCreatedPostNotificationHandler : INotificationHandler<PublishedPostNotification>
	{
		public async Task Handle(PublishedPostNotification notification, CancellationToken cancellationToken)
		{
			// TODO
			Console.WriteLine($"Sending push notification about post '{notification.Title}'");
		}
	}
}
