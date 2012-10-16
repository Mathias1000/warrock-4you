using System;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using Warrock.Lib;
using Warrock.Lib.Networking;
using Warrock.Util;
using Warrock.Game.Room;
using Warrock.Handlers;
using Warrock.Game;

namespace Warrock.Networking
{
    public  class GameClient : Client
    {
        public bool Authenticated { get; set; }
        public tUser AccountInfo { get; set; }
        public Player Player { get; set; }
        public int uniqIDisCRC = 910;
        public int uniqID { get; private set; }//this stuff generatet by Gamelogin
        public int uniqID2 { get; private set; }

        public IPEndPoint RemoteEndPoint { get; set; }
        public IPEndPoint LocalEndPoint { get; set; }


        public long nIP { get; set; }
        public int nPort { get; set; }
        public long lIP { get; set; }
        public int lPort { get; set; }

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
        #region peer to peer
        public void setRemoteEndPoint(IPEndPoint Target)
        {
            nIP = IPToInt(Target.Address.ToString());
            nPort = PortToInt(Target.Port);
           this.RemoteEndPoint = Target;
        }
        public void setLocalEndPoint(IPEndPoint Target)
        {
            lIP = IPToInt(Target.Address.ToString());
            lPort = PortToInt(Target.Port);
            this.LocalEndPoint  = Target;
        }
        public int PortToInt(int Port)
        {
            byte[] PortBytes = BitConverter.GetBytes(Port);
            byte[] PortBytesNew = new byte[2] { PortBytes[1], PortBytes[0] };
            ushort newPort = BitConverter.ToUInt16(PortBytesNew, 0);

            return newPort;
        }
        public string ReverseIP(string tString)
        {
            string[] bString = tString.Split(new char[] { '.' });
            string tNew = "";
            for (int i = (bString.Length - 1); i > -1; i--)
            { tNew += bString[i] + "."; }
            return tNew.Substring(0, tNew.Length - 1);
        }

        public long IPToInt(string addr)
        {
            return (long)(uint)IPAddress.NetworkToHostOrder((int)IPAddress.Parse(ReverseIP(addr)).Address);
        }
        public void SetUpNetwork(IPEndPoint GroupEP, byte UDPId)
        {
            nIP = IPToInt(GroupEP.Address.ToString());
            nPort = PortToInt(GroupEP.Port);
            Log.WriteLine(LogLevel.Debug, "Setting Up Network! UDPID: ", UDPId);
           // Log.WriteLine(LogLevel.Debug ,"Setting Up Network! UDPID: ", UDPId, " (nIP: ", this.lNetworkIP, ":", this.iNetworkPort, " lIP: ", this.lLocalIP, ":", this.iLocalPort }));
        }
        #endregion
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
