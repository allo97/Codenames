using System.Collections.Generic;
using System.Threading.Tasks;
using TajniacyAPI.JWTAuthentication.Models;

namespace TajniacyAPI.JWTAuthentication.Services.Interfaces
{
    public interface IAuthenticateService
    {
        Task<AuthenticateResponse> Authenticate(AuthenticateRequest model, string ipAddress);
        Task<AuthenticateResponse> RefreshToken(string token, string ipAddress);
        Task RevokeToken(string token, string ipAddress);
    }
}
