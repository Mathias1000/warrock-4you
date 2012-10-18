using System;
using System.Timers;
using Warrock.Util;
using Warrock.Game.Room;
using Warrock.Data;
using Warrock.Lib;
using Warrock.Lib.Networking;

namespace Warrock.Game.Game
{
    public class Game
    {
        public PlayerRoom pRoom { get;  set; }

        public int RoomTimeLeft { get { return this.pRoom.RoomTimeLeft; } set { this.pRoom.RoomTimeLeft = value; } }

        public bool GameActive { get { return this.pRoom.GameActive; } }
        #region GameBase Variables
        public ushort CurrentRound { get; set; }
        public long RoundTimeSpend { get; set; }
        #endregion
        private readonly Timer SpawnTimer;

        public Game()
        {
            SpawnTimer = new Timer(5000);//5 sec ok?
            SpawnTimer.Elapsed += new ElapsedEventHandler(GameSpawnTick);
            SpawnTimer.Start();
        }
        public void SetIngame()
        {
            foreach (var p in pRoom.RoomPlayers.Values)
            {
                p.pClient.Player.PlayGame = this;
            }
        }
        public RoomAction OpenSpawn()
        {
            this.pRoom.GameActive = true;
            RoomAction SpawnAction = new RoomAction
            {
                Action = RoomActionType.OpenSpawn,
                PacketValue = 3,
                Value = 882,
                PacketValue2 = 2,

            };
            return SpawnAction;
        }

        public void GameSpawnTick(object sender, ElapsedEventArgs e)
        {
            if (this.GameActive)
            {
            }
        }
    }
}
