using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using TajniacyAPI.CardsManagement.DataAccess.Model;
using TajniacyAPI.CardsManagement.Services.Interfaces;

namespace TajniacyAPI.CardsManagement.Controllers
{
    [Route("api/tajniacy/CardsManagement/[controller]/[action]")]
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
            try
            {
                return Ok(await _cardsService.GetAllCards());
            }
            catch (Exception ex)
            {
                var errorMessage = "There was an error while trying to list Word Cards from MongoDB";
                return BadRequest(errorMessage + "\n" + ex);
            }
        }

        /// <summary>
        /// Add Word Card to MongoDB
        /// </summary>
        /// <param name="word">Word that will appear on the card</param>
        [HttpPost]
        [ProducesResponseType(typeof(WordCard), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddCard([FromBody] string word)
        { 
            try
            {
                return Ok(await _cardsService.AddCard(word));
            }
            catch (Exception ex)
            {
                var errorMessage = "There was an error while trying to add card to MongoDB";
                return BadRequest(errorMessage + "\n" + ex);
            }
        }

        /// <summary>
        /// Update Word Card in MongoDB
        /// </summary>
        /// <param name="wordCard">Word Card to update in MongoDB</param>
        [HttpPut]
        [ProducesResponseType(typeof(WordCard), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateCard([FromBody] WordCard wordCard)
        {
            try
            {
                return Ok(await _cardsService.UpdateCard(wordCard));
            }
            catch (Exception ex)
            {
                var errorMessage = "There was an error while trying to list Word Cards from MongoDB";
                return BadRequest(errorMessage + "\n" + ex);
            }
        }

        /// <summary>
        /// Delete Word Card in MongoDB
        /// </summary>
        /// <param name="id">ID of Word Card to delete</param>
        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteCard(string id)
        {
            try
            {
                await _cardsService.DeleteCard(id);
                return Ok();
            }
            catch (Exception ex)
            {
                var errorMessage = "There was an error while trying to list Word Cards from MongoDB";
                return BadRequest(errorMessage + "\n" + ex);
            }
        }

        /// <summary>
        /// Call an initialization of basic cards - api/system/init
        /// </summary>
        /// <param name="setting">Enter "init" to initialize MongoDB</param> 
        [HttpGet("{setting}")]
        [ProducesResponseType(typeof(BulkWriteResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Exception), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Get(string setting)
        {
            try
            {
                if (setting == "init")
                {
                    var result = await _cardsService.AddCards();
                    return Ok(result);
                } else
                {
                    throw new Exception("Enter init to initialize DB!");
                } 
            }
            catch (Exception ex)
            {
                var errorMessage = "There was an error while trying to list Word Cards from MongoDB";
                return BadRequest(errorMessage + "\n" + ex);
            }

        }
    }
}
