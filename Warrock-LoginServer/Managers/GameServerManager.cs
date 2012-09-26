using Warrock_Lib;
using Warrock_InterLib;
using Warrock.Util;
using System.Collections.Concurrent;
using Warrock.InterServer;

namespace Warrock_LoginServer.Managers
{
   public  class GameServerManager
    {
        public static GameServerManager Instance { get; private set; }
        public ConcurrentDictionary<byte, GameConnection> Worlds { get; private set; }
        public ConcurrentBag<int> InterServerConnections { get; private set; }
        public int WorldCount { get { return Worlds.Count; } }

    }
}
