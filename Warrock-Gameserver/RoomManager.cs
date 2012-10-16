using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Warrock.Util;
using System.Collections.Concurrent;
using Warrock.Game;
using Warrock.Lib.Networking;
using Warrock.Lib;

namespace Warrock
{
    [ServerModule(Util.InitializationStage.DataStore)]
    public class RoomManager
    {
        public static RoomManager Instance { get; private set; }
        public  ConcurrentDictionary<int, PlayerRoom> ServerRooms = new ConcurrentDictionary<int, PlayerRoom>();
        public int MaxRoomPages = 17; //17 = 254 rooms this is maxium of client

        [InitializerMethod]
        public static bool Load()
        {
            
            Instance = new RoomManager()
            {
            };
            Log.WriteLine(LogLevel.Info, "RoomManager initialized.");
            return true;
        }
        public PlayerRoom GetRoomByChanneldAndID(byte ChanneldID, int RoomID)
        {
           IEnumerable<PlayerRoom> Rooom = ServerRooms.Values.Where(m => m.ChannelID == ChanneldID && m.RoomID == RoomID);
           if (Rooom.Count() == 0) { return null; }
           PlayerRoom FinalRoom = Rooom.First();
           return FinalRoom;
        }
        public void UpdatePageByID(int PageID,int ChanneldID)
        {
            using (var pack = new Warrock.Lib.Networking.WRPacket((int)GameServerOpcodes.Room_List))
            {
                for (int i = 15 * PageID, j = 0; j < 15 && i < RoomManager.Instance.ServerRooms.Count; ++i, ++j)//thanks crommon
                {
                    if (RoomManager.Instance.ServerRooms.ContainsKey(i))
                    {
                        RoomManager.Instance.ServerRooms[i].WriteInfo(pack); ;
                    }
                }
                PlayerManager.Instance.SendAllPlayerInChannelPacket(pack, ChanneldID);
            }
        }
        public bool GetEmptyRoomSlot(out int pSlot)
        {
            pSlot = 0;
            IEnumerable<int> keyRange = Enumerable.Range(0, 15 * MaxRoomPages);
            var freeKeys = keyRange.Except(ServerRooms.Keys);
            if (freeKeys.Count() == 0)
                return false; // no free slot

            pSlot = freeKeys.First();
            return true;
        }
    }
}
