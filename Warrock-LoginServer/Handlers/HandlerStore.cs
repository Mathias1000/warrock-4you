using System;
using System.Collections.Generic;
using System.Reflection;
using Warrock.Lib;
using Warrock.Util;
using Warrock.Lib.Networking;

namespace Warrock_LoginServer.Handlers
{
    [ServerModule(InitializationStage.Metadata)]
    public class HandlerStore
    {
        private static readonly Dictionary<int, MethodInfo> LoginHandlers = new Dictionary<int, MethodInfo>();
  
        [InitializerMethod]
        public static bool Load()
        {
            ushort HandlerCount = 0;;
            foreach (var info in Reflector.FindMethodsByAttribute<PacketHandlerAttribute>())
            {
                if (!LoginHandlers.ContainsKey(info.First.OpCode))
                {
                    LoginHandlers.Add(info.First.OpCode, info.Second);
                    HandlerCount++;
                }
                else
                {
                    Log.WriteLine(LogLevel.Warn, "Duplicate handler found: {0}", info.First.OpCode);
                }
            }
            Log.WriteLine(LogLevel.Info, "Load : {0} Handlers", HandlerCount);
            return true;
        }

        public static MethodInfo GetHandler(int header)
        {
            MethodInfo meth;
            if (LoginHandlers.TryGetValue(header, out meth))
            {
                return meth;
            }
            return null;
        }

        public static Action GetCallback(MethodInfo method, params object[] parameters)
        {
            return () => method.Invoke(null, parameters);
        }
    }
}
