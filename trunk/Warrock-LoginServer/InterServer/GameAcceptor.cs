using System;
using System.Net.Sockets;
using Warrock.Util;
using Warrock_InterLib.NetworkObjects;
using Warrock.InterServer;

namespace Warrock_LoginServer.InterServer
{
    [ServerModule(InitializationStage.Services)]
    public sealed class GameAcceptor : AbstractAcceptor
    {
        public static GameAcceptor Instance { get; private set; }

        public GameAcceptor(int port)
            : base(port)
        {
            this.OnIncommingConnection += new OnIncomingConnectionDelegate(GameAcceptor_OnIncommingConnection);
            Log.WriteLine(LogLevel.Info, "Listening on port {0}", port);
        }

        private void GameAcceptor_OnIncommingConnection(Socket session)
        {
            // So something with it X:
            Log.WriteLine(LogLevel.Info, "Incomming connection from {0}", session.RemoteEndPoint);
            GameConnection wc = new GameConnection(session);
        }

        [InitializerMethod]
        public static bool Load()
        {
            return Load(1000);
        }

        public static bool Load(int port)
        {
            try
            {
                Instance = new GameAcceptor(port);
                return true;
            }
            catch { return false; }
        }

    }
}
