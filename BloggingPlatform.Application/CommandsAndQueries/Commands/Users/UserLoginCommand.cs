using BloggingPlatform.Application.Helpers.Response;
using BloggingPlatform.Application.Models;
using BloggingPlatform.Application.Models.Authentication;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BloggingPlatform.Application.CommandsAndQueries.Commands.Users
{
    public class UserLoginCommand : IRequest<ResponseModel<AuthModel>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
