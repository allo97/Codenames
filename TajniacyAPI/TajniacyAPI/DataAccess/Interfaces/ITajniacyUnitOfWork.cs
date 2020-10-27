using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TajniacyAPI.DataAccess.Repository.Interfaces;

namespace TajniacyAPI.DataAccess.Interfaces
{
    public interface ITajniacyUnitOfWork : IMongoDBUnitOfWork
    {
        IWordCardsRepo WordCardsRepo { get; }
    }
}
