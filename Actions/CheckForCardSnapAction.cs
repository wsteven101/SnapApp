using System;
using System.Collections.Generic;
using System.Text;
using SnapGame.Interfaces;
using SnapGame.Types;

namespace SnapGame.Actions
{
    class CheckForCardSnapAction : IGameActionResult<int?>
    {
        public int? Result { get => _result; }

        #region private vars
        private readonly IGameContext _gameContext;
        private readonly List<Card> _commonPile;
        private int? _result = null;
        #endregion


        public CheckForCardSnapAction(IGameContext gameContext, List<Card> commonPile)
        {
            _gameContext = gameContext;
            _commonPile = commonPile;
        }

        public void Play()
        {
            if (_commonPile.Count < 2)
            {
                return;
            }

            var faceValueMatch = !_gameContext.GameVariation.HasFlag(SnapType.FaceValue) ? true :
                    (_commonPile[_commonPile.Count - 1].Face == _commonPile[_commonPile.Count - 2].Face) ;
            var suitValueMatch = !_gameContext.GameVariation.HasFlag(SnapType.SuitValue) ? true :
                    (_commonPile[_commonPile.Count - 1].Suit == _commonPile[_commonPile.Count - 2].Suit) ;
                
            if (faceValueMatch && suitValueMatch)
            {
                _result = _gameContext.RandomGenerator.GetRndPlayer();
            }
        }

        public void Report()
        {
            if (Result.HasValue)
            {
                var card1 = _commonPile[_commonPile.Count - 1];
                var card2 = _commonPile[_commonPile.Count - 2];

                Console.WriteLine($"\n++ SNAP!!! The card '{card1}' snaps with '{card2}'");
                Console.WriteLine($"++ Player '{_gameContext.Players[Result.Value]}' takes the pile!\n");
            }
        }

    }
}
