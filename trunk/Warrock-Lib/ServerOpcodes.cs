﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Warrock_Lib
{
    public enum LoginServerOpcodes : int
    {
        SendPatchVersion = 4112,
        LoginResponse =  4352,
    }
    public enum GameServerOpcodes : int
    {
        Welcome = 24832,
    }
}
