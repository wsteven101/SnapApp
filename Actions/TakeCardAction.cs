using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using SnapGame.Interfaces;
using SnapGame.Types;

namespace SnapGame.Actions
{
    class TakeCardFromPileAction : IGameActionResult<(Card CardTaken, List<Card> NewCardPile)>
    {
        public (Card CardTaken, List<Card> NewCardPile) Result { get => _result; }

        private (Card CardTaken, List<Card> NewCardPile) _result;
        private readonly List<Card> _cards = new List<Card>();


        public TakeCardFromPileAction(List<Card> cards) => _cards = cards;

        public void Play()
        {
            Card cardTaken = null;
            var newPile = new List<Card>(_cards);

            if (_cards.Count > 0)
            {
                cardTaken = _cards[_cards.Count - 1];
                newPile.RemoveAt(_cards.Count - 1);
            }

            _result = (cardTaken, newPile);
        }

        public void Report() { }
    }
}
