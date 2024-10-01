using BloggingPlatform.Application.Commands.Users;
using BloggingPlatform.Application.DTOs.AuthenticationDTOs;
using BloggingPlatform.Application.DTOs.UserDTOs;
using BloggingPlatform.Application.Helpers.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloggingPlatform.Application.Interfaces
{
    public interface IUserService
    {
        Task<ResponseModel<AuthModel>> RegisterUserAsync(RegisterDTO model);
        Task<ResponseModel<AuthModel>> UserLoginAsync(LoginDTO model);
    }
}
