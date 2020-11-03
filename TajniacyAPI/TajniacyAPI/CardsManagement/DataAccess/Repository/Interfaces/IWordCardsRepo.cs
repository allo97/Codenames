using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TajniacyAPI.CardsManagement.DataAccess.Model;

namespace TajniacyAPI.CardsManagement.DataAccess.Repository.Interfaces
{
    public interface IWordCardsRepo : IMongoRepository<WordCard>
    {
        Task<WordCard> GetWordCardByWord(string word, IClientSessionHandle session = null);
    }
}
