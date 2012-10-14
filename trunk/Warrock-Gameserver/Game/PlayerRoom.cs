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
        private readonly List<RoomPlayer> RoomPlayers = new List<RoomPlayer>();
        private readonly ConcurrentDictionary<RoomPlayer, GameClient> TeamA = new ConcurrentDictionary<RoomPlayer, GameClient>();
        private readonly ConcurrentDictionary<RoomPlayer, GameClient> TeamB = new ConcurrentDictionary<RoomPlayer, GameClient>();
        
        private RoomPlayer RoomMaster { get; set; }
        private int RoomID { get; set; }
        private byte MaxPlayers { get; set; }
        private int MapID { get; set; }
        private bool IsWaiting { get; set; }
        private string RoomName { get; set; }
        private RoomMode Mode { get; set; }
        private int RoomType { get; set; }
        private bool VoteKick { get; set; }
        private byte LevelLimit { get; set; }
        private bool PremiumOnly { get; set; }
        private bool RoomPing { get; set; }

        public PlayerRoom(RoomPlayer RoomMaster)
        {
            this.RoomID = Program.ServerRooms.Count + 1;
            this.RoomMaster = RoomMaster;
            this.RoomPlayers.Add(RoomMaster);
        }
        public void WriteInfo(WRPacket pPacket)
        {
            pPacket.addBlock(this.RoomID);
            pPacket.addBlock(1);
            pPacket.addBlock(this.IsWaiting);
            pPacket.addBlock(0);//RoomMasterSlot
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
            pPacket.addBlock(0);//autostart
            pPacket.addBlock(0); // ??
            pPacket.addBlock(this.RoomPing);
            pPacket.addBlock(-1);
        }
    }
}
