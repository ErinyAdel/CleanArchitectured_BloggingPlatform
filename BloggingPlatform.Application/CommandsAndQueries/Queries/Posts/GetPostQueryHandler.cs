using AutoMapper;
using BloggingPlatform.Application.Interfaces;
using BloggingPlatform.Application.Repositories.Posts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloggingPlatform.Application.CommandsAndQueries.Queries.Posts
{
    public class GetPostQueryHandler : IRequestHandler<GetPostQuery, GetPostQuery>
    {
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;

        public GetPostQueryHandler(IPostRepository postRepository, IMapper mapper)
        {
            _postRepository = postRepository;
            _mapper = mapper;
        }

        public async Task<GetPostQuery> Handle(GetPostQuery request, CancellationToken cancellationToken)
        {
            var post = await _postRepository.GetByIdAsync(request.PostId);
            return post;
        }
    }
}
