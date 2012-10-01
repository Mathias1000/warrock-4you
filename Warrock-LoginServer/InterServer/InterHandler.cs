using System;
using Warrock_InterLib;
using Warrock.Util;
using Warrock_InterLib.Networking;
using Warrock.InterServer;

namespace Zepheus.Login.InterServer
{
    public sealed class InterHandler
    {
        public static void SendAssigned(GameConnection wc)
        {
            using (var p = new InterPacket(InterHeader.Assigned))
            {
                wc.SendPacket(p);
            }
        }
        [InterPacketHandler(InterHeader.SendOnlineUserReuqest)]
        public static void SetOnlineUsers(GameConnection wc, InterPacket packet)
        {
            int Count;
            if(!packet.TryReadInt(out Count))
            {
                return;
            }
            wc.OnlineUsers = Count;
        }
        [InterPacketHandler(InterHeader.Assign)]
        public static void HandleServerAssignement(GameConnection wc, InterPacket packet)
        {
            byte ID;
            string IP;
            ushort port;
            int PlayerLimit;
            string ServerName;
            if (!packet.TryReadByte(out ID) ||!packet.TryReadString(out IP,16) ||!packet.TryReadUShort(out port)||!packet.TryReadInt(out PlayerLimit) || !packet.TryReadString(out ServerName,16))
            {
                return;
            }
            wc.ID = ID;
            wc.IP = IP;
            wc.Port = port;
            wc.PlayerLimit = PlayerLimit;
            wc.ServerName = ServerName;
            if (Warrock_LoginServer.Managers.GameServerManager.Instance.GameServers.ContainsKey(ID))
            {
                Log.WriteLine(LogLevel.Error, "Already loaded this world?");
                wc.Disconnect();
                return;
            }
            if (Warrock_LoginServer.Managers.GameServerManager.Instance.GameServers.TryAdd(wc.ID, wc))
            {
                Log.WriteLine(LogLevel.Info, "Assigned GameServer {0}!", wc.ID);
                SendAssigned(wc);
            }
            else
            {
                Log.WriteLine(LogLevel.Error, "Couldn't assign world {0}..", wc.ID);
            }
        }
    }
}
