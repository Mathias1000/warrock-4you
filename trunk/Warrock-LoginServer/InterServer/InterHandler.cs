using System;
using Warrock_InterLib;
using Warrock.Util;
using Warrock_InterLib.Networking;
using Warrock.InterServer;

namespace Zepheus.Login.InterServer
{
    public sealed class InterHandler
    {
        [InterPacketHandler(InterHeader.Assigned)]
        public static void HandleAssigned(GameConnection lc, InterPacket packet)
        {
   
            Console.WriteLine("auth ok");
        }
        public static void SendAssigned(GameConnection wc)
        {
            using (var p = new InterPacket(InterHeader.Assigned))
            {
                wc.SendPacket(p);
            }
        }
        [InterPacketHandler(InterHeader.Assign)]
        public static void HandleServerAssignement(GameConnection wc, InterPacket packet)
        {
            byte ID;
            string IP;
            ushort port;
            if (!packet.TryReadByte(out ID) ||!packet.TryReadString(out IP,12) ||!packet.TryReadUShort(out port))
            {
                return;
            }
            wc.ID = ID;
            wc.IP = IP;
            wc.Port = port;
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
