using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Warrock.Util;
using Warrock.Lib.Networking;
using Warrock.Networking;
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
            using (var pp = new WRPacket((int)GameServerOpcodes.Welcome))
            {
                pp.addBlock(1);
                pp.addBlock(DateTime.Now.ToString(@"ss\/mm\/HH\/dd\/MM\/110") + "/3/356/0");
                pClient.SendPacket(pp);
            }
        }
        [PacketHandler((int)ClientGameOpcode.UserListRequest)]
        public static void UserList(GameClient pClient, WRPacket pPacket)
        {
            List<GameClient> UserList = new List<GameClient>();
            int Operation = pPacket.ReadInt(2);
            switch (Operation)
            {
                case 0://ALL
                    UserList = PlayerManager.Instance.getAllplayers();
                    break;
                case 1:// byname
                    string ScreachName = pPacket.ReadString(3);
                    UserList = PlayerManager.Instance.getAllplayers().FindAll(p => p.Player.AccountInfo.NickName == ScreachName);
                    break;
                case 2: //byClan
                    break;
                case 3://by level
                    string[] RangeS = pPacket.ReadString(3).Split('-');
                    byte MinRange = byte.Parse(RangeS[0]);
                    byte MaxRange = 0;
                    if (RangeS.Length == 2)
                    {
                        MaxRange = byte.Parse(RangeS[1]);
                    }
                    else
                    {
                        MaxRange = MinRange;
                    }
                    UserList = PlayerManager.Instance.getAllplayers().FindAll(p => p.Player.Level >= MinRange && p.Player.Level <= MaxRange);
                    break;
                default:
                    Log.WriteLine(LogLevel.Warn, "Unkown UserList Operator");
                    break;

            }
            using (var pack = new WRPacket((int)GameServerOpcodes.SendUserList))
            {
                PacketHelper.WriteUserList(UserList, pack);
                pClient.SendPacket(pack);
            }
        }
        

    }
}
