using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Warrock.CommandHandlers.CommandInfo
{
    public class CmdCommandInfo
    {
        public string Command { get; private set; }
        public CmdCommandHandler.Command Function { get; private set; }
        public string[] Parameters { get; private set; }

        public CmdCommandInfo(string command, CmdCommandHandler.Command function, string[] param)
        {
            Command = command;
            Function = function;
            Parameters = param;
        }
    }
}
