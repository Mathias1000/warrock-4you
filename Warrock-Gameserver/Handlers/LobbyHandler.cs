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
using Warrock.Game;
using Warrock.Data;
using Warrock.Game.Room;

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
        [PacketHandler((int)ClientGameOpcode.RoomList_Request)]
        #region Room
        public static void RoomListRequest(GameClient pClient, WRPacket pPacket)
        {

            int PageID = pPacket.ReadInt(2);
            using (var pp = new WRPacket((int)GameServerOpcodes.Room_List))
            {
                int pageCount = RoomManager.Instance.ServerRooms.Count /RoomManager.Instance.MaxRoomPages+1;
                pClient.Player.PlayerSeeRoomListPage = PageID;
                pp.addBlock(RoomManager.Instance.ServerRooms.Count);
                pp.addBlock(PageID);
                pp.addBlock(pageCount);//pagecount??
                for (int i = 15 * PageID, j = 0; j < 15 && i < RoomManager.Instance.ServerRooms.Count; ++i, ++j)//thanks crommon
                {
                    if (RoomManager.Instance.ServerRooms.ContainsKey(i))
                    {
                        RoomManager.Instance.ServerRooms[i].WriteInfo(pp); ;
                    }
                }
                pClient.SendPacket(pp);
            }
        }
        [PacketHandler((int)ClientGameOpcode.Create_Room)]
        public static void CreateRoom(GameClient pClient, WRPacket pPacket)
        {
            if (pClient.Player.pRoom == null)
            {
                int pRoomSlot;
                if (!RoomManager.Instance.GetEmptyRoomSlot(out pRoomSlot))
                {
                    PacketHelper.SendMessage(pClient, "The Server Has Maxium of RoomPlease Select OpenRoom");
                }
                int MaxP = 4;
                RoomPlayer pMaster = new RoomPlayer
                {
                    pClient = pClient,
                    UserID = pClient.Player.UserID,
                    isReady = false,
                    isMaster = true,
                    RoomSlot = 0,
                    Team = TeamType.DERBAN,
                };
                pClient.Player.IsInLobby = false;
                if (pClient.Player.ChannelID != 4)
                {
                    MaxP = 8 * (pPacket.ReadByte(5)+1);
                }
                int room = pPacket.ReadByte(6);
                PlayerRoom NewRomm = new PlayerRoom
                {
                    RoomName = pPacket.ReadString(2),
                    isPrivate = Convert.ToBoolean(pPacket.ReadByte(3)),
                    RoomPassword = pPacket.ReadString(4),
                    pGameType = (Data.GameType)pPacket.ReadByte(6),
                    MapID = pPacket.ReadInt(7),
                    MaxPlayers = (byte)MaxP,
                    ChannelID = pClient.Player.ChannelID,
                    RoomType = (Data.RoomType)pPacket.ReadByte(9),
                    PremiumOnly = pPacket.ReadByte(11),
                    VoteKick = Convert.ToBoolean(pPacket.ReadByte(12)),
                    RoomMaster = pMaster,
                 
                };
                NewRomm.TeamDEBERAN.Add(pMaster.RoomSlot, pMaster);
                NewRomm.SetMinAndMaxLevel(pPacket.ReadByte(10));
                pMaster.pRoom = NewRomm;
                NewRomm.RoomPlayers.TryAdd(pMaster.UserID,pMaster);
                pClient.Player.pRoom = NewRomm;
                if (!RoomManager.Instance.ServerRooms.TryAdd(NewRomm.RoomID, NewRomm))
                {
                    Log.WriteLine(LogLevel.Error, "Failed Add Room {0} RoomCreater is {1}", NewRomm.RoomID,pClient.Player.NickName);
                    NewRomm = null;
                    pClient.Player.pRoom = null;
                    return;
                }
                PacketHelper.SendCreateRoomSucces(pClient.Player);
                pClient.Player.pRoom.SendPlayerUpdate();
                Log.WriteLine(LogLevel.Debug, "Create Room {0}", NewRomm.RoomName);
            }
        }
        [PacketHandler((int)ClientGameOpcode.Leave_Room)]
        public static void LeaveRoom(GameClient pClient, WRPacket pPacket)
        {
             RoomPlayer RemoveP;
            if (pClient.Player.pRoom == null) { return; }
            if(!pClient.Player.pRoom.RoomPlayers.TryGetValue(pClient.Player.UserID,out RemoveP)){return;}
            if (pClient.Player.pRoom.RoomPlayers.Count <= 1)
            {
                pClient.Player.pRoom.SendResetSlotRoom(RemoveP);
                pClient.Player.pRoom.Remove();
                RoomManager.Instance.UpdatePageByID(pClient.Player.PlayerSeeRoomListPage, pClient.Player.ChannelID);
            }
            else if (pClient.Player.pRoom.RoomStatus == 2)//isplaying
            {
                if (pClient.Player.pRoom.TeamDEBERAN.Count == 2 && pClient.Player.pRoom.TeamNIU.Count == 1 || pClient.Player.pRoom.TeamNIU.Count == 2 && pClient.Player.pRoom.TeamDEBERAN.Count == 1)
                {
                    //todo end of game
                    RoomManager.Instance.UpdatePageByID(pClient.Player.PlayerSeeRoomListPage, pClient.Player.ChannelID);
                }
            }
            else if (pClient.Player.pRoom.RoomMaster.pClient == pClient)
            {
                RoomPlayer oldMaster = pClient.Player.pRoom.RoomMaster;
                if (pClient.Player.pRoom.SwitchMaster())
                {
                    pClient.Player.pRoom.SendResetSlotRoom(oldMaster);
                    pClient.Player.pRoom.SendPlayerUpdate();
                    oldMaster = null;
                }
                else
                {
                    pClient.Player.pRoom.Remove();
                }

                pClient.Player.pRoom = null;
                RoomManager.Instance.UpdatePageByID(pClient.Player.PlayerSeeRoomListPage, pClient.Player.ChannelID);
            }
            else
            {
                pClient.Player.pRoom.RoomPlayers.TryGetValue(pClient.Player.UserID, out RemoveP);
                pClient.Player.pRoom.RemovePlayer(pClient.Player.UserID);
                pClient.Player.pRoom.SendResetSlotRoom(RemoveP);
                pClient.Player.pRoom.SendPlayerUpdate();
                pClient.Player.pRoom = null;
                RoomManager.Instance.UpdatePageByID(pClient.Player.PlayerSeeRoomListPage, pClient.Player.ChannelID);
            }
        }
        public static void SendJoinRoomFailed(GameClient pClient, RoomErrCode pCode)
        {
            using (var pack = new WRPacket((int)GameServerOpcodes.PlayerJoinRoom))
            {
                pack.addBlock((int)pCode);
                pClient.SendPacket(pack);
            }
        }
        [PacketHandler((int)ClientGameOpcode.Join_Room)]
        public static void JoinRoom(GameClient pClient, WRPacket pPacket)
        {
            byte roomID = pPacket.ReadByte(2);
            string Password = pPacket.ReadString(3);
           PlayerRoom Room;       
           if (!RoomManager.Instance.ServerRooms.TryGetValue(roomID, out Room)) { return; }

           RoomPlayer JoinedPlayer = new RoomPlayer
           {
               pClient = pClient,
               pRoom = Room,
               isMaster = false,
               isLiving = true,
               chooseClass = "1",
                Life = 1000,
               UserID = pClient.Player.UserID,
           };
           if (Room.RoomPassword != Password)
           {
               SendJoinRoomFailed(pClient, RoomErrCode.InvalidPassword);
           }
           else if (Room.PremiumOnly == 1 && pClient.Player.Premium <= 0)
           {
               SendJoinRoomFailed(pClient, RoomErrCode.OnlyPremium);
           }
           else if (Room.MinLevel >= pClient.Player.Level && pClient.Player.Level >= Room.MaxPlayers)
           {
               SendJoinRoomFailed(pClient, RoomErrCode.BadLevel);
           }
           else//premium only
           {
               if (pClient.Player.Ping >= 100 && Room.RoomPing == PingLimits.Green)
               {
                   PacketHelper.SendMessage(pClient, "Your Ping is over the Ping Limit!");
               }
               else if (pClient.Player.Ping > 300 && Room.RoomPing == PingLimits.Yellow)
               {
                   PacketHelper.SendMessage(pClient, "Your Ping is over the Ping Limit!");

               }
               else
               {
                   if (Room.pPlayerJoIn(JoinedPlayer))
                   {
                       Room.SendPlayerUpdate();
                       RoomManager.Instance.UpdatePageByID(pClient.Player.PlayerSeeRoomListPage, pClient.Player.ChannelID);
                   }
                   else
                   {
                       JoinedPlayer = null;
                       SendJoinRoomFailed(pClient, RoomErrCode.GenericError);
                   }
               }
           }

        }
        public static void ChangePacket(GameClient pClient, string[] pack)
        {

        }
        [PacketHandler((int)ClientGameOpcode.ChangeRoomData)]
        public static void ChangeRoomData(GameClient pClient, WRPacket pPacket)
        {
            if (pClient.Player.pRoom == null) { pClient.Disconnect(); return; }
            RoomPlayer RoomPlayer;
            RoomAction ResponseAction = null;
            if (!pClient.Player.pRoom.RoomPlayers.TryGetValue(pClient.Player.UserID, out RoomPlayer))
            {
                return;
            }


                Console.WriteLine(pPacket.Dump());
                RoomActionType PacketActionType = (RoomActionType)pPacket.ReadUShort(5);
                
                if (pPacket.ReadByte(2) != RoomPlayer.RoomSlot)//secruty
                {
                    PacketHelper.SendMessage(RoomPlayer.pClient, "Ilegal Action!");
                    return;
                }
                ushort value = pPacket.ReadUShort(8);
                ushort masterValue = pPacket.ReadUShort(11);
                ushort PacketValue = value;
                ushort PacketValue2 = pPacket.ReadUShort(9);
                string[] AllBlock = pPacket.getAllBlocks();
                #region PacketType Call
                switch (PacketActionType)
                {
                    case RoomActionType.InviteIntoGame:
                        if (RoomPlayer.isMaster && RoomPlayer.pRoom.AllReady() || RoomPlayer.pClient.Player.Acces_level > 0)
                        {
                            switch (RoomPlayer.pRoom.Mode)
                            {
                                case RoomMode.Conquest:
                                    new Game.Game.Conquest(RoomPlayer.pRoom);
                                    break;
                                case RoomMode.Deathmatch:
                                    new Game.Game.Deathmatch(RoomPlayer.pRoom);
                                    break;
                                case RoomMode.Explosive:
                                    new Game.Game.Explosiv(RoomPlayer.pRoom);
                                    break;
                                case RoomMode.FFA:
                                    new Game.Game.FFAGame(RoomPlayer.pRoom);
                                    break;
                            }
                        }
                        break;
                    case RoomActionType.ChangeRdy:
                        if (RoomPlayer.isReady)
                        {
                            RoomPlayer.isReady = true;
                        }
                        else
                        {
                            RoomPlayer.isReady = false;
                        }
                        ResponseAction = new RoomAction
                        {
                            Action = RoomActionType.ChangeRdy,
                            PacketValue2 = PacketValue2,
                            PacketValue = PacketValue,
                            Value = value,
                            MasterValue = masterValue,

                        };
                        //todo update in room?
                        break;
                    case RoomActionType.ChangeMapID:
                       RoomPlayer.pRoom.MapID = value;
                       ResponseAction = new RoomAction
                       {
                           Action = RoomActionType.ChangeMapID,
                           PacketValue2 = PacketValue2,
                           PacketValue = PacketValue,
                           Value = value,
                           MasterValue = masterValue,

                       };
                        break;
                    case RoomActionType.ChangeRoomMode:
                        pClient.Player.pRoom.Mode = (Data.RoomMode)value;
                        if (RoomPlayer.pClient.Player.pRoom.Mode == RoomMode.Deathmatch)
                        {
                            RoomPlayer.pClient.Player.pRoom.SetRealRound();
                        }
                        ResponseAction = new RoomAction
                        {
                            Action = RoomActionType.ChangeRoomMode,
                            PacketValue2 = PacketValue2,
                            PacketValue = PacketValue,
                            Value = value,
                            MasterValue = masterValue,

                        };
                        break;
                    case RoomActionType.ChangeRoomRounds:
                        RoomPlayer.pClient.Player.pRoom.Rounds = value;
                        if (RoomPlayer.pClient.Player.pRoom.Mode == RoomMode.Deathmatch)
                        {
                            RoomPlayer.pClient.Player.pRoom.SetRealRound();
                        }
                        ResponseAction = new RoomAction
                        {
                            Action = RoomActionType.ChangeRoomRounds,
                            PacketValue2 = PacketValue2,
                            PacketValue = PacketValue,
                            Value = value,
                            MasterValue = masterValue,

                        };
                        break;
                    case RoomActionType.ChangeRoomTimeLeft:
                        RoomPlayer.pClient.Player.pRoom.RoomTimeLeft = value * 10;
                        ResponseAction = new RoomAction
                        {
                            Action = RoomActionType.ChangeRoomTimeLeft,
                            PacketValue2 = PacketValue2,
                            PacketValue = PacketValue,
                            Value = value,
                            MasterValue = masterValue,

                        };
                        break;
                    case RoomActionType.ChangeKillLimit:
                        RoomPlayer.pRoom.SetKillLimit(value);
                        break;
                    case RoomActionType.ChangeRoomSlot:
                        byte OldSlot = RoomPlayer.RoomSlot;
                        if (RoomManager.Instance.switchTeam(RoomPlayer))
                        {
                            if (RoomPlayer.isMaster)
                            {
                                if (!RoomPlayer.pRoom.SwitchMaster())
                                {
                                    if (RoomPlayer.pRoom.RoomMaster.Team == TeamType.DERBAN)
                                    {
                                        RoomPlayer.pRoom.RoomMaster.RoomSlot = RoomPlayer.RoomSlot;
                                        RoomPlayer.pRoom.RoomMaster.Team = TeamType.DERBAN;
                                        RoomPlayer.pRoom.MovePlayer(RoomPlayer, OldSlot);
                                       RoomPlayer.pRoom.SendPlayerUpdate();
                                    }
                                    else if (RoomPlayer.pRoom.RoomMaster.Team == TeamType.NIU)
                                    {
                                        RoomPlayer.pRoom.RoomMaster.RoomSlot = RoomPlayer.RoomSlot;
                                        RoomPlayer.pRoom.RoomMaster.Team = TeamType.NIU;
                                        RoomPlayer.pRoom.MovePlayer(RoomPlayer, OldSlot);
                                        RoomPlayer.pRoom.SendPlayerUpdate();
                                    }
                                }
                            }
                        }
                        break;
                    case RoomActionType.ChangeRoomPing:
                        RoomPlayer.pClient.Player.pRoom.RoomPing = (Data.PingLimits)value;
                        ResponseAction = new RoomAction
                        {
                            Action = RoomActionType.ChangeRoomPing,
                            PacketValue2 = PacketValue2,
                            PacketValue = PacketValue,
                            Value = value,
                            MasterValue = masterValue,

                        };
                        break;
                    case RoomActionType.ChangeRoomAutoStart:
                        RoomPlayer.pClient.Player.pRoom.AutoStart = (byte)value;
                        ResponseAction = new RoomAction
                        {
                            Action = RoomActionType.ChangeRoomAutoStart,
                            PacketValue2 = PacketValue2,
                            PacketValue = PacketValue,
                            Value = value,
                            MasterValue = masterValue,

                        };
                        break;
                    default:
                        Log.WriteLine(LogLevel.Warn, "Unkown ActionType {0} for packet 3000", PacketActionType);
                        break;
                }
                #endregion
                if (ResponseAction != null)
                {
                    using(var pack = new WRPacket((int)GameServerOpcodes.RoomAtion_Response))
                    {
                        ResponseAction.WriteInfo(pack);
                      
                        RoomPlayer.pRoom.SendPacketToAllRoomPlayers(pack);
                    }
                    ResponseAction = null;//reset
                }
        }
        #endregion
    }
}
