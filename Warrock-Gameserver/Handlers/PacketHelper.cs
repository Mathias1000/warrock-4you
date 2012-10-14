using System.Collections.Concurrent;
using System.Collections.Generic;
using Warrock.Game;
using Warrock.Data;
using Warrock.Networking;
using Warrock.Lib.Networking;
using Warrock.Lib;

namespace Warrock.Handlers
{
    public class PacketHelper
    {
        public static void WriteUserList(List<GameClient> UserList, WRPacket pack)
        {
            pack.addBlock(UserList.Count);
            foreach (GameClient pClient in UserList)
            {
                pack.addBlock(pClient.Player.UserID);
                pack.addBlock(pClient.Player.NickName);
                pack.addBlock(0);//premium

                pack.addBlock(5);// 7= clan

                if (pClient.Player.pInventory.hasPX("CK01"))
                {
                    pack.addBlock(-1);
                }
                else
                {
                    pack.addBlock(WRLevel.getLevelforExp(pClient.Player.Experience));
                }
                pack.addBlock(pClient.Player.ChannelID);
                pack.addBlock(-1);//RooID//-1 lobby 
                pack.addBlock(-1);
            }
        }
        public static void SendPlayerInfo(GameClient pClient)
        {
            using(var pack = new WRPacket((int)GameServerOpcodes.PlayerInfo))
            {
                pClient.Player.WritePlayerInfo(pack);
                pClient.SendPacket(pack);
            }
        }
        public static void SendPacketToAllOnlinePlayer(WRPacket pPacket)
        {
            List<GameClient> AllUsers = ClientManager.Instance.GetAllClients();
            foreach (GameClient pClient in AllUsers)
            {
                pClient.SendPacket(pPacket);
            }
        }
        public static void SendMessage(GameClient pClient, string Message)
        {
            using(var pack = new WRPacket((int)GameServerOpcodes.Show_Message))
            {
                pack.addBlock(4);//unk
                pack.addBlock(0);
                pack.addBlock(Message);
                pClient.SendPacket(pack);
            }
        }
        public static void SendChannelChange(GameClient pClient)
        {
            using (var pack = new WRPacket((int)GameServerOpcodes.ChannelChange))
            {
                pack.addBlock(1);//bool?
                pack.addBlock(pClient.Player.ChannelID);
                pClient.SendPacket(pack);
            }
        }
    }
}
