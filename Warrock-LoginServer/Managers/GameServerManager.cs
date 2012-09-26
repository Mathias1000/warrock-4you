using Warrock_Lib;
using Warrock_InterLib;
using Warrock.Util;
using System.Collections.Concurrent;
using System.Timers;
using Warrock.InterServer;

namespace Warrock_LoginServer.Managers
{
    [ServerModule(InitializationStage.Clients)]
   public  class GameServerManager
    {
        public static GameServerManager Instance { get; private set; }
        public ConcurrentDictionary<byte, GameConnection> GameServers { get; private set; }
        public ConcurrentBag<int> InterServerConnections { get; private set; }
        public int ServerCount { get { return GameServers.Count; } }

        private readonly Timer updater; //updates the game Server Status

        [InitializerMethod]
        public static bool Load()
        {
            Instance = new GameServerManager();
            Log.WriteLine(LogLevel.Info, "LoginManager initialized.");
            return true;
        }
       	public GameServerManager()
		{
			GameServers = new ConcurrentDictionary<byte, GameConnection>();

            updater = new Timer(10000);
			updater.Elapsed += new ElapsedEventHandler(UpdaterElapsed);
			updater.Start();
		}
        void UpdaterElapsed(object sender, ElapsedEventArgs e)
        {
            if (ServerCount == 0) return;
            foreach (var Server in GameServers.Values)
            {
  
            }
        }
    }
}
