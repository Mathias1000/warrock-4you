using System;
using Warrock.Util;
using Warrock.Game.Room;

namespace Warrock.Game.Game
{
    public class Game
    {
        public PlayerRoom pRoom { get;  set; }

        public int RoomTimeLeft { get { return this.pRoom.RoomTimeLeft; } set { this.pRoom.RoomTimeLeft = value; } }
        public Game()
        {
        }
        public void SetIngame()
        {
            foreach (var p in pRoom.RoomPlayers.Values)
            {
                p.pClient.Player.PlayGame = this;
            }
        }
    }
}
