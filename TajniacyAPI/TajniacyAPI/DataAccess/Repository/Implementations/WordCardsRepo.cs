using MongoDB.Driver;
using TajniacyAPI.DataAccess.Model;
using TajniacyAPI.DataAccess.Repository.Base;
using TajniacyAPI.DataAccess.Repository.Interfaces;

namespace TajniacyAPI.DataAccess.Repository.Implementations
{
    public class WordCardsRepo : MongoRepository<WordCard>, IWordCardsRepo
    {
        public WordCardsRepo(IMongoCollection<WordCard> collection) : base(collection) { }
    }
}
