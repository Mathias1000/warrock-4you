using System;
using Warrock.Util;
using Warrock.Game.Room;

namespace Warrock.Game.Game
{
    public class Conquest : Game
    {
        public Conquest(PlayerRoom Room)
        {
            this.pRoom = Room;
            this.SetIngame();
        }
        public override void Update()
        {

        }
    }
}
