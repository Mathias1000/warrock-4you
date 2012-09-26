using System;
using System.Net.Sockets;
using Warrock.Util;
using Warrock_Lib.Networking;
using Warrock_LoginServer.Managers;

namespace Warrock_LoginServer.Networking
{
    [ServerModule(InitializationStage.Networking)]
    public sealed class LoginAcceptor : Listener
    {
        public static LoginAcceptor Instance { get; private set; }
        public LoginAcceptor(int port)
            : base(port)
        {
            Start();
            Log.WriteLine(LogLevel.Info, "Accepting clients on port {0}", port);
        }

        public override void OnClientConnect(Socket socket)
        { 
            LoginClient client = new LoginClient(socket);
            ClientManager.Instance.AddClient(client);
            Log.WriteLine(LogLevel.Debug, "Client connected from {0}", client.Host);
        }

        [InitializerMethod]
        public static bool Load()
        {
            try
            {
                Instance = new LoginAcceptor(Config.Instance.LoginServerPort);
                return true;
            }
            catch (Exception ex)
            {
                Log.WriteLine(LogLevel.Exception, "LoginAcceptor exception: {0}", ex.ToString());
                return false;
            }
        }
    }
}
