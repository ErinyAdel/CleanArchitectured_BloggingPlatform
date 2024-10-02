using BloggingPlatform.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloggingPlatform.Application.Interfaces
{
    public interface ITokenService
    {
        Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user);
    }
}
