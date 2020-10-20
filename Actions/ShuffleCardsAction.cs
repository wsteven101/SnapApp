using System;
using System.Collections.Generic;
using System.Text;
using SnapGame.Interfaces;
using SnapGame.Types;

namespace SnapGame.Actions
{
    class ShuffleCardsAction : IGameActionResult<List<Card>>
    {
        public List<Card> Result {get;} = new List<Card>();

        private readonly IGameContext _gameContext;
        private readonly List<Card> _unshuffledCards = new List<Card>();


        public ShuffleCardsAction(IGameContext gameContext,  List<Card> cards) 
        {
            _gameContext = gameContext;
            _unshuffledCards = cards;
        }

        public void Play() 
        {
            var unshuffledCardsCopy = new List<Card>(_unshuffledCards);

            for (int cardCounter = _gameContext.NoOfCards; cardCounter > 0; cardCounter--)
            {
                int cardPos = _gameContext.RandomGenerator.GetRndCardValue(unshuffledCardsCopy.Count);

                Result.Add(unshuffledCardsCopy[cardPos]);
                unshuffledCardsCopy.RemoveAt(cardPos);
            }
        }

        public void Report()
        {
            Console.WriteLine($"Shuffling cards....");
        }
    }
}
