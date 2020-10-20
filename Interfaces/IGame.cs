using System;
using System.Collections.Generic;
using System.Text;

namespace SnapGame.Interfaces
{
    public interface IGame<T>
    {
        public T Run();
    }
}
