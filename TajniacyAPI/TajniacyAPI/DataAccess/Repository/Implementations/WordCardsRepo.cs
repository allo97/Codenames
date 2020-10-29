using MongoDB.Driver;
using System.Threading.Tasks;
using TajniacyAPI.DataAccess.Model;
using TajniacyAPI.DataAccess.Repository.Base;
using TajniacyAPI.DataAccess.Repository.Interfaces;

namespace TajniacyAPI.DataAccess.Repository.Implementations
{
    public class WordCardsRepo : MongoRepository<WordCard>, IWordCardsRepo
    {
        public WordCardsRepo(IMongoCollection<WordCard> collection) : base(collection) { }

        public Task<WordCard> GetWordCardByWord(string word, IClientSessionHandle session = null) => session == null
            ? _collection.Find(n => n.Word == word).FirstOrDefaultAsync()
            : _collection.Find(session, n => n.Word == word).FirstOrDefaultAsync();
    }
}
