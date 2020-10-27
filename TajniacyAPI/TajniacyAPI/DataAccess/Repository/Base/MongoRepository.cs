using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using TajniacyAPI.DataAccess.Model.Interfaces;
using TajniacyAPI.DataAccess.Repository.Interfaces;

namespace TajniacyAPI.DataAccess.Repository.Base
{
    public abstract class MongoRepository<T> : IMongoRepository<T> where T : IEntity
    {
        protected readonly IMongoCollection<T> _collection;

        public MongoRepository(IMongoCollection<T> collection)
        {
            _collection = collection;
        }

        public virtual Task<List<T>> GetAll() => _collection.Find(Builders<T>.Filter.Empty).ToListAsync();

        public Task<T> GetById(string id, IClientSessionHandle session = null) => session == null
            ? _collection.Find(n => n.ID == id).FirstOrDefaultAsync()
            : _collection.Find(session, n => n.ID == id).FirstOrDefaultAsync();
    }
}
