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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace BloggingPlatform.Application.Commands.Users
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, ResponseModel<AuthModel>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly JWT _jwt;
        private readonly IMapper _mapper;
        private readonly IValidator<RegisterDTO> _registerValidator;
        private readonly ILogger<RegisterUserCommandHandler> _logger;

        public RegisterUserCommandHandler(UserManager<ApplicationUser> userManager, IConfiguration configuration, 
            IOptions<JWT> jwt, IMapper mapper, IValidator<RegisterDTO> registerValidator, ILogger<RegisterUserCommandHandler> logger)
        {
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

                // Validate RegisterDTO
                var registerDto = _mapper.Map<RegisterDTO>(model);
                var validationResult = await _registerValidator.ValidateAsync(registerDto);

                if (!validationResult.IsValid)
                {
                    var errorMessages = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    _logger.LogError($"Validation failed: {errorMessages}");
                    return DynamicResponse<AuthModel>.Failed(null, errorCode: (int)ResponseStatusCode.BadRequest, errorMessage: errorMessages);
                }

                var user = _mapper.Map<ApplicationUser>(model);

                var userWithSameEmail = await _userManager.FindByEmailAsync(model.Email);

                if (userWithSameEmail?.IsBlocked == true)
                {
                    _logger.LogInformation($"In RegisterUserAsync Service ==> User: {model.Email} IsBlocked = true");
                    BlockUser(userWithSameEmail);
                }

                if (userWithSameEmail != null)
                {
                    _logger.LogInformation($"In RegisterUserAsync Service ==> User: {model.Email} Is Already Registered.");
                    return DynamicResponse<AuthModel>.Failed(null, errorCode: (int)ResponseStatusCode.Conflict, errorMessage: $"Email {user.Email} is already registered.");
                }

                var result = await _userManager.CreateAsync(user, model.Password);
                if (!result.Succeeded)
                {
                    var errors = string.Empty;
                    foreach (var error in result.Errors)
                        errors += $"{error.Description}, ";
                    return DynamicResponse<AuthModel>.Failed(null, errorCode: (int)ResponseStatusCode.InternalServerError, errorMessage: $"result.Succeeded =false errors Result ==> {errors}");
                }

                var userToken = await CreateJwtToken(user);
                await _userManager.AddToRoleAsync(user, "User");

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
                _logger.LogError(ex.Message);
                return null;
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
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
            issuer: _jwt.Issuer,
            audience: _jwt.Audience,
            claims: claims,
                expires: DateTime.Now.AddDays(_jwt.DurationInDays),
                signingCredentials: signingCredentials);

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
