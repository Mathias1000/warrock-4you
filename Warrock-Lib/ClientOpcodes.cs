using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Warrock_Lib
{
    public enum ClientLoginOpcodes : int
    {
        PatchRequest = 4112,
        Login = 4352,
        ChangeNickname = 4353,

    }
    public enum ClientGameOpcode : int
    {
        Welcome = 91337,

    }
}
