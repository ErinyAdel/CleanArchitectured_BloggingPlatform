using AutoMapper;
using BloggingPlatform.Application.Constants;
using BloggingPlatform.Application.DTOs.AuthenticationDTOs;
using BloggingPlatform.Application.DTOs.UserDTOs;
using BloggingPlatform.Application.Helpers;
using BloggingPlatform.Application.Helpers.Response;
using BloggingPlatform.Application.Interfaces;
using BloggingPlatform.Application.Validators.UsersValidators;
using BloggingPlatform.Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BloggingPlatform.Application.CommandsAndQueries.Commands.Users
{
    public class UserLoginCommandHandler : IRequestHandler<UserLoginCommand, ResponseModel<AuthModel>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IUserLoginValidator _validator;
        private readonly ILogger<UserLoginCommandHandler> _logger;
        private readonly ITokenService _tokenService;

        public UserLoginCommandHandler(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IMapper mapper,
            IUserLoginValidator validator, ILogger<UserLoginCommandHandler> logger, ITokenService tokenService)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _mapper = mapper;
            _validator = validator;
            _logger = logger;
            _tokenService = tokenService;
        }

        public async Task<ResponseModel<AuthModel>> Handle(UserLoginCommand model, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"Start UserLoginAsync Service ==> LoginDTO model: {model}");

                await _unitOfWork.BeginTransactionAsync();

                var loginDto = _mapper.Map<LoginDTO>(model);
                var validationResult = await _validator.ValidateAsync(loginDto);

                if (!validationResult.IsValid)
                {
                    var errorMessages = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    _logger.LogError($"Validation Failed: {errorMessages}");
                    return DynamicResponse<AuthModel>.Failed(null, errorCode: (int)ResponseStatusCode.BadRequest, errorMessage: errorMessages);
                }

                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                    return DynamicResponse<AuthModel>.Failed(null, errorCode: (int)ResponseStatusCode.NotFound, errorMessage: "User not found.");

                var passwordValid = await _userManager.CheckPasswordAsync(user, model.Password);
                if (!passwordValid)
                    return DynamicResponse<AuthModel>.Failed(null, errorCode: (int)ResponseStatusCode.Unauthorized, errorMessage: "Invalid password.");

                if (user.IsBlocked)
                    return DynamicResponse<AuthModel>.Failed(null, errorCode: (int)ResponseStatusCode.Forbidden, errorMessage: "This user is blocked.");

                JwtSecurityToken userToken;
                try
                {
                    userToken = await _tokenService.CreateJwtToken(user);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Token Generation Failed: {ex.Message}");
                    await _unitOfWork.RollbackTransactionAsync();
                    return DynamicResponse<AuthModel>.Failed(null, errorCode: (int)ResponseStatusCode.InternalServerError, errorMessage: "Token generation failed.");
                }

                await _unitOfWork.CommitTransactionAsync();

                return DynamicResponse<AuthModel>.Success(new AuthModel
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(userToken),
                    IsAuthenticated = true
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred during login: {ex.Message}");
                await _unitOfWork.RollbackTransactionAsync();
                return DynamicResponse<AuthModel>.Failed(null, errorCode: (int)ResponseStatusCode.InternalServerError, errorMessage: "An unexpected error occurred.");
            }
        }
    }

}
