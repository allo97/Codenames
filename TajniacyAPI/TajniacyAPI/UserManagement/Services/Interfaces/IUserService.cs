using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using TajniacyAPI.UserManagement.DataAccess.Model;

namespace TajniacyAPI.UserManagement.Services.Interfaces
{
    public interface IUserService
    {
        Task<List<User>> GetAllUsers();
        Task<User> GetUser(string id);
        Task<User> AddUser(User user);
        Task<User> UpdateUser(User user);
        Task DeleteUser(string id);
    }
}
