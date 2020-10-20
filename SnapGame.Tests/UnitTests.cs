using System;
using Xunit;
using SnapGame;
using SnapGame.Interfaces;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using Moq;
using System.Threading;
using SnapGame.Types;
using SnapGame.Common;
using SnapGame.Actions;

namespace SnapGame.Tests
{
    public class UnitTests
    {
        [Theory,
            InlineData(SnapType.FaceValue,"Lisa"),
            InlineData(SnapType.FaceValue, "Bart"),
            InlineData(SnapType.SuitValue,"Lisa"),
            InlineData(SnapType.SuitValue, "Bart")
            ]
        public void test_game_and_results(SnapType gameVariation, string expectedWinner, int noOfDecks = 1)
        {
            var players = new List<string> { "Bart", "Lisa" };

            var realRndGen = new Random();
            var rndGen = new Mock<IGameRandomGenerator>();
            rndGen.Setup(r => r.GetRndPlayer()).Returns(players.IndexOf(expectedWinner));
            rndGen.Setup(r => r.GetRndCardValue(It.IsAny<int>())).Returns((int x) => realRndGen.Next(x) );

            var game = new SnapGameAction(gameVariation, players, noOfDecks, rndGen.Object);

            game.Play();

            Assert.Equal(expectedWinner, game.Result.WinnerNames);
        }

        [Theory,
            InlineData(0,Card.CardSuit.Spades,Card.CardFace.Ace),
            InlineData(9, Card.CardSuit.Spades, Card.CardFace.Ten),
            InlineData(12, Card.CardSuit.Spades, Card.CardFace.King), 
            InlineData(13, Card.CardSuit.Hearts, Card.CardFace.Ace),
            InlineData(51, Card.CardSuit.Diamonds, Card.CardFace.King)]
        public void test_card_creation_from_value(int cardValue, Card.CardSuit expectedSuit, Card.CardFace expectedCardFace)
        {
            Card c = new Card(cardValue);
            Assert.Equal(expectedSuit, c.Suit);
            Assert.Equal(expectedCardFace, c.Face);
            Assert.Equal(cardValue, c.CardValue);
        }

        [Theory,
            InlineData(-1),
            InlineData(52),
            InlineData(100)]
        public void test_card_creation_from_value_limits(int cardValue)
        {
            Assert.Throws<Exception>(() => new Card(cardValue));
        }

        [Fact]
        public void test_game_context_creation()
        {
            var rndGen = new Mock<IGameRandomGenerator>();
            var ctx = new GameContext(SnapType.FaceValue,new List<string> { "Bart", "Lisa" }, 1, rndGen.Object, 4);

            Assert.Equal(2, ctx.NoOfPlayers);
            Assert.Equal(4, ctx.NoOfCards);
            Assert.IsAssignableFrom<IGameRandomGenerator>(ctx.RandomGenerator);

            ctx = new GameContext(SnapType.FaceValue,new List<string> { "Bart", "Lisa" }, 2, rndGen.Object);
            Assert.Equal(104, ctx.NoOfCards);
        }

        [Fact]
        public void test_prepare_cards_action()
        {
            var rndGen = new Mock<IGameRandomGenerator>();
            rndGen.SetupSequence(r => r.GetRndCardValue(It.IsAny<int>())).Returns(3).Returns(2).Returns(1).Returns(0);

            var ctx = new GameContext(SnapType.FaceValue,new List<string> { "Bart", "Lisa" }, 7, rndGen.Object);

            var action = new PrepareCardsAction(ctx);
            action.Play();

            Assert.Equal(ctx.NoOfCards, action.Result.Count);

            int idx = 0;
            foreach (var resCard in action.Result)
            {
                Assert.Equal(idx, resCard.CardValue);
                idx = (idx + 1) % ctx.NoOfCardsInDeck;
            }
        }

        [Fact]
        public void test_shuffle_card_action()
        {
            var testCardList = new List<Card> { new Card(0), new Card(15), new Card(34), new Card(45) };

            var rndGen = new Mock<IGameRandomGenerator>();
            rndGen.SetupSequence(r => r.GetRndCardValue(It.IsAny<int>())).Returns(3).Returns(2).Returns(1).Returns(0);

            var ctx = new GameContext(SnapType.FaceValue,new List<string> {"Bart","Lisa" }, 1, rndGen.Object, testCardList.Count);

            var shuffleAction = new ShuffleCardsAction(ctx,testCardList);
            shuffleAction.Play();

            int idx = testCardList.Count - 1;
            foreach(var resCard in shuffleAction.Result)
            {
                Assert.Equal(testCardList[idx].CardValue, resCard.CardValue);
                idx--;
            }

        }

        [Fact]
        public void test_take_card()
        {
            var testCardList = new List<Card> { new Card(0), new Card(15), new Card(34), new Card(45) };

            var action = new TakeCardFromPileAction(testCardList);

            action.Play();

            Assert.Equal(testCardList[testCardList.Count - 1], action.Result.CardTaken);
            Assert.Equal(testCardList.Count-1,action.Result.NewCardPile.Count);
            Assert.Equal(testCardList[testCardList.Count - 2], action.Result.NewCardPile[action.Result.NewCardPile.Count - 1]);
        }

        [Fact]
        public void test_add_card_to_pile()
        {
            var testCardList = new List<Card> { new Card(0), new Card(15), new Card(34), new Card(45) };
            var origLen = testCardList.Count;
            var card = new Card(27);

            var action = new AddCardToPileAction(testCardList, card);

            action.Play();

            Assert.Equal(origLen + 1, action.Result.Count);
            Assert.Equal(card.CardValue,action.Result[action.Result.Count-1].CardValue);
        }

        [Theory,
            InlineData(SnapType.FaceValue, 0, 0, 15, 45, 45),   // no match i.e. player no. 0
            InlineData(SnapType.FaceValue, 2, 0, 15, 45, 45), 
            InlineData(SnapType.FaceValue, 1, 0, 15, 45, 45),
            InlineData(SnapType.FaceValue, 2, 0, 15, 0, 13),
            InlineData(SnapType.SuitValue, 2, 0, 15, 44, 47),
            InlineData(SnapType.SuitValue, 1, 0, 15, 39, 47),
            InlineData(SnapType.FaceValue | SnapType.SuitValue, 1, 0, 15, 51, 51),
            InlineData(SnapType.FaceValue | SnapType.SuitValue, 1, 0, 15, 24, 24)]
        public void test_check_for_snap_match_action(SnapType gameVariation, int winningPlayerNo, int card1, int card2, int card3, int card4)
        {
            var testCardList = new List<Card> { new Card(card1), new Card(card2), new Card(card3), new Card(card4) };

            var ctx = CreateGameContext(winningPlayerNo, gameVariation, 1, testCardList.Count);

            var action = new CheckForCardSnapAction(ctx,testCardList);
            action.Play();

            Assert.Equal(winningPlayerNo, action.Result);
        }

        [Theory,
            InlineData(SnapType.FaceValue, 2, 0, 15, 0, 22),
            InlineData(SnapType.FaceValue, 1, 0, 15, 50, 51),
            InlineData(SnapType.SuitValue, 2, 0, 15, 0, 39),
            InlineData(SnapType.SuitValue, 1, 0, 15, 38, 47),
            InlineData(SnapType.FaceValue | SnapType.SuitValue, 2, 0, 15, 0, 13),
            InlineData(SnapType.FaceValue | SnapType.SuitValue, 1, 0, 15, 50, 51)]
        public void test_check_for_snap_no_match_action(SnapType gameVariation, int winningPlayerNo, int card1, int card2, int card3, int card4)
        {
            var testCardList = new List<Card> { new Card(card1), new Card(card2), new Card(card3), new Card(card4) };

            var ctx = CreateGameContext(winningPlayerNo, gameVariation, 1, testCardList.Count);

            var action = new CheckForCardSnapAction(ctx, testCardList);
            action.Play();

            Assert.NotEqual(winningPlayerNo, action.Result);
        }



        [Fact]
        public void test_check_for_winner()
        {
            var pileOne = new List<Card> { new Card(5), new Card(25) };
            var pileTwo = new List<Card> { new Card(5), new Card(17), new Card(38), new Card(45) };


            // test 1
            var playerPilesA = new List<List<Card>> { pileOne, pileTwo };

            var ctx = CreateGameContext(1, SnapType.FaceValue, 1, 52);

            var action = new DetermineWinnerAction(ctx, playerPilesA);
            action.Play();

            Assert.Equal("Lisa", action.Result.WinnerNames);

            // test 2 flip the winner
            var playerPilesB = new List<List<Card>> { pileTwo, pileOne };

            var ctx2 = CreateGameContext(1, SnapType.FaceValue, 1, 52);

            action = new DetermineWinnerAction(ctx2, playerPilesB);
            action.Play();

            Assert.Equal("Bart", action.Result.WinnerNames);
        }

        [Fact]
        public void test_check_for_draw()
        {
            var pileOne = new List<Card> { new Card(5), new Card(17), new Card(38), new Card(45) };
            var pileTwo = new List<Card> { new Card(5), new Card(17), new Card(38), new Card(45) };
            var playerPiles = new List<List<Card>> { pileOne, pileTwo };

            var ctx = CreateGameContext(1, SnapType.FaceValue, 1, 52);

            var action = new DetermineWinnerAction(ctx, playerPiles);
            action.Play();

            Assert.Contains("Lisa", action.Result.WinnerNames);
            Assert.Contains("Bart", action.Result.WinnerNames);
        }

        internal GameContext CreateGameContext(int winningPlayerNo, SnapType gameVariation, int NoOfDecks, int totalCardCount)
        {            
            var rndGen = new Mock<IGameRandomGenerator>();
            rndGen.SetupSequence(r => r.GetRndPlayer()).Returns(winningPlayerNo);
            var ctx = new GameContext(gameVariation, new List<string> { "Bart", "Lisa" }, 1, rndGen.Object, totalCardCount);
            return ctx;
        }

    }
}
