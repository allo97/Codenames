using MongoDB.Driver;
using System.Threading;
using System.Threading.Tasks;

namespace TajniacyAPI.MongoAPI.Interfaces
{
    public interface IMongoDBUnitOfWork
    {
        Task<IClientSessionHandle> StartSessionAsync(ClientSessionOptions options = null, CancellationToken cancellationToken = default);
    }
}
