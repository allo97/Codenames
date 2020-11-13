using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TajniacyAPI.CardsManagement.DataAccess.Interfaces;
using TajniacyAPI.CardsManagement.DataAccess.Model;
using TajniacyAPI.CardsManagement.Services.Interfaces;

namespace TajniacyAPI.CardsManagement.Services.Implementations
{
    public class CardsService : ICardsService
    {
        private readonly ICardsUnitOfWork _cardsUnitOfWork;

        public CardsService(ICardsUnitOfWork tajniacyUnitOfWork)
        {
            _cardsUnitOfWork = tajniacyUnitOfWork;
        }
        public async Task<List<WordCard>> GetAllCards()
        {
            var result = await _cardsUnitOfWork.WordCardsRepo.GetAll();
            if (result == null)
                throw new Exception($"Can't get Words Cards from mongoDB");

            return result;
        }

        public async Task<WordCard> AddCard(string word)
        {
            var existingCard = await _cardsUnitOfWork.WordCardsRepo.GetByWord(word);
            if (existingCard != null)
                throw new Exception($"Card {existingCard.Word} already exists.");

            var wordCard = new WordCard
            {
                Word = word
            };

            var result = await _cardsUnitOfWork.WordCardsRepo.Save(wordCard);
            if (!result)
                throw new Exception($"Can't save card {word} to mongoDB");

            return wordCard;
        }

        public async Task<WordCard> UpdateCard(WordCard wordCard)
        {
            var existingWord = await _cardsUnitOfWork.WordCardsRepo.GetById(wordCard.ID);
            if (existingWord == null)
                throw new Exception($"Can't find card {wordCard.Word} in database.");

            existingWord.Word = wordCard.Word;

            var result = await _cardsUnitOfWork.WordCardsRepo.Update(existingWord);
            if (!result)
                throw new Exception($"Can't update card {existingWord.Word} to mongoDB");

            return wordCard;
        }

        public async Task DeleteCard(string id)
        {
            var result = await _cardsUnitOfWork.WordCardsRepo.Delete(id);
            if (!result)
                throw new Exception($"This card already doesn't exist in mongoDB!");
        }

        public async Task<BulkWriteResult> InitWordCards()
        {
            List<WordCard> wordCards = new List<WordCard>();

            for (int i = 0; i < 1000; i++)
            {
                wordCards.Add(new WordCard
                {
                    Word = "Słowo" + i
                });
            }
            return await _cardsUnitOfWork.WordCardsRepo.BulkSave(wordCards);
        }

        public async Task<BulkWriteResult> InitSelectionCards()
        {
            List<SelectionCard> selectionCards = new List<SelectionCard>();

            //create blue cards
            for (int i = 0; i < 8; i++)
            {
                selectionCards.Add(new SelectionCard
                {
                    CardType = CardType.Blue
                });
            }

            //create red cards
            for (int i = 0; i < 8; i++)
            {
                selectionCards.Add(new SelectionCard
                {
                    CardType = CardType.Red
                });
            }

            //create neutral cards
            for (int i = 0; i < 7; i++)
            {
                selectionCards.Add(new SelectionCard
                {
                    CardType = CardType.Neutral
                });
            }

            //create black card
            selectionCards.Add(new SelectionCard
            {
                CardType = CardType.Black
            });

            return await _cardsUnitOfWork.SelectionCardsRepo.BulkSave(selectionCards);
        }
    }
}
