using System;
using System.Net.Sockets;
using Warrock_Lib.Networking;
using Warrock.Util;


namespace Warrock.Networking
{
    public sealed class GameAcceptor : Listener
    {
        public static GameAcceptor Instance { get; private set; }

        public GameAcceptor(int port)
            : base(port)
        {
            Start();
            Log.WriteLine(LogLevel.Info, "Listening at port {0} for incoming clients.", port);
        }

        public override void OnClientConnect(Socket socket)
        {
            GameClient client = new GameClient(socket);
   
         //  ClientManager.Instance.AddClient(client); //They register once authenticated now
            Log.WriteLine(LogLevel.Debug, "Client connected from {0}", client.Host);
           // ClientManager.Instance.AddClient(client); //They register once authenticated now
        }

        public static bool Load()
        {
            try
            {
                Instance = new GameAcceptor(5540);//later in settings
                return true;
            }
            catch (Exception ex)
            {
                Log.WriteLine(LogLevel.Exception, "ZoneAcceptor exception: {0}", ex.ToString());
                return false;
            }
        }
    }
}
