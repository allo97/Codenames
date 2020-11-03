using TajniacyAPI.MongoAPI.Interfaces;
using TajniacyAPI.UserManagement.DataAccess.Repository.Interfaces;

namespace TajniacyAPI.UserManagement.DataAccess.Interfaces
{
    public interface IUsersUnitOfWork : IMongoDBUnitOfWork
    {
        IUsersRepo UsersRepo { get; }
    }
}
