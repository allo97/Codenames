using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using TajniacyAPI.JWTAuthentication.Entities;
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

        /// <summary>
        /// Authenticate user
        /// </summary>
        /// <param name="model">Credentials to log in</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(typeof(AuthenticateResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateRequest model)
        {
            try
            {
                var response = await _authService.Authenticate(model, IpAddress());

                SetTokenCookie(response.RefreshToken);

                return Ok(response);
            }
            catch (Exception ex)
            {
                var errorMessage = "There was an error while trying to authenticate";
                return BadRequest(errorMessage + "\n" + ex);
            }
        }

        /// <summary>
        /// Refresh JWT Token and exchange old refresh token from cookies to new
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(typeof(AuthenticateResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RefreshToken()
        {
            try
            {
                var refreshToken = Request.Cookies["refreshToken"];
                var response = await _authService.RefreshToken(refreshToken, IpAddress());

                if (response == null)
                    return Unauthorized(new { message = "Invalid token" });

                SetTokenCookie(response.RefreshToken);

                return Ok(response);
            }
            catch (Exception ex)
            {
                var errorMessage = "There was an error while trying to refresh token";
                return BadRequest(errorMessage + "\n" + ex);
            }
        }

        /// <summary>
        /// Revoke token, it no longer can't be used to authenticate
        /// </summary>
        /// <param name="model">Model containing Token</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RevokeToken([FromBody] RevokeTokenRequest model)
        {
            try
            {
                // accept token from request body or cookie
                var token = model.Token ?? Request.Cookies["refreshToken"];

                if (string.IsNullOrEmpty(token))
                    throw new Exception("Token is required");

                await _authService.RevokeToken(token, IpAddress());

                return Ok("Token revoked");
            }
            catch (Exception ex)
            {
                var errorMessage = "There was an error while trying to revoke Token";
                return BadRequest(errorMessage + "\n" + ex);
            }
        }

        /// <summary>
        /// Get all refresh tokens from user
        /// </summary>
        /// <param name="id">User Id from Mongo</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(IEnumerable<RefreshToken>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetRefreshTokens(string id)
        {
            try
            {
                var user = await _usersService.GetUser(id);
                return Ok(user.RefreshTokens);
            }
            catch (Exception ex)
            {
                var errorMessage = "There was an error while trying get refresh tokens for user with given id";
                return BadRequest(errorMessage + "\n" + ex);
            }
        }

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
