using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using SnapGame.Interfaces;
using SnapGame.Types;
using System.IO;

namespace SnapGame.Actions
{
    class DetermineWinnerAction : IGameActionResult<SnapGameResult>
    {
        public SnapGameResult Result { get => _result; }

        #region private vars
        private SnapGameResult _result;
        private readonly List<List<Card>> _playerCardPile;
        private readonly IGameContext _gameContext;
        #endregion


        public DetermineWinnerAction(IGameContext gameContext, List<List<Card>> playerCardPile)
        {
            _gameContext = gameContext;
            _playerCardPile = playerCardPile;
        }

        public void Play()
        {
            var pileLengths = _playerCardPile.Select(p => p.Count).ToList();
            var biggestPileSize = pileLengths.Max();
            var winnerList = _playerCardPile.Where(p => (p.Count == biggestPileSize) && (p.Count != 0)).
                Select(w => _playerCardPile.IndexOf(w)).ToList();

            _result = new SnapGameResult(_gameContext, pileLengths, winnerList);
        }

        public void Report()
        {
            Console.WriteLine($"\n\nEnd of game, all the cards used up.");

            if (Result.Winners.Count == 0)
            {
                Console.WriteLine("Whoops!!! There was no winner, no cards were matched!!!");
            }
            else if (Result.Winners.Count == 1)
            {
                Console.WriteLine($"The winner is {Result.WinnerNames}!");
            }
            else
            {
                Console.WriteLine($"There was a draw between {Result.WinnerNames}");
            }
        }
    }
}
