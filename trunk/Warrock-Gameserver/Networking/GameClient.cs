using System;
using System.Net.Sockets;
using System.Reflection;
using Warrock_Lib;
using Warrock_Lib.Networking;
using Warrock.Util;
using Warrock.Data;
using Warrock.Handlers;
using Warrock.Lib;

namespace Warrock.Networking
{
    public sealed class GameClient : Client
    {
        public bool Authenticated { get; set; }
        public tUser AccountInfo { get; set; }
        public Player Player { get; set; }
        public bool HasPong { get; set; }
        public GameClient(Socket socket)
            : base(socket)
        {
            base.OnDisconnect += new EventHandler<SessionCloseEventArgs>(GameClient_OnDisconnect);
            base.OnPacket += new EventHandler<PacketReceivedEventArgs>(GameClient_OnPacket);
            Player.Client = this;
            HasPong = true;
            Authenticated = false;
        }

        void GameClient_OnPacket(object sender, PacketReceivedEventArgs e)
        {
          /* if (!Authenticated && !(e.Packet.Header == 6 && e.Packet.Type == 1)) return; //do not handle packets if not authenticated!
            MethodInfo method = HandlerStore.GetHandler(e.Packet.Header, e.Packet.Type);
            if (method != null)
            {
                Action action = HandlerStore.GetCallback(method, this, e.Packet);
                Worker.Instance.AddCallback(action);
            }
            else
            {
                Log.WriteLine(LogLevel.Debug, "Unhandled packet: {0}|{1}", e.Packet.Header, e.Packet.Type);
                Console.WriteLine(e.Packet.Dump());
            }*/
        }

        void GameClient_OnDisconnect(object sender, SessionCloseEventArgs e)
        {
            ClientManager.Instance.RemoveClient(this);
            Log.WriteLine(LogLevel.Debug, "Client disconnected.");
        }

        public override string ToString()
        {
            if (Player != null)
            {
                return "GameClient|Player:" + Player.PlayerName;
            }
            else
            {
                return "GameClient|NoPlayer";
            }
        }
    }
}
