using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TajniacyAPI.DataAccess.Model;

namespace TajniacyAPI.DataAccess.Repository.Interfaces
{
    public interface IWordCardsRepo : IMongoRepository<WordCard>
    {
    }
}
