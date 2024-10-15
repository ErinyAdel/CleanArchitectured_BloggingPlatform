using BloggingPlatform.Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloggingPlatform.Application.CommandsAndQueries.Queries.Posts
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
