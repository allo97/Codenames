﻿using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Threading;
using System.Threading.Tasks;
using TajniacyAPI.DataAccess.Interfaces;
using TajniacyAPI.DataAccess.Model;
using TajniacyAPI.DataAccess.Repository.Implementations;
using TajniacyAPI.DataAccess.Repository.Interfaces;

namespace TajniacyAPI.DataAccess.Implementations
{
    public class TajniacyUnitOfWork : ITajniacyUnitOfWork, IDisposable
    {
        private readonly IMongoDatabase _mongoDB;
        private readonly IMongoClient _mongoClient;

        public TajniacyUnitOfWork(IOptionsMonitor<MongoSettings> mongoSettings)
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
