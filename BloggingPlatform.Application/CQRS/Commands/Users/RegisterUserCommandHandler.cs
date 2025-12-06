using AutoMapper;
using BloggingPlatform.Application.Constants;
using BloggingPlatform.Application.Helpers.Response;
using BloggingPlatform.Domain.Common.Authentication;
using BloggingPlatform.Domain.Entities;
using BloggingPlatform.DTO.Validator.User;
using BloggingPlatform.ServiceInterface.Interface;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BloggingPlatform.Application.CQRS.Commands.Users
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, ResponseModel<AuthModel>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly JWT _jwt;
        private readonly IMapper _mapper;
        private readonly ILogger<RegisterUserCommandHandler> _logger;
        private readonly ITokenService _tokenService;

        public RegisterUserCommandHandler(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IConfiguration configuration,
            IOptions<JWT> jwt, IMapper mapper, ILogger<RegisterUserCommandHandler> logger, ITokenService tokenService)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _configuration = configuration;
            _jwt = jwt.Value;
            _mapper = mapper;
            _logger = logger;
            _tokenService = tokenService;
        }

        public async Task<ResponseModel<AuthModel>> Handle(RegisterUserCommand model, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"Start RegisterUserAsync Service ==> RegisterDTO model: {model}");

                await _unitOfWork.BeginTransactionAsync();

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
