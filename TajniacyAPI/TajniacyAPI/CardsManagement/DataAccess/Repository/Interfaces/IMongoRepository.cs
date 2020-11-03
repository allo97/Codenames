using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using TajniacyAPI.CardsManagement.DataAccess.Model.Interfaces;

namespace TajniacyAPI.CardsManagement.DataAccess.Repository.Interfaces
{
    public interface IMongoRepository<T> where T : IEntity
    {
        /// <summary>
        /// Inserts or updates documents to collection.
        /// </summary>
        /// <param name="documents">List of documents</param>
        /// <param name="isUpsert">Determines if not found documents should be inserted</param>
        /// <param name="session">Transaction session</param>
        /// <returns></returns>
        Task<BulkWriteResult<T>> BulkSave(IEnumerable<T> documents, bool isUpsert, IClientSessionHandle session = null);
        /// <summary>
        /// Inserts or updates documents to collection. Upsert is enabled by default.
        /// </summary>
        /// <param name="documents">List of documents to insert</param>
        /// <param name="session">Transaction session</param>
        /// <returns></returns>
        Task<BulkWriteResult<T>> BulkSave(IEnumerable<T> documents, IClientSessionHandle session = null);
        /// <summary>
        /// Returns list of all documents in collection.
        /// </summary>
        /// <returns></returns>
        Task<List<T>> GetAll();
        /// <summary>
        /// Get document by ID.
        /// </summary>
        /// <param name="id">Document ID</param>
        /// <param name="session">Transaction session</param>
        /// <returns></returns>
        Task<T> GetById(string id, IClientSessionHandle session = null);
        /// <summary>
        /// Inserts or updates document to collection.
        /// </summary>
        /// <param name="document">Document</param>
        /// <param name="isUpsert">Determines if not found document should be inserted</param>
        /// <param name="session">Transaction session</param>
        /// <returns></returns>
        Task<bool> Save(T document, bool isUpsert, IClientSessionHandle session = null);
        /// <summary>
        /// Inserts or updates document to collection. Upsert is enabled by default.
        /// </summary>
        /// <param name="document">Document</param>
        /// <param name="session">Transaction session</param>
        /// <returns></returns>
        Task<bool> Save(T document, IClientSessionHandle session = null);
        /// <summary>
        /// Updates document in collection.
        /// </summary>
        /// <param name="document">Document</param>
        /// <param name="session">Transaction session</param>
        /// <returns></returns>
        Task<bool> Update(T document, IClientSessionHandle session = null);
        /// <summary>
        /// Deletes document in collection.
        /// </summary>
        /// <param name="id">Document ID</param>
        /// <param name="session">Transaction session</param>
        /// <returns></returns>
        Task<bool> Delete(string id, IClientSessionHandle session = null);
    }
}
