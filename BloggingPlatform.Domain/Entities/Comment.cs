using BloggingPlatform.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloggingPlatform.Domain.Entities
{
    public class Comment : BaseEntity
    {
        public string CommentText { get; set; }
        public int PostId { get; set; }
        public string AuthorId { get; set; }

        public Post Post { get; set; }
        public ApplicationUser Author { get; set; }
    }
}
