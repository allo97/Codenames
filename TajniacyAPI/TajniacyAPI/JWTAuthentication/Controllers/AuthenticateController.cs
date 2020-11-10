using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TajniacyAPI.JWTAuthentication.Models;
using TajniacyAPI.JWTAuthentication.Services.Interfaces;
using TajniacyAPI.UserManagement.Services.Interfaces;

namespace TajniacyAPI.JWTAuthentication.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/tajniacy/JWTAuthentication/[controller]/[action]")]
    public class AuthenticateController : ControllerBase
    {
        private IAuthenticateService _authService;
        private IUsersService _usersService;

        public AuthenticateController(IAuthenticateService authService, IUsersService usersService)
        {
            _authService = authService;
            _usersService = usersService;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateRequest model)
        {
            var response = await _authService.Authenticate(model, IpAddress());

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            SetTokenCookie(response.RefreshToken);

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var response = await _authService.RefreshToken(refreshToken, IpAddress());

            if (response == null)
                return Unauthorized(new { message = "Invalid token" });

            SetTokenCookie(response.RefreshToken);

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> RevokeToken([FromBody] RevokeTokenRequest model)
        {
            // accept token from request body or cookie
            var token = model.Token ?? Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(token))
                return BadRequest(new { message = "Token is required" });

            await _authService.RevokeToken(token, IpAddress());

            return Ok(new { message = "Token revoked" });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRefreshTokens(string id)
        {
            var user = await _usersService.GetUser(id);
            if (user == null) return NotFound();

            return Ok(user.RefreshTokens);
        }

        // helper methods

        private void SetTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }

        private string IpAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
    }
}
