using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Warrock.CommandHandlers.CommandInfo
{
    public class LobbyCommandInfo
    {
             public string Command { get; private set; }
        public LobbyCommandHandler.Command Function { get; private set; }
        public string[] Parameters { get; private set; }
        public byte GmLevel { get; private set; }

        public LobbyCommandInfo(string command, LobbyCommandHandler.Command function, byte gmlevel, string[] param)
        {
            Command = command;
            Function = function;
            GmLevel = gmlevel;
            Parameters = param;
        }
    }
}
