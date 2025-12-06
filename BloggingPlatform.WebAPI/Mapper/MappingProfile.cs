using AutoMapper;
using BloggingPlatform.Application.CQRS.Commands.Posts;
using BloggingPlatform.Application.CQRS.Commands.Users;
using BloggingPlatform.Application.CQRS.Queries.Posts;
using BloggingPlatform.Domain.Entities;
using BloggingPlatform.DTO.DTO.Post;
using BloggingPlatform.DTO.DTO.User;

namespace BloggingPlatform.WebAPI.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region User
            CreateMap<ApplicationUser, UserLoginCommand>().ReverseMap();
            CreateMap<RegisterUserCommand, ApplicationUser>().ReverseMap();
            CreateMap<RegisterUserCommand, RegisterDTO>().ReverseMap();
            CreateMap<UserLoginCommand, LoginDTO>().ReverseMap();
            #endregion

            #region Post
            CreateMap<Post, GetPostQuery>()
                    .ForMember(dest => dest.PostId, opt => opt.MapFrom(src => src.Id)).ReverseMap();
            CreateMap<CreatePostCommand, PostDTO>().ReverseMap();
            #endregion
        }
    }
}
