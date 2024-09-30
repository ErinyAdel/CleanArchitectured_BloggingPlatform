using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloggingPlatform.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime CreationDate { get; set; }
        public bool IsBlocked { get; set; }
        [NotMapped]
        public ICollection<Post> Posts { get; set; }
        [NotMapped]
        public ICollection<Comment> Comments { get; set; }
        [NotMapped]
        public ICollection<Follower> Followers { get; set; }
        [NotMapped]
        public ICollection<Follower> Following { get; set; }
    }
}
