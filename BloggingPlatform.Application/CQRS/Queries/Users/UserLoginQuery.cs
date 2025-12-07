using BloggingPlatform.Application.Helpers.Response;
using BloggingPlatform.Domain.Common.Authentication;
using MediatR;

namespace BloggingPlatform.Application.CQRS.Queries.Users
{
    public class UserLoginQuery : IRequest<ResponseModel<AuthModel>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
