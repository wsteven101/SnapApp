using System;
using System.Collections.Generic;
using System.Text;
using SnapGame.Types;


namespace SnapGame.Interfaces
{
    public interface IGameContext
    {
        public SnapType GameVariation { get; }
        public List<string> Players { get; }
        public int NoOfPlayers { get; }
        public int NoOfCards { get; }
        public int NoOfCardsInDeck { get; }
        public IGameRandomGenerator RandomGenerator { get; }
    }
}
