using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
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
            var result = await _tajniacyUnitOfWork.WordCardsRepo.GetAll();
            if (result == null)
                throw new Exception($"Can't get Words Cards from mongoDB");

            return result;
        }

        public async Task<WordCard> AddCard(string word)
        {
            var wordCard = new WordCard
            {
                Word = word
            };
            var result = await _tajniacyUnitOfWork.WordCardsRepo.Save(wordCard);
            if (!result)
                throw new Exception($"Can't save card {word} to mongoDB");

            return wordCard;
        }

        public async Task<WordCard> UpdateCard(WordCard wordCard)
        {
            var existingWord = await _tajniacyUnitOfWork.WordCardsRepo.GetById(wordCard.ID);
            if (existingWord == null)
                throw new Exception($"Can't find card {wordCard.Word} in database.");

            existingWord.Word = wordCard.Word;

            var result = await _tajniacyUnitOfWork.WordCardsRepo.Update(existingWord);
            if (!result)
                throw new Exception($"Can't update card {existingWord.Word} to mongoDB");

            return wordCard;
        }

        public async Task DeleteCard(string id)
        {
            var result = await _tajniacyUnitOfWork.WordCardsRepo.Delete(id);
            if (!result)
                throw new Exception($"This card already doesn't exist in mongoDB!");
        }

        public async Task<BulkWriteResult> AddCards()
        {
            List<WordCard> wordCards = new List<WordCard>();

            for (int i = 0; i < 25; i++)
            {
                wordCards.Add(new WordCard
                {
                    Word = "Słowo" + i
                });
            }
            return await _tajniacyUnitOfWork.WordCardsRepo.BulkSave(wordCards);
        }
    }
}
