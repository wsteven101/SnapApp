using System;
using System.Collections.Generic;
using System.Text;

namespace SnapGame.Types
{

    [Flags]
    public enum SnapType { 
        None = 0,
        FaceValue = 1, 
        SuitValue = 2 
    }

}
