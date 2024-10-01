using BloggingPlatform.Application.Constants;
using BloggingPlatform.Application.DTOs.UserDTOs;
using BloggingPlatform.Application.Interfaces;
using BloggingPlatform.Application.Validators.UsersValidators;
using BloggingPlatform.Domain.Entities;
using BloggingPlatform.Persistence.Services;
using FluentValidation;
using FluentValidation.Results;
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
        private readonly IUserService _userService;
        private readonly RegisterUserValidator _registerValidator;
        private readonly UserLoginValidator _loginValidator;

        public UserController(IUserService userService, RegisterUserValidator registerValidator, UserLoginValidator loginValidator)
        {
            _userService = userService;
            _registerValidator = _registerValidator;
            _loginValidator = loginValidator;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO model)
        {
            ValidationResult resultVal = await _registerValidator.ValidateAsync(model);

            if (!resultVal.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.RegisterUserAsync(model);
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

            var result = await _userService.UserLoginAsync(model);
            if (!result.IsSuccess)
                return StatusCode(result?.ErrorCode != null && result.ErrorCode != 0 ? (int)result.ErrorCode : (int)ResponseStatusCode.NoContent, new { Message = result.ErrorMessage });
            return Ok(result);
        }
    }
}
