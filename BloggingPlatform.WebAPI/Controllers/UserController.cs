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
        private UserValidator _validator;

        public UserController(IUserService userService, UserValidator validator)
        {
            _userService = userService;
            _validator = validator;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO model)
        {
            ValidationResult resultVal = await _validator.ValidateAsync(model);

            if (!resultVal.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.RegisterUserAsync(model);
            if (result == null)
                return StatusCode((int)ResponseStatusCode.NoContent, new { Message = "Error In Registration" });
            else
                return Ok(result);
        }
    }
}
