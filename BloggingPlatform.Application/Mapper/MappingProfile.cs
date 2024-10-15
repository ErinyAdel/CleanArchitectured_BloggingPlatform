using AutoMapper;
using BloggingPlatform.Application.CommandsAndQueries.Commands.Posts;
using BloggingPlatform.Application.CommandsAndQueries.Commands.Users;
using BloggingPlatform.Application.CommandsAndQueries.Queries.Posts;
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
            CreateMap<ApplicationUser, UserLoginCommand>().ReverseMap();
            CreateMap<RegisterUserCommand, ApplicationUser>().ReverseMap();
            #endregion

            #region Post
            CreateMap<Post, GetPostQuery>()
                .ForMember(dest => dest.PostId, opt => opt.MapFrom(src => src.Id)).ReverseMap();
            #endregion
        }
    }
}
