using System.Threading.Tasks;
using TajniacyAPI.MongoAPI.Interfaces;
using TajniacyAPI.UserManagement.DataAccess.Model;

namespace TajniacyAPI.UserManagement.DataAccess.Repository.Interfaces
{
    public interface IUsersRepo : IMongoRepository<User>
    {
        Task<User> GetByUsername(string username);
    }
}
