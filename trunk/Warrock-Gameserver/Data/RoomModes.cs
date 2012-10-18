using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Warrock.Data
{
    public enum RoomMode : byte
    {
        Explosive = 0,
        FFA = 1,
        Deathmatch=2,
        Conquest = 3,
        ZombiServervival = 7,
        ZombiDefence = 8,
    }
    public enum GameType : byte
    {
        UrbanObs = 0,
        BattleGroup = 2,
        CloseQuarders = 12,
        Zombie = 46,
    }
    public enum RoomType: byte
    {
        Knugels = 2,
        Normal = 0,
    }
    public enum PingLimits : byte
    {
        Green,
        Yellow,
        Red,
    }
}
