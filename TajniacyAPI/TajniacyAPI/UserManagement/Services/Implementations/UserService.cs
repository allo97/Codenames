using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using TajniacyAPI.UserManagement.DataAccess.Model;
using TajniacyAPI.UserManagement.Services.Interfaces;

namespace TajniacyAPI.UserManagement.Services.Implementations
{
    public class UserService : IUserService
    {
        public Task<User> AddUser(User user)
        {
            throw new System.NotImplementedException();
        }

        public Task DeleteUser(string id)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<User>> GetAllUsers()
        {
            throw new System.NotImplementedException();
        }

        public Task<User> GetUser(string id)
        {
            throw new System.NotImplementedException();
        }

        public Task<User> UpdateUser(User user)
        {
            throw new System.NotImplementedException();
        }
    }
}
