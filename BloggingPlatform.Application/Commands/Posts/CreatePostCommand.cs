﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BloggingPlatform.Application.Commands.Posts
{
    public class CreatePostCommand : IRequest<int>
    {
        public string Title { get; set; }
        public string Content { get; set; }

        //[JsonIgnore]
        //[IgnoreDataMember]
        public string AuthorId;
    }
}
