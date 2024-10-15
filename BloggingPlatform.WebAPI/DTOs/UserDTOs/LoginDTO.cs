using BloggingPlatform.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloggingPlatform.Application.DTOs.UserDTOs
{
    public class LoginDTO : IUserCredentials
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
