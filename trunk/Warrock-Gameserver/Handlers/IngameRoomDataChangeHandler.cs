using System;
using Warrock.Game.Room;
using Warrock.Game;
using Warrock.Data;
using Warrock.Game.Events;
using Warrock.Util;

namespace Warrock.Handlers
{
    public class IngameRoomDataChangeHandler
    {
        [RoomEvent(RoomActionType.InviteIntoGame)]
        public static void StartGame(RoomPlayer pRoomPlayer, RoomAction Action)
        {
            if (pRoomPlayer.isMaster && pRoomPlayer.pRoom.AllReady() || pRoomPlayer.pClient.Player.Acces_level > 0)
            {
                Game.Game.Game NewGame = null;
                switch (pRoomPlayer.pRoom.Mode)
                {
                    case RoomMode.Conquest:
                        NewGame = new Game.Game.Conquest(pRoomPlayer.pRoom);
                        Action.Value += 3;
                        break;
                    case RoomMode.Deathmatch:
                        NewGame = new Game.Game.Deathmatch(pRoomPlayer.pRoom);
                        Action.Value += 3;
                        break;
                    case RoomMode.Explosive:
                        NewGame = new Game.Game.Explosiv(pRoomPlayer.pRoom);
                        Action.Value += 3;
                        break;
                    case RoomMode.FFA:
                        NewGame = new Game.Game.FFAGame(pRoomPlayer.pRoom);
                        Action.Value += 3;
                        break;
                    case RoomMode.ZombiDefence:
                        NewGame = new Game.Game.ZombiDefence(pRoomPlayer.pRoom);
                        break;
                    case RoomMode.ZombiServervival:
                        NewGame = new Game.Game.ZombiServervival(pRoomPlayer.pRoom);
                        Action.Value += 3;
                        break;
                    default:
                        Log.WriteLine(LogLevel.Warn, "Unkown GameMode Found {0}", pRoomPlayer.pRoom.Mode);
                        break;
                }
                pRoomPlayer.pClient.Player.PlayGame = NewGame;
                Action.SendToRoom(pRoomPlayer.pRoom);
                pRoomPlayer.pRoom.SetAllIngame();
                pRoomPlayer.pRoom.SendPlayerJoin(pRoomPlayer);//this send then ingame
            }

            pRoomPlayer.pRoom.RoomStatus = 2;
            Warrock.RoomManager.Instance.UpdatePageByID(pRoomPlayer.pClient.Player.PlayerSeeRoomListPage, pRoomPlayer.pClient.Player.ChannelID);
        }
        
        [RoomEvent(RoomActionType.SpawnRequest)]
        public static void SpawnRequest(RoomPlayer pRoomPlayer, RoomAction Action)
        {
            pRoomPlayer.isReadyToSpawn = true;
            if (pRoomPlayer.pRoom.AllReadyToSpawn())
            {
                if (pRoomPlayer.pClient.Player.PlayGame != null)
                {
                    Action = pRoomPlayer.pClient.Player.PlayGame.OpenSpawn();
                }
                Action.SendToRoom(pRoomPlayer.pRoom);
            }
            else
            {
                //todo not Ready
            }
        }
        
    }
}
