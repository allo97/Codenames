using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using TajniacyAPI.DataAccess.Model;
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

        /// <summary>
        /// List Word Cards from MongoDB
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<WordCard>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetAllCards()
        {
            return Ok(await _cardsService.GetAllCards());
        }
    }
}
