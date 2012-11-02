using System;
using Warrock.Game.Room;
using Warrock.Game;
using Warrock.Data;
using Warrock.Game.Events;
using Warrock.Util;
using Warrock.Game.WeaponSets;

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
                        Action.PacketValue = NewGame.pRoom.MapID;
                        break;
                    case RoomMode.Deathmatch:
                        NewGame = new Game.Game.Deathmatch(pRoomPlayer.pRoom);
                        Action.Value += 3;
                        Action.PacketValue = NewGame.pRoom.MapID;
                        break;
                    case RoomMode.Explosive:
                        NewGame = new Game.Game.Explosiv(pRoomPlayer.pRoom);
                        Action.Value += 3;
                        Action.PacketValue = NewGame.pRoom.MapID;
                        break;
                    case RoomMode.FFA:
                        NewGame = new Game.Game.FFAGame(pRoomPlayer.pRoom);
                        Action.Value += 3;
                        Action.PacketValue = NewGame.pRoom.MapID;
                        break;
                    case RoomMode.ZombiDefence:
                        NewGame = new Game.Game.ZombiDefence(pRoomPlayer.pRoom);
                        Action.PacketValue = NewGame.pRoom.MapID;
                        break;
                    case RoomMode.ZombiServervival:
                        NewGame = new Game.Game.ZombiServervival(pRoomPlayer.pRoom);
                        Action.Value += 3;
                        Action.PacketValue = NewGame.pRoom.MapID;
                        break;
                    default:
                        Log.WriteLine(LogLevel.Warn, "Unkown GameMode Found {0}", pRoomPlayer.pRoom.Mode);
                        break;
                }
                pRoomPlayer.pClient.Player.PlayGame = NewGame;
   
                pRoomPlayer.pRoom.SetAllIngame();
                Action.SendToRoom(pRoomPlayer.pRoom);
                pRoomPlayer.pRoom.SendPlayerJoin(pRoomPlayer);//this send then ingame
            }

            pRoomPlayer.pRoom.RoomStatus = 2;
            if (pRoomPlayer.pRoom.AllReady() || pRoomPlayer.pClient.AccountInfo.Access_level > 0)
            {
                pRoomPlayer.pRoom.SendPlayerJoin(pRoomPlayer);//this send then ingame
            }
            Warrock.RoomManager.Instance.UpdatePageByID(pRoomPlayer.pClient.Player.PlayerSeeRoomListPage, pRoomPlayer.pClient.Player.ChannelID);
        }
        [RoomEvent(RoomActionType.SpawnPlayer)]
        public static void SpawnPlayer(RoomPlayer pRoomPlayer, RoomAction Action)
        {
           WeaponSet CurrSet; 
          if (!pRoomPlayer.pClient.Player.pInventory.WeaponsSets.TryGetValue((WeaponSetType)1,out CurrSet))
          {
              PacketHelper.SendMessage(pRoomPlayer.pClient, "Can not get Weapon you Weapon Set");
              return;
          }
            Data.Weapon weapon;
            //if(!ItemDataProvider.Instance.GetWeaponByCode(out weapon,pRoomPlayer.CurrentPlayerWeaponSet.Slots[1].WeaponString)) return;

          pRoomPlayer.CurrentPlayerWeaponSet = CurrSet;
      //    pRoomPlayer.CurrentWeapon = weapon;
            Action.SendToRoom(pRoomPlayer.pRoom);
            pRoomPlayer.isReadyToSpawn = false;
        }
        [RoomEvent(RoomActionType.ChangeWeapon)]
        public static void ChangePlayerWeapon(RoomPlayer pRoomPlayer, RoomAction Action)
        {
            if (Action.PacketValue2 < 8)
            {
                return;
            }
            //RoomPlayer.CurrentWeapon = pRoomPlayer.CurrentPlayerWeaponSet.Slots[(byte)Action.PacketValue2];//slot

//            Console.WriteLine(pRoomPlayer.CurrentWeapon.WeaponString);
            Console.WriteLine(Action.Value);//weaponid
        }
        [RoomEvent(RoomActionType.OpenSpawn)]
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
