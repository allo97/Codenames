using System.ComponentModel.DataAnnotations;

namespace TajniacyAPI.JWTAuthentication.Models
{
    public class AuthenticateRequest
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
