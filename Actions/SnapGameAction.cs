using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using SnapGame.Interfaces;
using SnapGame.Types;
using System.Linq;

[assembly: InternalsVisibleTo("SnapGame")]

namespace SnapGame.Actions
{
    class SnapGameAction : IGameActionResult<SnapGameResult>
    {   
        public  SnapGameResult Result {  get => _result; }

        #region private vars     
        private List<IGameAction> _playedActions = new List<IGameAction>();
        private const int maxNoOfDecks = 100;
        private readonly int _noOfCardsInDeck = 0;        
        private readonly IGameContext _gameContext;
        private SnapGameResult _result;
        #endregion


        public SnapGameAction(SnapType winningMode, List<string> players, int noOfDecks, 
                                IGameRandomGenerator rndGen = null)
        {
            if ((noOfDecks < 0) || (noOfDecks > maxNoOfDecks))
            {
                throw new ArgumentOutOfRangeException($"No of decks must be between '0' and {noOfDecks}");
            }

            if (winningMode == SnapType.None)
            {
                throw new Exception("Invalid SnapType parameter passed");
            }

            _noOfCardsInDeck = Enum.GetNames(typeof(Card.CardSuit)).Length * 
                               Enum.GetNames(typeof(Card.CardFace)).Length;

            _gameContext = new GameContext(winningMode, players, noOfDecks, rndGen);
        }

        public void Play() 
        {
             // init lists

            var playerCardPile = new List<List<Card>>();
            for (int p = 0; p < _gameContext.NoOfPlayers; p++)
            {
                playerCardPile.Add(new List<Card>());
            }

             // setup the cards

            var prepareAction = ProcessAction(new PrepareCardsAction(_gameContext));

            var shuffleAction = ProcessAction(new ShuffleCardsAction(_gameContext, prepareAction.Result));

             // start playing

            PlayOff(shuffleAction.Result, playerCardPile);

             // decide the winner

            var determineWinnerAction = ProcessAction(new DetermineWinnerAction(_gameContext,playerCardPile));

            _result = determineWinnerAction.Result;   
        }

        public void PlayOff(List<Card> shuffledCards, List<List<Card>> playerCardPile)
        {
            var commonCardPile = new List<Card>();

            var takeCardFromPileAction = ProcessAction(new TakeCardFromPileAction(shuffledCards));
            var shuffledPile = takeCardFromPileAction.Result.NewCardPile;
             
             // whilst there are still cards in the common pile

            while (takeCardFromPileAction.Result.CardTaken != null)
            {
                var cardTaken = takeCardFromPileAction.Result.CardTaken;

                var addCardToPileAction = ProcessAction(new AddCardToPileAction(commonCardPile, cardTaken));
                commonCardPile = addCardToPileAction.Result;

                var checkForSnapAction = ProcessAction(new CheckForCardSnapAction(_gameContext, commonCardPile));

                if (checkForSnapAction.Result.HasValue)
                {
                     // Snap! player takes the pile

                    playerCardPile[checkForSnapAction.Result.Value].AddRange(commonCardPile);
                    commonCardPile.RemoveRange(0, commonCardPile.Count);
                }

                takeCardFromPileAction = ProcessAction(new TakeCardFromPileAction(shuffledPile));
                shuffledPile = takeCardFromPileAction.Result.NewCardPile;
            }

            CheckState(commonCardPile, playerCardPile);

        }

        private T ProcessAction<T>(T action)
        {
            IGameAction baseAction = (IGameAction) action;

            baseAction.Play();
            baseAction.Report();

            _playedActions.Add(baseAction);

            return action;
        }

        private void CheckState(List<Card> commonPile, List<List<Card>> playerPiles)
        {
            var totalCards = playerPiles.Select(p => p.Count).Sum() + commonPile.Count;
            if (totalCards != _gameContext.NoOfCards)
            {
                throw new Exception($"Serious Error: Total number of cards played {totalCards} does not tally with cards in deck {_gameContext.NoOfCards}");
            }
        }

        public void Report()
        {
            Console.WriteLine($"{Result}");
        }

    }
}
