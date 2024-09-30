using BloggingPlatform.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloggingPlatform.Domain.Entities
{
    public class Post : BaseEntity
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string AuthorId { get; set; }

        public ApplicationUser Author { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
