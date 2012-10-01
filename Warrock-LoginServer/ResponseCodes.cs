using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Warrock_LoginServer
{
    public enum LoginResponse : int
    {
        ChangeNickName = 72000,
        UserNameNotFound = 72010,
        WrongPassword = 72020,
        AlreadyLogged = 72030,
        Banned = 73050,
        TimeBanned = 73020,
    }
}
