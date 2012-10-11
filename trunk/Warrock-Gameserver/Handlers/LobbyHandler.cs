using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Warrock.Util;
using Warrock_Lib.Networking;
using Warrock.Networking;
using Warrock_Lib;
using Warrock.Database;
using Warrock.Lib;

namespace Warrock.Handlers
{
    public sealed class LobbyHandler
    {
        [PacketHandler((int)ClientGameOpcode.Welcome)]
        public static void Welcome(GameClient pClient, WRPacket pPacket)
        {
            //todo add mac bann
            using (var pp = new WRPacket(24832))
            {
                pp.addBlock(1);
                pp.addBlock(DateTime.Now.ToString(@"ss\/mm\/HH\/dd\/MM\/110") + "/3/356/0");
                ClientManager.Instance.GetClientCount();
                pClient.SendPacket(pp);
                ClientManager.Instance.GetClientCount();
            }
        }
    }
}
