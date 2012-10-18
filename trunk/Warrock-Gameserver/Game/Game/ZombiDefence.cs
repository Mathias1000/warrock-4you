using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Warrock.Game.Game
{
   public class ZombiDefence : ZombiGame
    {
       public ZombiDefence(PlayerRoom Room)
        {
            this.pRoom = Room;
            this.SetIngame();
        }
    }
}
