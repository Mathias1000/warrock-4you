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
        public Warrock.Lib.tUser User { get; set; }
        public bool HasPong { get; set; }

        public GameClient(Socket socket)
            : base(socket)
        {
     
            this.Player = new Player();
            this.Player.Client = this;
            base.ccType = ClientType.GameClient;
            base.OnDisconnect += new EventHandler<SessionCloseEventArgs>(GameClient_OnDisconnect);
            base.OnPacket += new EventHandler<PacketReceivedEventArgs>(GameClient_OnPacket);
            HasPong = true;
            Authenticated = false;
        }

        void GameClient_OnPacket(object sender, PacketReceivedEventArgs e)
        {
            MethodInfo method = HandlerStore.GetHandler(e.Packet.OPCode);
            if (method != null)
            {
                Action action = HandlerStore.GetCallback(method, this, e.Packet);
                Worker.Instance.AddCallback(action);
            }
            else
            {
                Log.WriteLine(LogLevel.Debug, "Unhandled packet {0} Data: {1}", e.Packet.OPCode,e.Packet.Dump());
            }
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
