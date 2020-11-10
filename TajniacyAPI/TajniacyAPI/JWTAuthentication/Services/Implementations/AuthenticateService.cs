using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using BC = BCrypt.Net.BCrypt;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TajniacyAPI.JWTAuthentication.Entities;
using TajniacyAPI.JWTAuthentication.Helpers;
using TajniacyAPI.JWTAuthentication.Models;
using TajniacyAPI.JWTAuthentication.Services.Interfaces;
using TajniacyAPI.UserManagement.DataAccess.Model;
using TajniacyAPI.UserManagement.Services.Interfaces;

namespace TajniacyAPI.JWTAuthentication.Services.Implementations
{
    public class AuthenticateService : IAuthenticateService
    {
        private readonly AppSettings _appSettings;
        private readonly IUsersService _usersService;

        public AuthenticateService(
            IOptions<AppSettings> appSettings,
            IUsersService userService)
        {
            _appSettings = appSettings.Value;
            _usersService = userService;
        }

        public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest model, string ipAddress)
        {
            var user = (await _usersService.GetAllUsers()).FirstOrDefault(user => user.Username == model.Username && BC.Verify(model.Password, user.Password));

            // return null if user not found
            if (user == null) return null;

            // authentication successful so generate jwt and refresh tokens
            var jwtToken = generateJwtToken(user);
            var refreshToken = generateRefreshToken(ipAddress);

            // save refresh token
            user.RefreshTokens.Add(refreshToken);

            // remove old refresh tokens from account
            removeOldRefreshTokens(user);

            //save changes to db
            await _usersService.UpdateUser(user);

            return new AuthenticateResponse(user, jwtToken, refreshToken.Token);
        }

        public async Task<AuthenticateResponse> RefreshToken(string token, string ipAddress)
        {
            var (refreshToken, user) = await getRefreshToken(token);

            // replace old refresh token with a new one and save
            var newRefreshToken = generateRefreshToken(ipAddress);
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
            refreshToken.ReplacedByToken = newRefreshToken.Token;
            user.RefreshTokens.Add(newRefreshToken);
            await _usersService.UpdateUser(user);

            // generate new jwt
            var jwtToken = generateJwtToken(user);

            return new AuthenticateResponse(user, jwtToken, newRefreshToken.Token);
        }

        public async Task RevokeToken(string token, string ipAddress)
        {
            var (refreshToken, user) = await getRefreshToken(token);

            // revoke token and save
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
            user.RefreshTokens.Add(refreshToken);
            await _usersService.UpdateUser(user);
        }

        // helper methods
        private string generateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.ID),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return "Bearer " + tokenHandler.WriteToken(token);
        }

        private RefreshToken generateRefreshToken(string ipAddress)
        {
            using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[64];
                rngCryptoServiceProvider.GetBytes(randomBytes);
                return new RefreshToken
                {
                    Token = Convert.ToBase64String(randomBytes),
                    Expires = DateTime.UtcNow.AddDays(7),
                    Created = DateTime.UtcNow,
                    CreatedByIp = ipAddress
                };
            }
        }

        private void removeOldRefreshTokens(User user)
        {
            user.RefreshTokens.RemoveAll(x =>
                !x.IsActive);
        }

        private async Task<(RefreshToken, User)> getRefreshToken(string token)
        {
            var user = (await _usersService.GetAllUsers()).FirstOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));
            if (user == null) throw new Exception("Invalid token");
            var refreshToken = user.RefreshTokens.FirstOrDefault(x => x.Token == token);
            if (!refreshToken.IsActive) throw new Exception("Invalid token");
            return (refreshToken, user);
        }
    }
}
