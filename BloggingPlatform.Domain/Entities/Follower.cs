using BloggingPlatform.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloggingPlatform.Domain.Entities
{
    public class Follower : BaseEntityManyToMany
    {
        public string FollowerUserId { get; set; }
        public string FollowedUserId { get; set; }

        public ApplicationUser FollowerUser { get; set; }
        public ApplicationUser FollowedUser { get; set; }
    }
}
