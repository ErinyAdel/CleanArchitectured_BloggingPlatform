using BloggingPlatform.Application.DTOs.PostsDTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloggingPlatform.Application.Queries.Posts
{
    public class GetPostQuery : IRequest<PostDTO>
    {
        public int PostId { get; set; }

        public GetPostQuery(int postId)
        {
            PostId = postId;
        }
    }
}
