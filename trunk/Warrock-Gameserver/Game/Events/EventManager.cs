using System;
using System.Collections.Generic;
using System.Reflection;
using Warrock.Networking;
using Warrock.Util;
using Warrock.Lib.Networking;
using Warrock.Data;
using Warrock.Game.Room;
using Warrock.Util;

namespace Warrock.Game.Events
{
    [ServerModule(Util.InitializationStage.Metadata)]
    public class EventManager
    {
        private static readonly Dictionary<RoomActionType, MethodInfo> RoomEvents = new Dictionary<RoomActionType, MethodInfo>();

        public static EventManager Instance { get; private set; }

        [InitializerMethod]
        public static bool Load()
        {
            Instance = new EventManager();
            ushort HandlerCount = 0; ;
            foreach (var info in Reflector.FindMethodsByAttribute<RoomEventAttribute>())
            {
                if (!RoomEvents.ContainsKey(info.First.Type))
                {

                    HandlerCount++;
                }
                else
                {
                    Log.WriteLine(LogLevel.Warn, "Duplicate RoomAction found: {0}", info.First.Type);
                }
            }
            Log.WriteLine(LogLevel.Info, "Load : {0} Handlers", HandlerCount);
            return true;
        }
        public static Action GetCallback(MethodInfo method, params object[] parameters)
        {
            return () => method.Invoke(null, parameters);
        }
        public void RoomEventInvoke(RoomActionType Action, RoomPlayer pPlayer,int PacketValue,int PacketValue2,int Value,int MasterValue)
        {
            MethodInfo Event;
            if (RoomEvents.TryGetValue(Action, out Event))
            {
                RoomAction RoomAction = new RoomAction(PacketValue, PacketValue2, Value, MasterValue);
                if (pPlayer == null) { return; }
                Action action = GetCallback(Event, pPlayer,RoomAction);
                RoomActionWorker.Instance.AddCallback(action);
            }
            else
            {
                Log.WriteLine(Util.LogLevel.Warn, "Unhandelt RoomEvent {0}", Action);
            }
        }
    }
}
