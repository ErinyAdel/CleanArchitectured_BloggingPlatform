using BloggingPlatform.Application.Commands.Users;
using BloggingPlatform.Application.Constants;
using BloggingPlatform.Application.DTOs.AuthenticationDTOs;
using BloggingPlatform.Application.DTOs.PostsDTOs;
using BloggingPlatform.Application.DTOs.UserDTOs;
using BloggingPlatform.Application.Helpers.Response;
using BloggingPlatform.Application.Interfaces;
using BloggingPlatform.Application.Queries.Posts;
using BloggingPlatform.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloggingPlatform.Persistence.Services
{
    public class PostService : IPostService
    {
        private readonly IMediator _mediator;

        public PostService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<ResponseModel<PostDTO>> GetPostAsync(int postId)
        {
            var query = new GetPostQuery(postId);
            var postDto = await _mediator.Send(query);

            if (postDto == null)
            {
                return DynamicResponse<PostDTO>.Failed(null, errorCode: (int)ResponseStatusCode.NotFound, errorMessage: "Post not found");
            }

            return DynamicResponse<PostDTO>.Success(postDto);
        }
    }
}
