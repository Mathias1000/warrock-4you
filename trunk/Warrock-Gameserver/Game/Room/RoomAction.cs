using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Warrock.Data;
using Warrock.Lib.Networking;

namespace Warrock.Game.Room
{
    public class RoomAction
    {
        public RoomActionType Action { get;  set; }
        public ushort PacketValue { get; set; }
        public ushort PacketValue2 { get; set; }
        

        //this for play
        public ushort Value { get; set; }
        public ushort MasterValue { get; set; }

        public void WriteInfo(WRPacket pPacket)
        {
            pPacket.addBlock(1);
            pPacket.addBlock(0);
            pPacket.addBlock(0);
            pPacket.addBlock(2);
            pPacket.addBlock((ushort)this.Action);
            pPacket.addBlock(1);
            pPacket.addBlock(0);
            pPacket.addBlock(this.PacketValue);
            pPacket.addBlock(this.Value);
            pPacket.addBlock(this.MasterValue);
            pPacket.addBlock(this.PacketValue2);
            pPacket.addBlock(0);
            pPacket.addBlock(0);
            pPacket.addBlock(0);
            pPacket.addBlock(0);
        }
    }
}
