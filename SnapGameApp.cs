using System;
using System.Collections.Generic;
using SnapGame.Types;
using SnapGame.Actions;

namespace SnapGame
{
    class SnapGameApp
    {
        static void Main(string[] args)
        {
            Console.WriteLine("++++++++++++++ Snap Game +++++++++++++++\n\n");

            int? noOfDecks = null;
            while (!noOfDecks.HasValue)
            {
                Console.WriteLine("Please enter the number of decks to be played (1-100):");
                var inputStr = Console.ReadLine();
                try
                {
                    noOfDecks = Convert.ToInt32(inputStr);
                }
                catch (FormatException) { }
            }
      
            int? matchType = null;
            while (!matchType.HasValue)
            {
                Console.WriteLine("\nSelect the method to match two cards for a 'Snap':");
                Console.WriteLine("  1) by face value");
                Console.WriteLine("  2) by suit value");
                Console.WriteLine("  3) by face and suit value");
                try
                {
                    matchType = Convert.ToInt32(Console.ReadLine());
                }
                catch (FormatException) { }

            }

            SnapType matchVariation = SnapType.None;
            switch(matchType.Value)
            {
                case 1: 
                    matchVariation = SnapType.FaceValue;
                    break;
                case 2:
                    matchVariation = SnapType.SuitValue;
                    break;
                case 3:
                    matchVariation = SnapType.FaceValue | SnapType.SuitValue;
                    break;
            }

            var players = new List<string> { "Player 1", "Player 2" };
            var game = new SnapGameAction(matchVariation, players, noOfDecks.Value);

            game.Play();

            Console.WriteLine("\nThanks for playing Snap.");
        }
    }
}
