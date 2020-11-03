using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TajniacyAPI.CardsManagement.DataAccess.Repository.Interfaces;

namespace TajniacyAPI.CardsManagement.DataAccess.Interfaces
{
    public interface ITajniacyUnitOfWork : IMongoDBUnitOfWork
    {
        IWordCardsRepo WordCardsRepo { get; }
    }
}
