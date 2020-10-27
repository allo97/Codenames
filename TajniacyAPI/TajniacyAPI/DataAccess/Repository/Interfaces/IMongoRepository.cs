using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using TajniacyAPI.DataAccess.Model.Interfaces;

namespace TajniacyAPI.DataAccess.Repository.Interfaces
{
    public interface IMongoRepository<T> where T : IEntity
    {

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
    }
}
