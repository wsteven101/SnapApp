Snap Game

   Simulates a game of Snap between two automated players with simplified rules.

Dependencies:

    Moq 4.14.7 (or above)
    xunit 2.4.0 (or above)

Build:

    Contains one solution written and built with
        1) C# .Net Core 3.1 
        2) Visual Studio 16.7.3

    To Build
        1) Open the Visual Studio Solution 
        2) Select 'Build Solution' in the usual way for Visual Studio

Running the Application:

    1) Select the project 'SnapGame' in the 'Solution Explorer'
    2) Select 'Debug' from the VS menu and then 'Start without Debugging'

Testing:

    To run the unit tests:
        1) Right click the project 'SnapGame.Tests'
        2) From the pop-up menu select 'Run Tests'

Description:

    This is a simplified and automated game of snaps whereby a game of 
    Snaps is simulated between two players.

    The user enters 
        1) the number of decks to be played
        2) the method of determining a 'snap' or match between two cards

Rules that the game follows:

    There are two automated players
    The simulation starts with one imaginary card pile. The pile contains all
    of the cards of all the decks of cards e.g. 1 deck is 52 cards, 2 deck is 104 cards etc.
    The cards are then shuffled.
    A card is taken from the shuffled pile, turned face-up and added to a new pile of cards
    which is referred to in the source code as the 'common pile'.
    Both player now have an opportunity to cry 'Snap' if the card turned and added to the pile
    matches the previous card on the common pile.
    The first playe to cry Snap wins the 'common pile' and adds it to that player's
    personal card pile.
    The players then continue to turn the cards one by one, adding them to a new common pile.
    The game finishes once there re no more cards to turn and any remaining cards
    in the common pile are ignored.
    Once there are no more cards to turn the players' personal piles of cards
    are counted up and the player with the biggest pile of cards wins.
    A draw is also possible.

Matching:

    Cards may be matched by 
        1) face value i.e. a 10 of Hearts matches a 10 of Spades.
        2) suit value i.e. a 10 of Spades matches a King of Spades.
        3) by both of the above a 10 of Hearts will only match a 10 of Hearts.

    Note: When matching by both 'face' and 'suit' it is only possible to
    have a match if more then one deck of cards has been entered.





