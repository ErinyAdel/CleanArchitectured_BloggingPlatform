using BloggingPlatform.Application.Helpers.Response;
using BloggingPlatform.Domain.Common.Authentication;
using MediatR;

namespace BloggingPlatform.Application.CQRS.Commands.Users
{
    public class RegisterUserCommand : IRequest<ResponseModel<AuthModel>>
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
