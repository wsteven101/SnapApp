using System;
using System.Collections.Generic;
using System.Text;

namespace SnapGame.Interfaces
{
    interface IGameActionResult<T> : IGameAction
    {
        T Result { get; }
    }
}
