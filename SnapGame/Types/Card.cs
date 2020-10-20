using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace SnapGame.Types
{


    public class Card
    {
        public enum CardSuit { Spades = 0, Hearts = 1, Clubs = 2, Diamonds = 3 }

        public enum CardFace { Ace = 0, Two = 1, Three = 2, Four = 3, Five = 4, 
                        Six = 5, Seven = 6, Eight = 7, Nine = 8, Ten = 9, 
                        Jack = 10, Queen = 11, King = 12 }

        public CardSuit Suit { get; set; }
        public CardFace Face { get; set; }
        public int CardValue { get { return GetCardValue(); } }

        private const int noOfCardsInSuit = 13;
        private const int noOfCardsInPack = noOfCardsInSuit * 4;

        public Card(int cardValue)
        {
            (Suit, Face) = FromValueToCardDetails(cardValue);
        }

        public Card(CardSuit suit, CardFace face)
        {
            Suit = suit;
            Face = face;
        }

        public override string ToString()
        {
            return Face.ToString() + " of " + Suit.ToString();
        }

        private (CardSuit, CardFace) FromValueToCardDetails(int cardValue)
        {
            if ( (cardValue < 0) || (cardValue >= noOfCardsInPack) )
            {
                throw new Exception($"Card value {cardValue} is not valid");
            }

            var facefValue = cardValue % noOfCardsInSuit;
            var suitValue = (cardValue - facefValue) / noOfCardsInSuit;

            return ( ((CardSuit)suitValue, (CardFace)facefValue) );
        }

        private int GetCardValue() => (Convert.ToInt32(Suit) * noOfCardsInSuit) + Convert.ToInt32(Face); 

    }
}
