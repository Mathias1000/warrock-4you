using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Warrock.Networking;

namespace Warrock.Game
{
    public class RoomPlayer
    {
        private GameClient pClient { get; set; }

        public RoomPlayer(GameClient Client)
        {
            this.pClient = Client;
        }
    }
}
