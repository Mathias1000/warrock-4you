using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Warrock.Game.Game
{
    public class ZombiGame : Game
    {
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
