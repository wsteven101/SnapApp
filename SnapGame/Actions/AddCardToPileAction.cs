using System;
using System.Collections.Generic;
using System.Text;
using SnapGame.Interfaces;
using SnapGame.Types;

namespace SnapGame.Actions
{
    class AddCardToPileAction : IGameActionResult<List<Card>>
    { 
        public List<Card>  Result { get; }
        private readonly Card _card;


        public AddCardToPileAction(List<Card> cards, Card card) 
        {
            Result = new List<Card>(cards);
            _card = card;
        }

        public void Play()
        {
            Result.Add(_card);
        }

        public void Report()
        {
            Console.WriteLine($"Adding card {_card.ToString()} to common pile.");
        }
    }
}
