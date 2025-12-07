using BloggingPlatform.Application.CQRS.Commands.Users;
using BloggingPlatform.Application.Constants;
using BloggingPlatform.DTO.DTO.User;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using AutoMapper;
using BloggingPlatform.Application.CQRS.Queries.Users;

namespace BloggingPlatform.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IValidator<RegisterDTO> _registerValidator;
        private readonly IValidator<LoginDTO> _loginValidator;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public UserController(IValidator<RegisterDTO> registerValidator, IValidator<LoginDTO> loginValidator, 
            IMediator mediator, IMapper mapper)
        {
            _registerValidator = registerValidator;
            _loginValidator = loginValidator;
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO model)
        {
            ValidationResult resultVal = await _registerValidator.ValidateAsync(model);
            if (!resultVal.IsValid)
                return BadRequest(ModelState);

            var command = _mapper.Map<RegisterUserCommand>(model);
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return StatusCode(result?.ErrorCode != null && result.ErrorCode != 0 ? (int)result.ErrorCode : (int)ResponseStatusCode.NoContent, new { Message = result.ErrorMessage });
            return Ok(result);
        }
        
        [HttpPost]
        [AllowAnonymous]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {
            ValidationResult resultVal = await _loginValidator.ValidateAsync(model);
            if (!resultVal.IsValid)
                return BadRequest(ModelState);

            var command = _mapper.Map<UserLoginQuery>(model);
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return StatusCode(result?.ErrorCode != null && result.ErrorCode != 0 ? (int)result.ErrorCode : (int)ResponseStatusCode.NoContent, new { Message = result.ErrorMessage });
            return Ok(result);
        }
    }
}
