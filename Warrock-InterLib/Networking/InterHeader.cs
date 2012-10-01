namespace Warrock_InterLib.Networking
{
    public enum InterHeader : ushort
    {
        Ping = 0x0000,
        Pong = 0x0001,
        Ivs = 0x0002,
        Auth = 0x003,
        Assign = 0x004,
        Assigned = 0x005,
        SendOnlineUserReuqest = 0x006,
    }
}
