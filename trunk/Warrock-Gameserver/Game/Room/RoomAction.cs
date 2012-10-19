using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Warrock.Data;
using Warrock.Lib.Networking;
using Warrock.Networking;
using Warrock.Lib;

namespace Warrock.Game.Room
{
    public class RoomAction
    {
        public RoomActionType Action { get; set; }
        public int PacketValue { get; set; }
        public int PacketValue2 { get; set; }


        //this for play
        public int Value { get; set; }
        public int MasterValue { get; set; }

        public void WriteInfo(WRPacket pPacket)
        {
            /*tvalue = packetvalue
              place1 = value
              place2 = mastervalue
              value = packetvalue2*/
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
        public RoomAction(int PacketValue, int PacketValue2, int Value, int MasterValue)
        {
            this.PacketValue = PacketValue;
            this.PacketValue2 = PacketValue2;
            this.Value = Value;
            this.MasterValue = MasterValue;
        }
        public RoomAction()
        {
        }
        public void SendToRoom(PlayerRoom pRoom)
        {
            using(var pack = new WRPacket((int)GameServerOpcodes.RoomAtion_Response))
            {
                this.WriteInfo(pack);
                pRoom.SendPacketToAllRoomPlayers(pack);
            }
        }
    }
}
