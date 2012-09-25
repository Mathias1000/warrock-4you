using System;
using Warrock.Util;
using Warrock_Lib.Networking;
using Warrock_LoginServer.Networking;
using Warrock_Lib;

namespace Warrock_LoginServer.Handlers
{
    public sealed class LoginHandler
    {
        [PacketHandler((int)ClientLoginOpcodes.PatchRequest)]
        public static void VersionInfo(LoginClient pClient, WRPacket pPacket)
        {
            using (var pack = new WRPacket((int)LoginServerOpcodes.SendPatchVersion))
            {
                pack.addBlock(0);
                pack.addBlock(17);
                pack.addBlock(33);
                pack.addBlock(37);
                pack.addBlock(0);
                pack.addBlock(0);
                pack.addBlock("http://patch.warrock.net/k2network/warrock/");
                pClient.SendPacket(pack);
            }
        }
    }
}
