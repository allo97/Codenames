using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using TajniacyAPI.UserManagement.DataAccess.Model;
using TajniacyAPI.UserManagement.DataAccess.Model.Dto;

namespace TajniacyAPI.UserManagement.Services.Interfaces
{
    public interface IUsersService
    {
        Task<List<User>> GetAllUsers();
        Task<User> GetUser(string id);
        Task<User> AddUser(User user);
        Task<User> UpdateUser(User user);
        Task DeleteUser(string id);
    }
}
