using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Warrock.Game.Game
{
   public class ZombiServervival : ZombiGame
    {
       public ZombiServervival(PlayerRoom Room)
        {
            this.pRoom = Room;
            this.SetIngame();
        }
    }
}
