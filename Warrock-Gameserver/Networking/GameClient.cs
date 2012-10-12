using System;
using System.Net.Sockets;
using System.Reflection;
using Warrock_Lib;
using Warrock_Lib.Networking;
using Warrock.Util;
using Warrock.Game;
using Warrock.Handlers;
using Warrock.Lib;

namespace Warrock.Networking
{
    public  class GameClient : Client
    {
        public bool Authenticated { get; set; }
        public tUser AccountInfo { get; set; }
        public Player Player { get; set; }
        public Warrock.Lib.tUser User { get; set; }
        public int uniqIDisCRC = 910;
        public int uniqID { get; private set; }//this stuff generatet by Gamelogin
        public int uniqID2 { get; private set; }

        public GameClient(Socket socket)
            : base(socket)
        {

            base.ccType = ClientType.GameClient;
            base.OnDisconnect += new EventHandler<SessionCloseEventArgs>(GameClient_OnDisconnect);
            base.OnPacket += new EventHandler<PacketReceivedEventArgs>(GameClient_OnPacket);
            Authenticated = false;
           
        }
        public override void SendPacket(WRPacket Packet)
        {
            this.Socket.Send(Packet.getGamePacket());
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
            if (Player.NickName != null)
            {
                return "GameClient|Player:" + Player.NickName;
            }
            else
            {
                return "GameClient|NoPlayer";
            }
        }
    }
}
