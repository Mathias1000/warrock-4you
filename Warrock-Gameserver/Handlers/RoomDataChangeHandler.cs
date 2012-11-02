using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Warrock.Data;
using Warrock.Game.Room;
using Warrock.Game.Events;
using Warrock.Game;

namespace Warrock.Handlers
{
    public class RoomDataChangeHandler
    {
        [RoomEvent(RoomActionType.ChangeRoomAutoStart)]
        public static void ChangeRoomAutostart(RoomPlayer pRoomPlayer, RoomAction Action)
        {
            pRoomPlayer.pRoom.AutoStart = (byte)Action.Value;
            Action.SendToRoom(pRoomPlayer.pRoom);
        }
        [RoomEvent(RoomActionType.ChangeMapID)]
        public static void ChangeMapID(RoomPlayer pRoomPlayer, RoomAction Action)
        {
            pRoomPlayer.pRoom.MapID = Action.Value;
            Action.SendToRoom(pRoomPlayer.pRoom);
        }
        [RoomEvent(RoomActionType.ChangeRdy)]
        public static void ChangeReady(RoomPlayer pRoomPlayer, RoomAction Action)
        {
            if (pRoomPlayer.isReady)
            {
                pRoomPlayer.isReady = true;
                Action.Value = 1;
            }
            else
            {
                pRoomPlayer.isReady = false;
                Action.Value = 0;
            }
            Action.SendToRoom(pRoomPlayer.pRoom);
            if (pRoomPlayer.pRoom.AllReady())
            {
                pRoomPlayer.pRoom.SendPlayerUpdate(pRoomPlayer.pClient);
            }
        }
        [RoomEvent(RoomActionType.ChangeRoomPing)]
        public static void ChangePing(RoomPlayer pRoomPlayer, RoomAction Action)
        {
            pRoomPlayer.pRoom.RoomPing = (PingLimits)Action.Value;
            Action.SendToRoom(pRoomPlayer.pRoom);
        }
        [RoomEvent(RoomActionType.ChangeRoomMode)]
        public static void ChangeRoomMode(RoomPlayer pRoomPlayer, RoomAction Action)
        {
            pRoomPlayer.pRoom.Mode = (Data.RoomMode)Action.Value;
            if (pRoomPlayer.pClient.Player.pRoom.Mode == RoomMode.Deathmatch)
            {
                pRoomPlayer.pClient.Player.pRoom.SetRealRound();
            }
            Action.SendToRoom(pRoomPlayer.pRoom);
        }
        [RoomEvent(RoomActionType.ChangeRoomRounds)]
        public static void ChangeRoomRounds(RoomPlayer pRoomPlayer, RoomAction Action)
        {
            pRoomPlayer.pClient.Player.pRoom.Rounds = Action.Value;
            if (pRoomPlayer.pClient.Player.pRoom.Mode == RoomMode.Deathmatch)
            {
                pRoomPlayer.pClient.Player.pRoom.SetRealRound();
            }
            Action.SendToRoom(pRoomPlayer.pRoom);
        }
        [RoomEvent(RoomActionType.ChangeRoomTimeLeft)]
        public static void ChangeRoomTimeLeft(RoomPlayer pRoomPlayer, RoomAction Action)
        {
            pRoomPlayer.pClient.Player.pRoom.RoomTimeLeft = Action.Value * 10;
            Action.SendToRoom(pRoomPlayer.pRoom);
        }
        [RoomEvent(RoomActionType.ChangeRoomSlot)]
        public static void ChangeRoomSlot(RoomPlayer pRoomPlayer, RoomAction Action)
        {
            //change normal players buggy???

            byte OldSlot = pRoomPlayer.RoomSlot;
            if (RoomManager.Instance.switchTeam(pRoomPlayer))
            {
                if (pRoomPlayer.isMaster)
                {
                    if (!pRoomPlayer.pRoom.SwitchMaster())
                    {
                        if (pRoomPlayer.pRoom.RoomMaster.Team == TeamType.DERBAN)
                        {
                            pRoomPlayer.pRoom.RoomMaster.RoomSlot = pRoomPlayer.RoomSlot;
                            pRoomPlayer.pRoom.RoomMaster.Team = TeamType.DERBAN;
                            pRoomPlayer.pRoom.MovePlayer(pRoomPlayer, OldSlot);
                            pRoomPlayer.pRoom.SendPlayerUpdate(pRoomPlayer.pClient);
                        }
                        else if (pRoomPlayer.pRoom.RoomMaster.Team == TeamType.NIU)
                        {
                            pRoomPlayer.pRoom.RoomMaster.RoomSlot = pRoomPlayer.RoomSlot;
                            pRoomPlayer.pRoom.RoomMaster.Team = TeamType.NIU;
                            pRoomPlayer.pRoom.MovePlayer(pRoomPlayer, OldSlot);
                            pRoomPlayer.pRoom.SendPlayerUpdate(pRoomPlayer.pClient);
                        }
                    }
                }
                else
                {
                }
                Action.Value = pRoomPlayer.RoomSlot;
                Action.MasterValue = pRoomPlayer.pRoom.RoomMaster.RoomSlot;
                Action.SendToRoom(pRoomPlayer.pRoom);

            }
        }

    }
}
