using BloggingPlatform.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;

namespace BloggingPlatform.ServiceInterface.Interface
{
    public interface ITokenService
    {
        Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user);
    }
}
