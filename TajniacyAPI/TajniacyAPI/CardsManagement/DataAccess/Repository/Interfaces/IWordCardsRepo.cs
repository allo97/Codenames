using MongoDB.Driver;
using System.Threading.Tasks;
using TajniacyAPI.CardsManagement.DataAccess.Model;
using TajniacyAPI.MongoAPI.Interfaces;

namespace TajniacyAPI.CardsManagement.DataAccess.Repository.Interfaces
{
    public interface IWordCardsRepo : IMongoRepository<WordCard>
    {
        Task<WordCard> GetWordCardByWord(string word, IClientSessionHandle session = null);
    }
}
