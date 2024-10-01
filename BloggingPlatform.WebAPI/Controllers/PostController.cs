using BloggingPlatform.Application.Commands.Posts;
using BloggingPlatform.Application.Queries.Posts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BloggingPlatform.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : Controller
    {
        private readonly IMediator _mediator;

        public PostController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostCommand command)
        {
            var postId = await _mediator.Send(command);
            return Ok(new { Id = postId });
        }

        [HttpGet("{postId}")]
        public async Task<IActionResult> GetPost(int postId)
        {
            var query = new GetPostQuery(postId);
            var postDto = await _mediator.Send(query);
            if (postDto == null)
            {
                return NotFound(new { Message = "Post not found" });
            }
            return Ok(postDto);
        }
    }
}
