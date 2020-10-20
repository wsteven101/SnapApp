using System;
using System.Collections.Generic;
using System.Text;


namespace SnapGame.Interfaces
{
    public interface IGameRandomGenerator
    {
        public int GetRndPlayer();

        public int GetRndCardValue(int? noOfCards);
    }
}
