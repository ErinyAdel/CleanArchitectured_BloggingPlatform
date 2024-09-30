using BloggingPlatform.Application.Commands.Posts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BloggingPlatform.WebAPI.Controllers
{
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
    }
}
