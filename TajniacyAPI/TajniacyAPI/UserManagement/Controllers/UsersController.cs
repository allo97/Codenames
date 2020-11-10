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
    //[Authorize]
    [ApiController]
    [Route("api/tajniacy/UserManagement/[controller]/[action]")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;
        private readonly IMapper _mapper;

        public UsersController(IUsersService usersService, IMapper mapper)
        {
            _usersService = usersService;
            _mapper = mapper;
        }

        /// <summary>
        /// List of users
        /// </summary>
        //[Authorize(Roles = Role.Admin)]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                return Ok(await _usersService.GetAllUsers());
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
                //var currentUserId = User.Identity.Name;
                //if (id != currentUserId && !User.IsInRole(Role.Admin))
                //    return Forbid();

                return Ok(await _usersService.GetUser(id));
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
        [HttpPost]
        [ProducesResponseType(typeof(UserDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddUser([FromBody] UserDto userDto)
        {
            try
            {
                return Ok(await _usersService.AddUser(_mapper.Map<User>(userDto)));
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
                return Ok(await _usersService.UpdateUser(_mapper.Map<User>(userDto)));
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
                await _usersService.DeleteUser(id);
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
