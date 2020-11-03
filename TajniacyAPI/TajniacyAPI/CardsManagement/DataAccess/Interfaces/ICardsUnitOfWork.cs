using TajniacyAPI.CardsManagement.DataAccess.Repository.Interfaces;
using TajniacyAPI.MongoAPI.Interfaces;

namespace TajniacyAPI.CardsManagement.DataAccess.Interfaces
{
    public interface ICardsUnitOfWork : IMongoDBUnitOfWork
    {
        IWordCardsRepo WordCardsRepo { get; }
    }
}
