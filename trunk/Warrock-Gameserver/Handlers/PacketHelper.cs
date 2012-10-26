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
        public static void SendItemShopResultSuccess(GameClient pClient,Game.Item.item Pitem)
        {
            using(var pack = new WRPacket((int)GameServerOpcodes.ItemShopResult))
            {
                pack.addBlock(1);
                pack.addBlock(1110);
                pack.addBlock(-1);
                pack.addBlock(3);
                pack.addBlock(4);
                pack.addBlock(Pitem.genItemString());
                pack.addBlock(pClient.Player.Dinar);
                pack.addBlock(pClient.Player.pInventory.getOpenSlots()); //Slots Enabled
                pClient.SendPacket(pack);
            }
        }
        public static void SendItemShopError(GameClient pClient, Data.ItemShopErr Error)
        {
            using (var pack = new WRPacket((int)GameServerOpcodes.ItemShopResult))
            {
                pack.addBlock((int)Error);
                pack.addBlock(1110);
                pClient.SendPacket(pack);
            }
        }
        public static void EquipmentItem(GameClient pClient, byte Class, string Weaponstring)
        {
            using (var pack = new WRPacket((int)29970))
            {
                pack.addBlock(1);
                pack.addBlock(Class);
                pack.addBlock(Weaponstring);
                pClient.SendPacket(pack);
            }

        }
        public static void WriteChatMessage(WRPacket pack,string Message,ChatType Type,string pSenderNick,int SeasonID,int TargetID,string TargetNick = "NULL")
        {
                pack.addBlock(1);
                pack.addBlock(SeasonID);
                pack.addBlock(pSenderNick);
                pack.addBlock((int)Type);
                pack.addBlock(TargetID);
                pack.addBlock(TargetNick);
                pack.addBlock(Message);
        }
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
        #region Room
        public static void SendCreateRoomSucces(Player pPlayer)
        {
            using (var pack = new WRPacket((int)GameServerOpcodes.CreateRoomSuccess))
            {
                pack.addBlock(0);
                pack.addBlock(0);
                pPlayer.pRoom.WriteInfo(pack);
                pPlayer.pClient.SendPacket(pack);
            }
        }
  
        public static void SendChangeRoomDataIngame(int tType,params string[] Data)
        {
            using (WRPacket p = new WRPacket((int)3000))
            {
                p.addBlock(1);
                for (int i = 0; i < Data.Length; i++)
                {
                    p.addBlock(Data[i]);

                    if (i == 2 && tType != 1000)
                    {
                        p.addBlock(tType);
                    }
                }
            }
        }
        
        #endregion
    }
}
