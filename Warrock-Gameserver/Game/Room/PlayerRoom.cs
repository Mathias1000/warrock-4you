using System;
using Warrock.Lib;
using Warrock.Networking;
using Warrock.Lib.Networking;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Warrock.Data;
using System.Linq;
using Warrock.Util;
using System.Threading;

namespace Warrock.Game
{
    public class PlayerRoom
    {
        public readonly ConcurrentDictionary<int, RoomPlayer> RoomPlayers = new ConcurrentDictionary<int, RoomPlayer>();
        public Dictionary<int, RoomPlayer> TeamDEBERAN = new Dictionary<int, RoomPlayer>();
        public Dictionary<int, RoomPlayer> TeamNIU = new Dictionary<int, RoomPlayer>();

        public RoomPlayer RoomMaster { get; set; }
        public bool GameActive { get; set; }
        public int RoomID { get; set; }
        public byte MaxPlayers { get; set; }
        public int MapID { get; set; }
        public byte RoomStatus { get; set; }
        public string RoomName { get; set; }
        public RoomMode Mode { get; set; }
        public RoomType RoomType { get; set; }
        public GameType pGameType { get; set; }
        public bool VoteKick { get; set; }
        public byte LevelLimit { get; set; }
        public ushort KillLimit { get; set; }
        public byte PremiumOnly { get; set; }
        public PingLimits RoomPing { get; set; }
        public byte ChannelID { get; set; }
        public string RoomPassword { get; set; }
        public bool isPrivate { get; set; }
        public byte LevelType { get; set; }
        public int Rounds { get; set; }
        public byte MinLevel { get; set; }
        public byte MaxLevel { get; set; }
        public int cDeberanPoints { get; set; }
        public int cNIUPoints { get; set; }
        public byte AutoStart { get; set; }
        public int RoomTimeLeft { get; set; }
        private Mutex locker = new Mutex();
        //need mutex???
        #region Send
        public void SendPacketToNIU(WRPacket pPacket)
        {
            foreach(RoomPlayer pP in this.TeamNIU.Values)
            {
                pP.pClient.SendPacket(pPacket);
            }
        }
        public void SendPacketToDERBAN(WRPacket pPacket)
        {
            foreach (RoomPlayer pP in this.TeamDEBERAN.Values)
            {
                pP.pClient.SendPacket(pPacket);
            }
        }
        public void SendPacketToAllRoomPlayers(WRPacket pPacket)
        {
            foreach (RoomPlayer pP in this.RoomPlayers.Values)
            {
                pP.pClient.SendPacket(pPacket);
            }
        }
        #endregion
        public void SetMinAndMaxLevel(byte LevelValue)
        {
            switch (LevelValue)
            {
                case 0:
                   this.MinLevel = 1;
                   this.MaxLevel = 99;
                   break;
                case 1:
                   this.MinLevel = 1;
                   this.MaxLevel = 10;
                    break;
                case 2:
                    this.MinLevel = 11;
                    this.MaxLevel = 20;
                    break;
                case 3:
                    this.MinLevel = 21;
                    this.MaxLevel = 30;
                    break;
                case 4:
                    this.MinLevel = 31;
                    this.MaxLevel = 40;
                    break;
                case 5:
                    this.MinLevel = 41;
                    this.MaxLevel = 99;
                    break;
            }
        }
        public void SetRealRound()
        {
            this.cNIUPoints = this.getRealRounds();
            this.cDeberanPoints = this.getRealRounds();
        }
        public void Release()
        {
            try
            {
                locker.ReleaseMutex();
            }
            catch { }
        }
        public void Enter()
        {
            locker.WaitOne();
        }
        public void SendResetSlotRoom(RoomPlayer pRoomPL)
        {
            using (var pack = new WRPacket((int)GameServerOpcodes.LeaveRoom))
            {
                pRoomPL.WriteResetSlot(pack,this.RoomMaster.RoomSlot);
                this.SendPacketToAllRoomPlayers(pack);
            }
        }
        public void RemovePlayer(int UserID)
        {
            RoomPlayer pPlayer = this.RoomPlayers.Values.First(m=> m.UserID == UserID);
            if (pPlayer.Team == TeamType.DERBAN)
            {
                this.TeamDEBERAN.Remove(pPlayer.RoomSlot);
            }
            else if(pPlayer.Team == TeamType.NIU)
            {
                this.TeamNIU.Remove(pPlayer.RoomSlot);
            }
            RoomPlayer pp;
            this.RoomPlayers.TryRemove(pPlayer.UserID, out pp);
        }
        public bool getEmptySlotNiu(out byte pSlot)
        {
            pSlot = 0;
            IEnumerable<int> keyRange = Enumerable.Range(this.MaxPlayers / 2, this.MaxPlayers);
            var freeKeys = keyRange.Except(this.TeamNIU.Keys);
            if (freeKeys.Count() == 0)
                return false; // no free slot

            pSlot = (byte)freeKeys.First();
            return true;
        }
        public bool getEmptySlotDerban(out byte pSlot)
        {
            pSlot = 0;
            IEnumerable<int> keyRange = Enumerable.Range(this.TeamDEBERAN.Count, this.MaxPlayers / 2);
            var freeKeys = keyRange.Except(this.TeamDEBERAN.Keys);
            if (freeKeys.Count() == 0)
                return false; // no free slot

            pSlot = (byte)freeKeys.First();
            return true;
        }
        public void SetAllIngame()
        {
            foreach (var pP in this.RoomPlayers.Values)
            {
                pP.isIngame = true;
            }
        }
        public bool pPlayerJoIn(RoomPlayer pPlayer)
        {
            byte slot;

           /* i            User.chooseClass = "1";
            User.setReadyState(0);
            User.Life = 1000;
            User.iKills = User.iDeaths = User.iPoints = 0;
            */
            if (this.RoomPlayers.Count <= 0)
            {
                /*User.setRoom(this);
                User.RoomSlot = 0;
              
                RoomMaster = 0;*/
                return true;
            }
            else
            {
                if (this.RoomPlayers.Count < this.MaxPlayers)
                {
                    for (int I = 0; I < this.MaxPlayers; I++)
                    {
                        if (I % 2 == 0)
                        {
                            if(getEmptySlotDerban(out slot))
                            {
                                pPlayer.RoomSlot = slot;
                               pPlayer.Team = Data.TeamType.DERBAN;
                               this.TeamDEBERAN.Add(pPlayer.RoomSlot, pPlayer);
                               this.RoomPlayers.TryAdd(pPlayer.UserID, pPlayer);
                                Log.WriteLine(LogLevel.Debug, "Room",pPlayer.pClient.Player.NickName + " Positive [" + I + "]");
                                return true;
                            }
                        }
                        else
                        {
                           if(getEmptySlotNiu(out slot))
                           {
                               pPlayer.RoomSlot = slot;
                                  Log.WriteLine(LogLevel.Debug, "Room",pPlayer.pClient.Player.NickName + " User Joined Side Negative [" + I + "]");
                                  this.TeamNIU.Add(pPlayer.RoomSlot, pPlayer);
                                pPlayer.Team = TeamType.NIU;
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
        public int getRealRounds()
        {
            int Rounds = 0;
            if (this.Mode == RoomMode.FFA)
            {
                switch (Rounds)
                {
                    case 0:
                        Rounds = 10;
                        break;
                    case 1:
                        Rounds = 15;
                        break;
                    case 2:
                        Rounds = 20;
                        break;
                    case 3:
                        Rounds = 25;
                        break;
                    case 4:
                        Rounds = 30;
                        break;
                }
            }
            else if (this.Mode == RoomMode.Explosive)
            {
                switch (Rounds)
                {
                    case 0:
                        Rounds = 1;
                        break;
                    case 1:
                        Rounds = 3;
                        break;
                    case 2:
                        Rounds = 5;
                        break;
                    case 3:
                        Rounds = 7;
                        break;
                    case 4:
                        Rounds = 9;
                        break;
                }
            }
            else if (this.Mode == RoomMode.Deathmatch)
            {
                switch (Rounds)
                {
                    case 0:
                        Rounds = 30;
                        break;
                    case 1:
                        Rounds = 50;
                        break;
                    case 2:
                        Rounds = 100;
                        break;
                    case 3:
                        Rounds = 150;
                        break;
                    case 4:
                        Rounds = 200;
                        break;
                }
            }
            return Rounds;
        }
        public void SetKillLimit(int Value)
        {
            if (this.Mode == RoomMode.FFA)
            {
                switch (Value)
                {
                    case 0:
                        this.KillLimit = 10;
                        break;
                    case 1:
                        this.KillLimit = 15;
                        break;
                    case 2:
                        this.KillLimit = 20;
                        break;
                    case 3:
                        this.KillLimit = 25;
                        break;
                    case 4:
                        this.KillLimit = 30;
                        break;
                    default:
                        this.KillLimit = 10;
                        break;
                }
            }
            else if (this.Mode == RoomMode.Deathmatch)
            {
                switch (Value)
                {
                    case 0:
                        this.KillLimit = 30;
                        break;
                    case 2:
                        this.KillLimit = 100;
                        break;
                    case 3:
                        this.KillLimit = 150;
                        break;
                    case 4:
                        this.KillLimit = 200;
                        break;
                    default:
                        this.KillLimit = 30;
                        break;
                }
            }
        }
        public void SendPlayerUpdate(GameClient pSender)
        {
            using (var pack = new WRPacket((int)GameServerOpcodes.UpdateRoomPlayers))
            {
                pack.addBlock(this.RoomPlayers.Count);
                foreach (RoomPlayer p in this.RoomPlayers.Values)
                {
                    p.WriteInfo(pack, pSender);
                }
                this.SendPacketToAllRoomPlayers(pack);
            }
        }
        public bool AllReady()
        {
            foreach (var pl in this.RoomPlayers.Values)
            {
                if (!pl.isReady)
                {
                    return false;
                }
            }
            return true;
        }
        public bool AllReadyToSpawn()
        {
            foreach (var pl in this.RoomPlayers.Values)
            {
                if (!pl.isReadyToSpawn)
                {
                    return false;
                }
            }
            return true;
        }
        public void WriteInfo(WRPacket pPacket)
        {
            pPacket.addBlock(this.RoomID);
            pPacket.addBlock(1);//team balance?
            pPacket.addBlock(this.RoomStatus);
            pPacket.addBlock(this.RoomMaster.RoomSlot);//RoomMasterSlot
            pPacket.addBlock(this.RoomName);
            pPacket.addBlock(Convert.ToByte((this.RoomPassword != "NULL")));// 1= with pw
            pPacket.addBlock(this.MaxPlayers);
            pPacket.addBlock(this.RoomPlayers.Count);
            pPacket.addBlock(this.MapID);
            pPacket.addBlock(0);
            pPacket.addBlock(2);
            pPacket.addBlock(0);
            pPacket.addBlock(this.Mode);//7
            pPacket.addBlock(4);//roomplayer?
            pPacket.addBlock(this.RoomStatus);
            pPacket.addBlock(4);
            pPacket.addBlock(0); // 1 = Room has Supermaster
            pPacket.addBlock(this.RoomType);
            pPacket.addBlock(this.LevelLimit);
            pPacket.addBlock(this.PremiumOnly);
            pPacket.addBlock(this.VoteKick);
            pPacket.addBlock(this.AutoStart);//autostart
            pPacket.addBlock(1); // ??
            pPacket.addBlock(this.RoomPing);
            pPacket.addBlock(1);
        }
        public void MovePlayer(RoomPlayer pPlayer,byte FromSlot)
        {
            using (var pPacket = new WRPacket((int)GameServerOpcodes.RoomAtion_Response))
            {
                pPacket.addBlock(1);
                pPacket.addBlock(FromSlot);
                pPacket.addBlock(0);
                pPacket.addBlock(2);
                pPacket.addBlock((ushort)Data.RoomActionType.ChangeRoomSlot);
                pPacket.addBlock(1);
                pPacket.addBlock(0);
                pPacket.addBlock((byte)pPlayer.Team);
                pPacket.addBlock(pPlayer.RoomSlot);
                pPacket.addBlock(this.RoomMaster.RoomSlot);
                pPacket.addBlock(0);

                pPacket.addBlock(0);
                pPacket.addBlock(0);
                pPacket.addBlock(0);
                pPacket.addBlock(0);
                pPlayer.pClient.SendPacket(pPacket);
            }
        }
        public void SendPlayerJoin(RoomPlayer pPLayer)
        {
            using (var pack = new WRPacket((int)GameServerOpcodes.PlayerJoinRoom))
            {
                pack.addBlock((byte)pPLayer.Team);
                pack.addBlock(pPLayer.RoomSlot);
                this.WriteInfo(pack);
                SendPacketToAllRoomPlayers(pack);
            }
        }
        public void Remove()
        {
            foreach (var pPlayer in this.RoomPlayers.Values)//update all players
            {
                pPlayer.pClient.Player.pRoom = null;

            }
            PlayerRoom r;
            if (RoomManager.Instance.ServerRooms.TryRemove(this.RoomID, out r))
            {
                r = null;
            }
            else
            {
                Log.WriteLine(LogLevel.Warn, "Failed Remove Room {0}", this.RoomName);
            }
        }
        public bool SwitchMaster()
        {
            List<RoomPlayer> NotMasters = this.RoomPlayers.Values.ToList().FindAll(m => m.isMaster == false);
            if (NotMasters.Count == 0)
                return false;

             RoomPlayer newmaster = NotMasters.First();
             this.RoomMaster = newmaster;
             return true;
        }
    }
}
