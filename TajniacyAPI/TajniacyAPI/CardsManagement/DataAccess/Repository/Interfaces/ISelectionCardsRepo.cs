using TajniacyAPI.CardsManagement.DataAccess.Model;
using TajniacyAPI.MongoAPI.Interfaces;

namespace TajniacyAPI.CardsManagement.DataAccess.Repository.Interfaces
{
    public interface ISelectionCardsRepo : IMongoRepository<SelectionCard>
    {

    }
}
