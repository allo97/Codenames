using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TajniacyAPI.DataAccess.Interfaces
{
    public interface IMongoDBUnitOfWork
    {
        Task<IClientSessionHandle> StartSessionAsync(ClientSessionOptions options = null, CancellationToken cancellationToken = default);
    }
}
