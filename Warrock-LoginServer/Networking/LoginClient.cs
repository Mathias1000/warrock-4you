using System;
using System.Net.Sockets;
using System.Reflection;
using Warrock.Util;
using Warrock.Lib;
using Warrock_LoginServer.Handlers;
using Warrock.Lib.Networking;

namespace Warrock_LoginServer.Networking
{
    public sealed class LoginClient : Client
    {
        public bool IsAuthenticated { get; set; }
        public int AccountID { get; set; }
        public string Username { get; set; }
        public byte Admin { get; set; }
        public Warrock.Lib.tUser User { get; set; }
   
        public LoginClient(Socket sock)
            : base(sock)
        {

            base.ccType = ClientType.LoginClient;
            base.OnPacket += new EventHandler<PacketReceivedEventArgs>(LoginClient_OnPacket);
            base.OnDisconnect += new EventHandler<SessionCloseEventArgs>(LoginClient_OnDisconnect);
            SendSPConnect();
        }
        void SendSPConnect()
        {
           this.SeassonID = new Random().Next(111111111, 999999999);
            WRPacket p = new WRPacket(4608);
            p.addBlock(SeassonID);
            p.addBlock(77);
            byte[] rPacket = p.getLoginPacket();
            this.Socket.Send(rPacket, 0, rPacket.Length, SocketFlags.None);
        }
        public override void SendPacket(WRPacket Packet)
        {
            Socket.Send(Packet.getLoginPacket());
        }
        void LoginClient_OnDisconnect(object sender, SessionCloseEventArgs e)
        {
            Log.WriteLine(LogLevel.Debug, "{0} Disconnected.", base.Host);


        }

        void LoginClient_OnPacket(object sender, PacketReceivedEventArgs e)
        {
            MethodInfo method = HandlerStore.GetHandler(e.Packet.OPCode);
            if (method != null)
            {
                Action action = HandlerStore.GetCallback(method, this, e.Packet);
                Worker.Instance.AddCallback(action);
            }
            else
            {
                Log.WriteLine(LogLevel.Debug, "Unhandled packet {0} Data: {1}", e.Packet.OPCode, e.Packet.Dump());
            }
        }
    }
}
