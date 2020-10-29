using MongoDB.Bson;
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

        public async Task<bool> Update(T document, IClientSessionHandle session = null)
        {
            var result = await (session == null
                ? _collection.ReplaceOneAsync(n => n.ID == document.ID, document)
                : _collection.ReplaceOneAsync(session, n => n.ID == document.ID, document));

            return result.IsAcknowledged;
        }

        public async Task<bool> Delete(string id, IClientSessionHandle session = null)
        {
            var result = await (session == null
                ? _collection.DeleteOneAsync(n => n.ID == id)
                : _collection.DeleteOneAsync(session, n => n.ID == id));

            return result.IsAcknowledged;
        }

        public Task<bool> Save(T document, IClientSessionHandle session = null)
        {
            return Save(document, true, session);
        }

        public async Task<bool> Save(T document, bool isUpsert, IClientSessionHandle session = null)
        {
            if (string.IsNullOrWhiteSpace(document.ID))
                document.ID = ObjectId.GenerateNewId().ToString();

            var result = await (session == null
                ? _collection.ReplaceOneAsync(n => n.ID == document.ID, document, new ReplaceOptions { IsUpsert = isUpsert })
                : _collection.ReplaceOneAsync(session, n => n.ID == document.ID, document, new ReplaceOptions { IsUpsert = isUpsert }));

            return result.IsAcknowledged;
        }

        public Task<BulkWriteResult<T>> BulkSave(IEnumerable<T> documents, IClientSessionHandle session = null)
        {
            return BulkSave(documents, true, session);
        }

        public async Task<BulkWriteResult<T>> BulkSave(IEnumerable<T> documents, bool isUpsert, IClientSessionHandle session = null)
        {
            var bulkOps = new List<WriteModel<T>>();

            foreach (var doc in documents)
            {
                if (string.IsNullOrWhiteSpace(doc.ID))
                    doc.ID = ObjectId.GenerateNewId().ToString();

                var filter = Builders<T>.Filter.Eq(n => n.ID, doc.ID);

                var replaceOne = new ReplaceOneModel<T>(filter, doc) { IsUpsert = isUpsert };

                bulkOps.Add(replaceOne);
            }

            return await (session == null
                ? _collection.BulkWriteAsync(bulkOps)
                : _collection.BulkWriteAsync(session, bulkOps));
        }
    }
}
