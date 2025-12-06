using AutoMapper;
using BloggingPlatform.Application.CQRS.Commands.Posts;
using BloggingPlatform.Application.Constants;
using BloggingPlatform.Application.CQRS.Queries.Posts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using BloggingPlatform.DTO.DTO.Post;

namespace BloggingPlatform.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public PostController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] PostDTO model)
        {
            var authorId = User.FindFirstValue(CustomClaimTypes.userId.ToString());

            var mappedCommand = _mapper.Map<CreatePostCommand>(model);
            mappedCommand.AuthorId = authorId;

            var postId = await _mediator.Send(mappedCommand);
            return Ok(new { Id = postId });
        }

        [HttpGet("{postId}")]
        public async Task<IActionResult> GetPost(int postId)
        {
            var query = new GetPostQuery(postId);
            var postDto = await _mediator.Send(query);

            if (postDto == null)
                return NotFound(new { Message = "Post not found" });
            return Ok(postDto);
        }
    }
}
