using System;
using Warrock.Lib;
using Warrock.Networking;
using Warrock.Lib.Networking;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Warrock.Data;

namespace Warrock.Game
{
    public class PlayerRoom
    {
        public readonly  Dictionary<int,RoomPlayer> RoomPlayers = new Dictionary<int,RoomPlayer>();
        public Dictionary<int, RoomPlayer> TeamDEBERAN = new Dictionary<int, RoomPlayer>();
        public Dictionary<int, RoomPlayer> TeamNIU = new Dictionary<int, RoomPlayer>();

        public RoomPlayer RoomMaster { get; set; }
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

        //need mutex???
        #region Lists
        public void AddPlayerToRoom(RoomPlayer pPlayer)
        {
            this.RoomPlayers.Add(pPlayer.UserID,pPlayer);
        }
        public void RemovePlayerToRoom(RoomPlayer pPlayer)
        {
            this.RoomPlayers.Remove(pPlayer.UserID);
        }
        public void AddPlayerToDERBIAN(RoomPlayer pPlayer)
        {
            this.TeamDEBERAN.Add(pPlayer.UserID, pPlayer);
        }
        public void RemovePlayerFroMDERBIAN(int pUserID)
        {
            this.TeamDEBERAN.Remove(pUserID);
        }
        public void AddPlayerToNIU(RoomPlayer pPlayer)
        {
            this.TeamNIU.Add(pPlayer.UserID, pPlayer);
        }
        public void RemovePlayerFromNIU(int pUserID)
        {
            this.TeamNIU.Remove(pUserID);
        }
        #endregion
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
        public void WriteInfo(WRPacket pPacket)
        {
            pPacket.addBlock(this.RoomID);
            pPacket.addBlock(1);
            pPacket.addBlock(this.RoomStatus);
            pPacket.addBlock(this.RoomMaster.RoomSlot);//RoomMasterSlot
            pPacket.addBlock(this.RoomName);
            pPacket.addBlock(0);
            pPacket.addBlock(this.MaxPlayers);
            pPacket.addBlock(this.RoomPlayers.Count);
            pPacket.addBlock(this.MapID);
            pPacket.addBlock(0);
            pPacket.addBlock(2);
            pPacket.addBlock(0);
            pPacket.addBlock(this.Mode);//7
            pPacket.addBlock(4);//roomplayer?
            if ((byte)this.Mode != 7)
            {
                pPacket.addBlock(1);
            }
            else
            {
                if (this.RoomPlayers.Count == 4)
                {
                    pPacket.addBlock(0); // 0 = unjoinable(grey room)
                }
                else
                {
                    pPacket.addBlock(1);
                }
            }
            pPacket.addBlock(4);
            pPacket.addBlock(0); // 1 = Room has Supermaster
            pPacket.addBlock(this.RoomType);
            pPacket.addBlock(this.LevelLimit);
            pPacket.addBlock(this.PremiumOnly);
            pPacket.addBlock(this.VoteKick);
            pPacket.addBlock(this.AutoStart);//autostart
            pPacket.addBlock(0); // ??
            pPacket.addBlock(this.RoomPing);
            pPacket.addBlock(-1);
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
    }
}
