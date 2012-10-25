using System;
using Warrock.Util;
using Warrock.Game.Room;

namespace Warrock.Game.Game
{
    public class Explosiv : Game
    {
        public ushort RoundsWonDerb { get; set; }
        public ushort RoundsWonNIU { get; set; }
        public bool BombPlanted { get; set; }

        public Explosiv(PlayerRoom Room)
        {
            this.pRoom = Room;
            this.SetIngame();
        }
        public override void Update()
        {
            if (this.RoomTimeLeft > 0) RoomTimeLeft -= 1000;

            this.RoundTimeSpend += 1000;

            if (this.RoundTimeSpend > 10000) { this.RoundTimeSpend = 0; }

            foreach (var pPlayer in this.pRoom.RoomPlayers.Values)
            {
                if (pPlayer.isSpawned)
                {
                  
                    pPlayer.isReadyToSpawn = false;
                }
                else
                {
                    if (!pPlayer.isSpawned && pPlayer.Spawntickcount <= 20)
                    {
                        pPlayer.Spawntickcount++;
                        pPlayer.PlayerSpawnTick();
                    }
                    else
                    {
                        pPlayer.PlayerSpawn();
                        pPlayer.isSpawned = true;
                    }
                }
            }
          
        }
    }
}
