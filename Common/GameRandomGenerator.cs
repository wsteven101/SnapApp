using System;
using System.Collections.Generic;
using System.Text;
using SnapGame.Interfaces;

namespace SnapGame.Common
{
    class GameRandomGenerator : IGameRandomGenerator
    {
        #region private vars
        private readonly int _noOfPlayers = 0;
        private readonly int _noOfCards = 0;
        private readonly Random _random = new Random();
        #endregion region

        public GameRandomGenerator(int noOfPlayers, int noOfCards) => (_noOfPlayers, _noOfCards) = (noOfPlayers, noOfCards);

        public GameRandomGenerator(int noOfPlayers, int noOfCards, int seed) 
        {
            _noOfPlayers = noOfPlayers;
            _noOfCards = noOfCards;
            _random = new Random(seed);
        }

        public int GetRndPlayer() => _random.Next(_noOfPlayers); 

        public int GetRndCardValue(int? noOfCards = null) =>  _random.Next(noOfCards.HasValue ? noOfCards.Value : _noOfCards ); 
    }
}
