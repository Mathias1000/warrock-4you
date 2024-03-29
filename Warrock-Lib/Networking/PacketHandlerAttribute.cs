﻿using System;

namespace Warrock.Lib.Networking
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class PacketHandlerAttribute : Attribute
    {
        public int OpCode { get; private set; }

        public PacketHandlerAttribute(int opcode)
        {
            OpCode = opcode;
        }
    }
}
