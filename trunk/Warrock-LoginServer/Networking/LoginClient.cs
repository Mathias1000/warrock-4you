using System;
using System.Net.Sockets;
using System.Reflection;
using Warrock.Util;
using Warrock_Lib;
using Warrock_LoginServer.Handlers;
using Warrock_Lib.Networking;

namespace Warrock_LoginServer.Networking
{
    public sealed class LoginClient : Client
    {
        public bool IsAuthenticated { get; set; }
        public int AccountID { get; set; }
        public string Username { get; set; }
        public byte Admin { get; set; }

        public LoginClient(Socket sock)
            : base(sock)
        {
            base.OnPacket += new EventHandler<PacketReceivedEventArgs>(LoginClient_OnPacket);
            base.OnDisconnect += new EventHandler<SessionCloseEventArgs>(LoginClient_OnDisconnect);
            SendSPConnect(sock);
        }

        void SendSPConnect(Socket SPSock)
        {
            WRPacket p = new WRPacket(4608);
            p.addBlock(new Random().Next(111111111, 999999999));
            p.addBlock(77);
            byte[] rPacket = p.getPacket();
           SPSock.Send(rPacket, 0, rPacket.Length, SocketFlags.None);
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
                Log.WriteLine(LogLevel.Debug, "Unhandled packet: {0}", e.Packet.OPCode);
            }
        }
    }
}
