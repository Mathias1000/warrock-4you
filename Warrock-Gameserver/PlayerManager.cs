using Warrock.Game;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Timers;
using Warrock.Util;
using Warrock.Networking;
using System.Net;

namespace Warrock
{
    public delegate void PlayerEvent(Player pPlayer);
    public class PlayerManager
    {
        public static PlayerManager Instance { get; private set; }
        private PlayerEvent LoggetIn { get; private set; }
        private PlayerEvent LoggetOut { get; private set; }
        public PlayerManager()
        {
        }
        public Player GetpPlayerByID(int UserID)
        {
            GameClient pPlayer = ClientManager.Instance.GetClientByID(UserID);
            if (pPlayer.Player != null)
            {
                return pPlayer.Player;
            }
            return null;
        }
        public Player GetpPlayerByName(string NickName)
        {
            GameClient pPlayer = ClientManager.Instance.GetClientByName(NickName);
            if (pPlayer.Player != null)
            {
                return pPlayer.Player;
            }
            return null;
        }
        public void InvokeLoggetIn(Player pPlayer)
        {
            this.LoggetIn.Invoke(pPlayer);
        }
        public void InvokeLoggetOut(Player pPlayer)
        {
            this.LoggetOut.Invoke(pPlayer);
        }
        [InitializerMethod]
        public static bool Load()
        {
            Instance = new PlayerManager();
            {
            };
            Log.WriteLine(LogLevel.Info, "PlayerManager initialized.");
            return true;
        }
    }
}
