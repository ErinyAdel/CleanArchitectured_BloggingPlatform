using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloggingPlatform.Application.Commands.Posts
{
    public class CreatePostCommand : IRequest<int> /* Returns Id on Success */
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string AuthorId { get; set; }
    }
}
