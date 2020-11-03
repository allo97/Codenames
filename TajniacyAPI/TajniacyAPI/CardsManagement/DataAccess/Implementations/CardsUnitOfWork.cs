using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Threading;
using System.Threading.Tasks;
using TajniacyAPI.CardsManagement.DataAccess.Interfaces;
using TajniacyAPI.CardsManagement.DataAccess.Model;
using TajniacyAPI.CardsManagement.DataAccess.Repository.Implementations;
using TajniacyAPI.CardsManagement.DataAccess.Repository.Interfaces;
using TajniacyAPI.MongoAPI.Implementations;

namespace TajniacyAPI.CardsManagement.DataAccess.Implementations
{
    public class CardsUnitOfWork : ICardsUnitOfWork, IDisposable
    {
        private readonly IMongoDatabase _mongoDB;
        private readonly IMongoClient _mongoClient;

        public CardsUnitOfWork(IOptionsMonitor<MongoSettings> mongoSettings)
        {
            _mongoClient = new MongoClient(mongoSettings.CurrentValue.ConnectionString);
            _mongoDB = _mongoClient.GetDatabase(mongoSettings.CurrentValue.MongoDBName);
        }

        public Task<IClientSessionHandle> StartSessionAsync(ClientSessionOptions options = null, CancellationToken cancellationToken = default)
        {
            return _mongoClient.StartSessionAsync(options, cancellationToken);
        }

        private IWordCardsRepo _wordCardsRepo;
        public IWordCardsRepo WordCardsRepo => _wordCardsRepo ?? (_wordCardsRepo = new WordCardsRepo(_mongoDB.GetCollection<WordCard>("Cards")));


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
