using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TajniacyAPI.DataAccess.Model;

namespace TajniacyAPI.Services.Interfaces
{
    public interface ICardsService
    {
        Task<List<WordCard>>GetAllCards();
        Task<WordCard> AddCard(string word);
        Task<WordCard> UpdateCard(WordCard wordCard);
        Task DeleteCard(string id);
        Task<BulkWriteResult> AddCards();
    }
}
