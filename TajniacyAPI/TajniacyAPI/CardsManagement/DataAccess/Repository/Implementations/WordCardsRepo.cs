using MongoDB.Driver;
using System.Threading.Tasks;
using TajniacyAPI.CardsManagement.DataAccess.Model;
using TajniacyAPI.CardsManagement.DataAccess.Repository.Interfaces;
using TajniacyAPI.MongoAPI.Implementations;

namespace TajniacyAPI.CardsManagement.DataAccess.Repository.Implementations
{
    public class WordCardsRepo : MongoRepository<WordCard>, IWordCardsRepo
    {
        public WordCardsRepo(IMongoCollection<WordCard> collection) : base(collection) { }

        public Task<WordCard> GetWordCardByWord(string word, IClientSessionHandle session = null) => session == null
            ? _collection.Find(n => n.Word == word).FirstOrDefaultAsync()
            : _collection.Find(session, n => n.Word == word).FirstOrDefaultAsync();
    }
}
