using BloggingPlatform.Application.Commands.Users;
using BloggingPlatform.Application.Constants;
using BloggingPlatform.Application.DTOs.UserDTOs;
using BloggingPlatform.Application.Interfaces;
using BloggingPlatform.Application.Validators.UsersValidators;
using BloggingPlatform.Domain.Entities;
using BloggingPlatform.Persistence.Services;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System;

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
            _registerValidator = _registerValidator;
            _loginValidator = loginValidator;
            _mediator = mediator;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO model)
        {
            ValidationResult resultVal = await _registerValidator.ValidateAsync(model);

            if (!resultVal.IsValid)
                return BadRequest(ModelState);

            var command = new RegisterUserCommand
            {
                UserName = model.UserName,
                Email = model.Email,
                Password = model.Password
            };
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

            var command = new UserLoginCommand
            {
                Email = model.Email,
                Password = model.Password
            };
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return StatusCode(result?.ErrorCode != null && result.ErrorCode != 0 ? (int)result.ErrorCode : (int)ResponseStatusCode.NoContent, new { Message = result.ErrorMessage });
            return Ok(result);
        }
    }
}
