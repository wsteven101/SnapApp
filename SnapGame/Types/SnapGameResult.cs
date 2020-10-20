using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using SnapGame.Interfaces;
using SnapGame.Types;

namespace SnapGame.Types
{
    public class SnapGameResult
    {
        public readonly IGameContext GameContext;
        public readonly List<int> PlayerPileCounts;
        public readonly List<int> Winners;
        public string WinnerNames => GetWinners();

        public SnapGameResult(IGameContext gameContext, List<int> playerPileCounts, List<int> winners)
        {
            GameContext = gameContext;
            PlayerPileCounts = playerPileCounts;
            Winners = winners;
        }

        public string GetWinners()
        {
            string winnerNames = "";
            foreach(var winner in Winners)
            {
                if (winnerNames != "")
                {
                    winnerNames += " and ";   // it's a draw!
                }

                winnerNames += GameContext.Players[winner];
             }

            return winnerNames;
        }
    }
}
