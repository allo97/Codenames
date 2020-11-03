using MongoDB.Driver;
using TajniacyAPI.MongoAPI.Implementations;
using TajniacyAPI.UserManagement.DataAccess.Model;
using TajniacyAPI.UserManagement.DataAccess.Repository.Interfaces;

namespace TajniacyAPI.UserManagement.DataAccess.Repository.Implementations
{
    public class UsersRepo : MongoRepository<User>, IUsersRepo
    {
        public UsersRepo(IMongoCollection<User> collection) : base(collection) { }
    }
}
