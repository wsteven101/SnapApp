using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;
using SnapGame.Interfaces;
using SnapGame.Types;
using SnapGame.Common;

[assembly: InternalsVisibleTo("SnapGame")]

namespace SnapGame
{

    class GameContext : IGameContext
    {
        public SnapType GameVariation { get; }
        public List<string> Players { get; } = new List<string>();
        public int NoOfPlayers { get => Players.Count; }
        public int NoOfCardsInDeck { get; }
        public int NoOfDecks { get; }
        public int NoOfCards { get => NoOfDecks * NoOfCardsInDeck; }
        public IGameRandomGenerator RandomGenerator { get; }

        public GameContext(SnapType gameVariation, List<string> players, int noOfDecks, int noOfCardsInDeck) : 
            this(gameVariation, players, noOfDecks, null)
        {
            NoOfCardsInDeck = noOfCardsInDeck;
        }

        public GameContext(SnapType gameVariation, List<string> players, int noOfDecks, IGameRandomGenerator randomGenerator = null,
                            int? noOfCardsInDeck = null)
        {
            Players = players;
            NoOfDecks = noOfDecks;
            GameVariation = gameVariation;

            NoOfCardsInDeck = noOfCardsInDeck.HasValue ? noOfCardsInDeck.Value : GetNoOfCardsInPack();
            RandomGenerator = randomGenerator ?? new GameRandomGenerator(NoOfPlayers, NoOfCards);
        }

        private int GetNoOfCardsInPack()
        {
            return Enum.GetNames(typeof(Card.CardSuit)).Length * Enum.GetNames(typeof(Card.CardFace)).Length;
        }
    }
}
