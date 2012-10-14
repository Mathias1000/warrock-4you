using System;
using Warrock.Lib;
using Warrock.Lib.Networking;
using Warrock.Networking;

namespace Warrock.Handlers
{
    public sealed class ChatHandler
    {
        [PacketHandler((int)ClientGameOpcode.ChangeChannel)]
        public static void ChangeChannel(GameClient pClient, WRPacket pPacket)
        {
            int ChanneID = pPacket.ReadInt(2);
            if (pClient.Authenticated)
            {
                pClient.Player.ChannelID = ChanneID;
                PacketHelper.SendChannelChange(pClient);
            }
        }
    }
}
