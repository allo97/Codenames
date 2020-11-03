using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TajniacyAPI.JWTAuthentication.Entities;
using TajniacyAPI.JWTAuthentication.Models;

namespace TajniacyAPI.JWTAuthentication.Services.Interfaces
{
    public interface IUserService
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model, string ipAddress);
        AuthenticateResponse RefreshToken(string token, string ipAddress);
        bool RevokeToken(string token, string ipAddress);
        IEnumerable<User> GetAllUsers();
        User GetById(int id);
    }
}
