using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TajniacyAPI.DataAccess.Interfaces;
using TajniacyAPI.DataAccess.Model;
using TajniacyAPI.Services.Interfaces;

namespace TajniacyAPI.Services.Implementations
{
    public class CardsService : ICardsService
    {
        private readonly ITajniacyUnitOfWork _tajniacyUnitOfWork;

        public CardsService(ITajniacyUnitOfWork tajniacyUnitOfWork)
        {
            _tajniacyUnitOfWork = tajniacyUnitOfWork;
        }
        public async Task<List<WordCard>> GetAllCards()
        {
            return await _tajniacyUnitOfWork.WordCardsRepo.GetAll();
        }
    }
}
