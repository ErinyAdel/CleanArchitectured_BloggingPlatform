using BloggingPlatform.Application.DTOs.AuthenticationDTOs;
using BloggingPlatform.Application.Helpers.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloggingPlatform.Application.Commands.Users
{
    public class RegisterUserCommand : IRequest<ResponseModel<AuthModel>>
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
