using AutoMapper;
using BloggingPlatform.Application.Constants;
using BloggingPlatform.Application.DTOs.AuthenticationDTOs;
using BloggingPlatform.Application.DTOs.UserDTOs;
using BloggingPlatform.Application.Helpers.Response;
using BloggingPlatform.Application.Interfaces;
using BloggingPlatform.Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace BloggingPlatform.Application.Commands.Users
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, ResponseModel<AuthModel>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly JWT _jwt;
        private readonly IMapper _mapper;
        private readonly IValidator<RegisterDTO> _registerValidator;
        private readonly ILogger<RegisterUserCommandHandler> _logger;

        public RegisterUserCommandHandler(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IConfiguration configuration, 
            IOptions<JWT> jwt, IMapper mapper, IValidator<RegisterDTO> registerValidator, ILogger<RegisterUserCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _configuration = configuration;
            _jwt = jwt.Value;
            _mapper = mapper;
            _registerValidator = registerValidator;
            _logger = logger;
        }

        public async Task<ResponseModel<AuthModel>> Handle(RegisterUserCommand model, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"Start RegisterUserAsync Service ==> RegisterDTO model: {model}");

                await _unitOfWork.BeginTransactionAsync();

                var registerDto = _mapper.Map<RegisterDTO>(model);
                var validationResult = await _registerValidator.ValidateAsync(registerDto);

                if (!validationResult.IsValid)
                {
                    var errorMessages = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    _logger.LogError($"Validation Failed: {errorMessages}");

                    return DynamicResponse<AuthModel>.Failed(null, errorCode: (int)ResponseStatusCode.BadRequest, errorMessage: errorMessages);
                }

                var user = _mapper.Map<ApplicationUser>(model);

                var userWithSameEmail = await _userManager.FindByEmailAsync(model.Email);
                if (userWithSameEmail?.IsBlocked == true)
                {
                    return DynamicResponse<AuthModel>.Failed(null, errorCode: (int)ResponseStatusCode.Forbidden, errorMessage: "This user is blocked.");
                }

                if (userWithSameEmail != null)
                {
                    return DynamicResponse<AuthModel>.Failed(null, errorCode: (int)ResponseStatusCode.Conflict, errorMessage: $"Email {model.Email} is already registered.");
                }

                var result = await _userManager.CreateAsync(user, model.Password);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return DynamicResponse<AuthModel>.Failed(null, errorCode: (int)ResponseStatusCode.InternalServerError, errorMessage: errors);
                }

                await _userManager.AddToRoleAsync(user, Roles.User.ToString());

                await _userManager.AddClaimAsync(user, new Claim(JwtRegisteredClaimNames.Email, user.Email));


                JwtSecurityToken userToken;
                try
                {
                    userToken = await CreateJwtToken(user);
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
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    IsAuthenticated = true,
                    Token = new JwtSecurityTokenHandler().WriteToken(userToken),
                    ExpiresOn = userToken.ValidTo,
                    IsActive = true,
                    Roles = Roles.User.ToString(),
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred during registration: {ex.Message}");
                await _unitOfWork.RollbackTransactionAsync();
                return DynamicResponse<AuthModel>.Failed(null, errorCode: (int)ResponseStatusCode.InternalServerError, errorMessage: "An unexpected error occurred.");
            }
        }


        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
                new Claim("userId", user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("userName", user.UserName),
            }
            .Union(userClaims)
            .Union(roleClaims);

            var jwtKey = _jwt.Key.ToString();
            var key = Convert.FromBase64String(jwtKey);
            var symmetricSecurityKey = new SymmetricSecurityKey(key);
            //var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken (
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(_jwt.DurationInDays),
                signingCredentials: signingCredentials
            );

            return jwtSecurityToken;
        }

        private void BlockUser(ApplicationUser user)
        {
            user.IsBlocked = true;
            user.Email = user.Email + "_Blocked_" + user.Id;
            user.UserName = user.UserName + "_Blocked_" + user.Id;
            user.NormalizedEmail = user.NormalizedEmail + "_Blocked_" + user.Id;
            user.NormalizedUserName = user.NormalizedUserName + "_Blocked_" + user.Id;
            //_unitOfWork.Complete();
        }
    }
}
