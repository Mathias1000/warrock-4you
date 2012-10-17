using System;
using Warrock.Util;
using Warrock.Game.Room;

namespace Warrock.Game.Game
{
    public class Explosiv : Game
    {
        public Explosiv(PlayerRoom Room)
        {
            this.pRoom = Room;
            this.SetIngame();
        }
    }
}
