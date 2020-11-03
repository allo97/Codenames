using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using TajniacyAPI.UserManagement.DataAccess.Model;
using TajniacyAPI.UserManagement.Services.Interfaces;

namespace TajniacyAPI.UserManagement.Controllers
{
    [ApiController]
    [Route("api/tajniacy/UserManagement/[controller]/[action]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// List of users
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<User>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                return Ok(await _userService.GetAllUsers());
            }
            catch (Exception ex)
            {
                var errorMessage = "There was an error while trying to list Users from MongoDB";
                return BadRequest(errorMessage + "\n" + ex);
            }
        }

        /// <summary>
        /// Get user
        /// </summary>
        /// <param name="id">Mongo ID</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetUser(string id)
        {
            try
            {
                return Ok(await _userService.GetUser(id));
            }
            catch (Exception ex)
            {
                var errorMessage = "There was an error while trying to get User from MongoDB";
                return BadRequest(errorMessage + "\n" + ex);
            }
        }

        /// <summary>
        /// Add User to MongoDB
        /// </summary>
        /// <param name="user">New user added to MongoDB</param>
        [HttpPost]
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddUser([FromBody] User user)
        {
            try
            {
                return Ok(await _userService.AddUser(user));
            }
            catch (Exception ex)
            {
                var errorMessage = "There was an error while trying to add User to MongoDB";
                return BadRequest(errorMessage + "\n" + ex);
            }
        }

        /// <summary>
        /// Update User in MongoDB
        /// </summary>
        /// <param name="user">User to update in MongoDB</param>
        [HttpPut]
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateUser([FromBody] User user)
        {
            try
            {
                return Ok(await _userService.UpdateUser(user));
            }
            catch (Exception ex)
            {
                var errorMessage = "There was an error while trying to update User in MongoDB";
                return BadRequest(errorMessage + "\n" + ex);
            }
        }

        /// <summary>
        /// Delete User in MongoDB
        /// </summary>
        /// <param name="id">ID of User to delete</param>
        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteUser(string id)
        {
            try
            {
                await _userService.DeleteUser(id);
                return Ok();
            }
            catch (Exception ex)
            {
                var errorMessage = "There was an error while trying to delete User from MongoDB";
                return BadRequest(errorMessage + "\n" + ex);
            }
        }
    }
}
