using System.Text.Json.Serialization;
using TajniacyAPI.UserManagement.DataAccess.Model;

namespace TajniacyAPI.JWTAuthentication.Models
{
    public class AuthenticateResponse
    {
        public string ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public string JwtToken { get; set; }

        [JsonIgnore] // refresh token is returned in http only cookie
        public string RefreshToken { get; set; }

        public AuthenticateResponse(User user, string jwtToken, string refreshToken)
        {
            ID = user.ID;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Username = user.Username;
            Role = user.Role;
            JwtToken = jwtToken;
            RefreshToken = refreshToken;
        }
    }
}
