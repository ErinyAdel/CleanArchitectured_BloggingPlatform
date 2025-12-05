using BloggingPlatform.Application.CommandsAndQueries.Commands.Users;
using BloggingPlatform.Application.Constants;
using BloggingPlatform.Application.DTOs.UserDTOs;
using BloggingPlatform.Application.Validators.UsersValidators;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BloggingPlatform.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly RegisterUserValidator _registerValidator;
        private readonly UserLoginValidator _loginValidator;
        private readonly IMediator _mediator;

        public UserController(RegisterUserValidator registerValidator, UserLoginValidator loginValidator, IMediator mediator)
        {
            _registerValidator = registerValidator;
            _loginValidator = loginValidator;
            _mediator = mediator;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO model)
        {
            var command = new RegisterUserCommand
            {
                UserName = model.UserName,
                Email = model.Email,
                Password = model.Password
            };

            ValidationResult resultVal = await _registerValidator.ValidateAsync(command);

            if (!resultVal.IsValid)
                return BadRequest(ModelState);

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
            var command = new UserLoginCommand
            {
                Email = model.Email,
                Password = model.Password
            };

            ValidationResult resultVal = await _loginValidator.ValidateAsync(command);

            if (!resultVal.IsValid)
                return BadRequest(ModelState);

            
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return StatusCode(result?.ErrorCode != null && result.ErrorCode != 0 ? (int)result.ErrorCode : (int)ResponseStatusCode.NoContent, new { Message = result.ErrorMessage });
            return Ok(result);
        }
    }
}
