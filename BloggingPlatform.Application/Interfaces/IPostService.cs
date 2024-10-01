using BloggingPlatform.Application.DTOs.PostsDTOs;
using BloggingPlatform.Application.Helpers.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloggingPlatform.Application.Interfaces
{
    public interface IPostService
    {
        Task<ResponseModel<PostDTO>> GetPostAsync(int postId);
    }
}
