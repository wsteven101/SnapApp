using System;
using System.Collections.Generic;
using System.Text;
using SnapGame.Interfaces;
using SnapGame.Types;

namespace SnapGame.Actions
{
    class PrepareCardsAction : IGameActionResult<List<Card>>
    {
        public List<Card> Result { get; } = new List<Card>();
        private readonly IGameContext _gameContext;


        public PrepareCardsAction(IGameContext gameContext)
        {
            _gameContext = gameContext;
            Result = new List<Card>(_gameContext.NoOfCards);
        }

        public void Play()
        {
            for (int cardValue = 0; cardValue < _gameContext.NoOfCards; cardValue++)
            {
                Result.Add(new Card(cardValue % _gameContext.NoOfCardsInDeck));
            }
        }

        public void Report()
        {
            Console.WriteLine($"There are {Result.Count} cards piled up on the table ready to use....\n");
        }
    }
}
