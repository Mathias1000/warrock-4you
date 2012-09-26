using System;
using System.Collections.Generic;
using Warrock_InterLib.Networking;
using Warrock.InterServer;
using Warrock.Util;

namespace Warrock.InterServer
{
	public sealed class InterHandler
	{
        [InterPacketHandler(InterHeader.Assigned)]
        public static void HandleAssigned(LoginConnector lc, InterPacket packet)
        {
            Log.WriteLine(LogLevel.Info, "GameServer is Ready");
            //todo Start another stuff
        }
        public static void TryAssiging(LoginConnector lc)
        {
            using (var p = new InterPacket(InterHeader.Assign))
            {
                p.WriteByte(Config.Instance.GameServerID);//id
                p.WriteString(Config.Instance.LoginServerIP,12);//ip
                p.WriteUShort(Config.Instance.LoginInterServerPort);//port
                lc.SendPacket(p);
            }
        }
	}
}
