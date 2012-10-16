using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Warrock.Networking;
using Warrock.Data;
using Warrock.Lib.Networking;

namespace Warrock.Game
{
    public class RoomPlayer
    {
        public GameClient pClient { get; set; }
        public TeamType Team { get; set; }
        public int UserID { get; set; }
        public bool isReady { get; set; }
        public PlayerRoom pRoom { get; set; }
        public bool isMaster { get; set; }
        public int Life { get; set; }
        public float PosX { get; set; }
        public float PosY { get; set; }
        public float PosZ { get; set; }
        public byte RoomSlot { get; set; }
        public int CurrentWeapon { get; set; }
        public bool isLiving { get; set; }
        public int FuckPacket = 1000;
        public int FuckPacket2 = 165000;
        public string chooseClass = "1";
        public bool isSpawned { get; set; }

        public void WriteInfo(WRPacket pPacket)
        {
        }
    }
}
