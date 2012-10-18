using System;
using Warrock.Util;
using Warrock.Game.Room;

namespace Warrock.Game.Game
{
    public class FFAGame : Game
    {
        public long HighestKills { get; set; } 
        public long Rounds { get; set; }

        public FFAGame(PlayerRoom Room)
        {
            this.pRoom = Room;
            this.SetIngame();
        }
    }
}
