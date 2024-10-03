using AutoMapper;
using BloggingPlatform.Application.Commands.Posts;
using BloggingPlatform.Application.Commands.Users;
using BloggingPlatform.Application.DTOs.PostsDTOs;
using BloggingPlatform.Application.DTOs.UserDTOs;
using BloggingPlatform.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloggingPlatform.Application.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region User
            CreateMap<ApplicationUser, RegisterDTO>().ReverseMap();
            CreateMap<ApplicationUser, UserLoginCommand>().ReverseMap();
            CreateMap<RegisterUserCommand, ApplicationUser>().ReverseMap();
            CreateMap<RegisterUserCommand, RegisterDTO>();
            CreateMap<UserLoginCommand, LoginDTO>().ReverseMap();
            #endregion

            #region Post
            CreateMap<Post, PostDTO>();
            CreateMap<CreatePostCommand, PostDTO>().ReverseMap();
            #endregion
        }
    }
}
