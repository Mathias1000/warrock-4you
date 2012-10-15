using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Warrock.CommandHandlers
{
    public class IngameCommandInfo
    {
        public string Command { get; private set; }
        public IngameCommandHandler.Command Function { get; private set; }
        public string[] Parameters { get; private set; }
        public byte GmLevel { get; private set; }

        public IngameCommandInfo(string command, IngameCommandHandler.Command function, byte gmlevel, string[] param)
        {
            Command = command;
            Function = function;
            GmLevel = gmlevel;
            Parameters = param;
        }
    }
}
