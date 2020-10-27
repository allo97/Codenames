using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TajniacyAPI.DataAccess.Model;

namespace TajniacyAPI.Services.Interfaces
{
    public interface ICardsService
    {
        Task<WordCard> GetAllCards();
    }
}
