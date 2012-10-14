using System;

namespace Warrock.Lib.Networking
{
    public sealed class PacketReceivedEventArgs : EventArgs
    {
        public WRPacket Packet { get; private set; }
        public PacketReceivedEventArgs(WRPacket packet)
        {
            this.Packet = packet;
        }
    }
}
