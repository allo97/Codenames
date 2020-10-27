using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TajniacyAPI.Services.Interfaces;

namespace TajniacyAPI.Controllers
{
    [Route("api/tajniacy/[controller]")]
    [ApiController]
    public class CardsController : ControllerBase
    {
        private readonly ICardsService _cardsService;

        public CardsController(ICardsService cardsService)
        {
            _cardsService = cardsService;
        }

        public async Task<IActionResult> GetAllCards()
        {
            return Ok(await _cardsService.GetAllCards());
        }
    }
}
