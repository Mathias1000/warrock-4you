using System;
using Warrock.Util;
using Warrock.Game.Room;

namespace Warrock.Game.Game
{
    public class Explosiv : Game
    {
        public ushort RoundsWonDerb { get; set; }
        public ushort RoundsWonNIU { get; set; }
        public Explosiv(PlayerRoom Room)
        {
            this.pRoom = Room;
            this.SetIngame();
        }
    }
}
