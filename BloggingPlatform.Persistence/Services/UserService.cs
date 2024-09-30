using AutoMapper;
using Azure;
using Azure.Core;
using BloggingPlatform.Application.DTOs.AuthenticationDTOs;
using BloggingPlatform.Application.DTOs.UserDTOs;
using BloggingPlatform.Application.Interfaces;
using BloggingPlatform.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NuGet.Protocol;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BloggingPlatform.Application.Helpers.Response;
using BloggingPlatform.Application.Constants;
using Microsoft.Extensions.Logging;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using BloggingPlatform.Application.Commands.Users;

namespace BloggingPlatform.Persistence.Services
{
    public class UserService : IUserService
    {
        private readonly IMediator _mediator;

        public UserService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<ResponseModel<AuthModel>> RegisterUserAsync(RegisterDTO model)
        {
            var command = new RegisterUserCommand
            {
                UserName = model.UserName,
                Email = model.Email,
                Password = model.Password
            };
            return await _mediator.Send(command);
        }
    }
}
