using System.Text.Json.Serialization;
using System;
using System.ComponentModel.DataAnnotations;

namespace TajniacyAPI.UserManagement.DataAccess.Model.Dto
{
    public class UserDto
    {
        public string ID { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Role { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
    }
}
