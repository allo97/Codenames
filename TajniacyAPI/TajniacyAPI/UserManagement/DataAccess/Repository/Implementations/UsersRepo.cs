using MongoDB.Driver;
using System.Threading.Tasks;
using TajniacyAPI.MongoAPI.Implementations;
using TajniacyAPI.UserManagement.DataAccess.Model;
using TajniacyAPI.UserManagement.DataAccess.Repository.Interfaces;

namespace TajniacyAPI.UserManagement.DataAccess.Repository.Implementations
{
    public class UsersRepo : MongoRepository<User>, IUsersRepo
    {
        public UsersRepo(IMongoCollection<User> collection) : base(collection) { }

        public Task<User> GetByUsername(string username)
        {
            return _collection.Find(n => n.Username.ToLower() == username.ToLower()).FirstOrDefaultAsync();
        }
    }
}
