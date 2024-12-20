﻿using BloggingPlatform.Application.CommandsAndQueries.Queries.Posts;
using BloggingPlatform.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloggingPlatform.Application.Repositories.Posts
{
    public interface IPostRepository
    {
        Task AddAsync(Post post);
        Task<GetPostQuery> GetByIdAsync(int postId);
    }
}
