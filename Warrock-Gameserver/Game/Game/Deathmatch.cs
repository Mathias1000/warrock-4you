using System;
using Warrock.Util;
using Warrock.Game.Room;

namespace Warrock.Game.Game
{
    public class Deathmatch : Game
    {
     public long KillsDeberanLeft { get; set; }
     public long KillsNIULeft { get; set; }

        public Deathmatch(PlayerRoom Room)
        {
            this.pRoom = Room;
            this.SetIngame();
        }
    }
}
