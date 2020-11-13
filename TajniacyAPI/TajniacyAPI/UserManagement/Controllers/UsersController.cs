using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using TajniacyAPI.UserManagement.DataAccess.Model;
using TajniacyAPI.UserManagement.DataAccess.Model.Dto;
using TajniacyAPI.UserManagement.Services.Interfaces;

namespace TajniacyAPI.UserManagement.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/tajniacy/UserManagement/[controller]/[action]")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _userService;
        private readonly IMapper _mapper;

        public UsersController(IUsersService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        /// <summary>
        /// List of users
        /// </summary>
        [Authorize(Roles = Role.Admin)]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                return Ok(_mapper.Map<List<UserDto>>(await _userService.GetAllUsers()));
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
        [ProducesResponseType(typeof(UserDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetUser(string id)
        {
            try
            {
                var currentUserId = User.Identity.Name;
                if (id != currentUserId && !User.IsInRole(Role.Admin))
                    return Unauthorized(new { message = "Unauthorized"});

                return Ok(_mapper.Map<UserDto>(await _userService.GetUser(id)));
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
        /// <param name="userDto">New user added to MongoDB</param>
        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(typeof(UserDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddUser([FromBody] UserDto userDto)
        {
            try
            {
                var createdUser = await _userService.AddUser(_mapper.Map<User>(userDto));
                return Ok(_mapper.Map<UserDto>(createdUser));
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
        [ProducesResponseType(typeof(UserDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateUser([FromBody] UserDto userDto)
        {
            try
            {
                var currentUserId = User.Identity.Name;
                if (userDto.ID != currentUserId && !User.IsInRole(Role.Admin))
                    return Unauthorized(new { message = "Unauthorized" });

                var updatedUser = await _userService.UpdateUser(_mapper.Map<User>(userDto));
                return Ok(_mapper.Map<UserDto>(updatedUser));
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
                var currentUserId = User.Identity.Name;
                if (id != currentUserId && !User.IsInRole(Role.Admin))
                    return Unauthorized(new { message = "Unauthorized" });

                await _userService.DeleteUser(id);
                return Ok("User has been deleted");
            }
            catch (Exception ex)
            {
                var errorMessage = "There was an error while trying to delete User from MongoDB";
                return BadRequest(errorMessage + "\n" + ex);
            }
        }
    }
}
