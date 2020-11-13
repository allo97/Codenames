using MongoDB.Driver;
using TajniacyAPI.CardsManagement.DataAccess.Model;
using TajniacyAPI.CardsManagement.DataAccess.Repository.Interfaces;
using TajniacyAPI.MongoAPI.Implementations;

namespace TajniacyAPI.CardsManagement.DataAccess.Repository.Implementations
{
    public class SelectionCardsRepo : MongoRepository<SelectionCard>, ISelectionCardsRepo
    {
        public SelectionCardsRepo(IMongoCollection<SelectionCard> collection) : base(collection) { }
    }
}
