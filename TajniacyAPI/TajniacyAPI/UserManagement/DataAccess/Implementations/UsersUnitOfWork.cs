using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Threading;
using System.Threading.Tasks;
using TajniacyAPI.MongoAPI.Implementations;
using TajniacyAPI.UserManagement.DataAccess.Interfaces;
using TajniacyAPI.UserManagement.DataAccess.Model;
using TajniacyAPI.UserManagement.DataAccess.Repository.Implementations;
using TajniacyAPI.UserManagement.DataAccess.Repository.Interfaces;

namespace TajniacyAPI.UserManagement.DataAccess.Implementations
{
    public class UsersUnitOfWork : IUsersUnitOfWork, IDisposable
    {
        private readonly IMongoDatabase _mongoDB;
        private readonly IMongoClient _mongoClient;

        public UsersUnitOfWork(IOptionsMonitor<MongoSettings> mongoSettings)
        {
            _mongoClient = new MongoClient(mongoSettings.CurrentValue.ConnectionString);
            _mongoDB = _mongoClient.GetDatabase(mongoSettings.CurrentValue.MongoDBName);
        }

        public Task<IClientSessionHandle> StartSessionAsync(ClientSessionOptions options = null, CancellationToken cancellationToken = default)
        {
            return _mongoClient.StartSessionAsync(options, cancellationToken);
        }

        private IUsersRepo _usersRepo;
        public IUsersRepo UsersRepo => _usersRepo ?? (_usersRepo = new UsersRepo(_mongoDB.GetCollection<User>("Users")));

        #region IDisposable

        private bool disposed = false;

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                }
            }

            this.disposed = true;
        }

        #endregion
    }
}
