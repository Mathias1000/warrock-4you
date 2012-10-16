using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Warrock.Util;
using System.Collections.Concurrent;
using Warrock.Game;

namespace Warrock
{
    [ServerModule(Util.InitializationStage.DataStore)]
    public class RoomManager
    {
        public static RoomManager Instance { get; private set; }
        public  ConcurrentDictionary<int, PlayerRoom> ServerRooms = new ConcurrentDictionary<int, PlayerRoom>();
        public int MaxRoomPages = 17; //17 = 254 rooms

        [InitializerMethod]
        public static bool Load()
        {
            
            Instance = new RoomManager()
            {
            };
            Log.WriteLine(LogLevel.Info, "RoomManager initialized.");
            return true;
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
