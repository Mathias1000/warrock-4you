using System;
using System.Collections.Generic;
using Warrock_InterLib.Networking;
using Warrock.InterServer;
using Warrock.Util;
using Warrock.Networking;

namespace Warrock.InterServer
{
	public sealed class InterHandler
	{
        [InterPacketHandler(InterHeader.Assigned)]
        public static void HandleAssigned(LoginConnector lc, InterPacket packet)
        {
            GameAcceptor.Load();
            Program.sockUdpServers.SetupUDPServer();
            Worker.Instance = new Worker();
            Log.WriteLine(LogLevel.Info, "GameServer is Ready");
            //todo Start another stuff
        }
        [InterPacketHandler(InterHeader.SendOnlineUserReuqest)]
        public static void OnlineUSers(LoginConnector lc, InterPacket packet)
        {
          using(var pack = new InterPacket(InterHeader.SendOnlineUserReuqest))
          {
              pack.WriteInt(ClientManager.Instance.GetClientCount());
      
              lc.SendPacket(pack);
          }
        }
        public static void TryAssiging(LoginConnector lc)
        {
            using (var p = new InterPacket(InterHeader.Assign))
            {
                p.WriteByte(Config.Instance.GameServerID);//id
                p.WriteString(Config.Instance.GameServerPublicIP,16);//ip
                p.WriteUShort((ushort)Config.Instance.GameServerPort);//port
                p.WriteInt(Config.Instance.PlayerLimit);
                p.WriteString(Config.Instance.ServerName, 16);
                lc.SendPacket(p);
            }
        }
	}
}
