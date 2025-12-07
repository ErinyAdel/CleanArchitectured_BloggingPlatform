using AutoMapper;
using BloggingPlatform.Application.Constants;
using BloggingPlatform.Application.Helpers.Response;
using BloggingPlatform.Domain.Common.Authentication;
using BloggingPlatform.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using BloggingPlatform.ServiceInterface.Interface;

namespace BloggingPlatform.Application.CQRS.Queries.Users
{
    public class UserLoginQueryHandler : IRequestHandler<UserLoginQuery, ResponseModel<AuthModel>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ILogger<UserLoginQueryHandler> _logger;
        private readonly ITokenService _tokenService;

        public UserLoginQueryHandler(UserManager<ApplicationUser> userManager, IMapper mapper,
            ILogger<UserLoginQueryHandler> logger, ITokenService tokenService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _logger = logger;
            _tokenService = tokenService;
        }

        public async Task<ResponseModel<AuthModel>> Handle(UserLoginQuery model, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"Start UserLoginAsync Service ==> LoginDTO model: {model}");

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
                    return DynamicResponse<AuthModel>.Failed(null, errorCode: (int)ResponseStatusCode.InternalServerError, errorMessage: "Token generation failed.");
                }

                var roles = await _userManager.GetRolesAsync(user);

                return DynamicResponse<AuthModel>.Success(new AuthModel
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(userToken),
                    IsAuthenticated = true,
                    UserName = user.UserName,
                    Email = user.Email,
                    Roles = string.Join(",", roles)
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred during login: {ex.Message}");
                return DynamicResponse<AuthModel>.Failed(null, errorCode: (int)ResponseStatusCode.InternalServerError, errorMessage: "An unexpected error occurred.");
            }
        }
    }
}
