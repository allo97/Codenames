using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using TajniacyAPI.CardsManagement.DataAccess.Model;

namespace TajniacyAPI.CardsManagement.Services.Interfaces
{
    public interface ICardsService
    {
        Task<List<WordCard>> GetAllCards();
        Task<WordCard> AddCard(string word);
        Task<WordCard> UpdateCard(WordCard wordCard);
        Task DeleteCard(string id);
        Task<BulkWriteResult> InitWordCards();
        Task<BulkWriteResult> InitSelectionCards();
    }
}
