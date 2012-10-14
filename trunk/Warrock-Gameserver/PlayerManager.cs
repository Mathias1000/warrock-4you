using Warrock.Game;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Timers;
using Warrock.Util;
using Warrock.Networking;
using System.Net;
using Warrock.Lib.Networking;

namespace Warrock
{
    public delegate void PlayerEvent(Player pPlayer);
    [ServerModule(InitializationStage.Clients)]
    public class PlayerManager
    {
        public static PlayerManager Instance { get; private set; }
        private PlayerEvent LoggetIn { get;  set; }
        private PlayerEvent LoggetOut { get;  set; }
        public PlayerManager()
        {
        }
        [InitializerMethod]
        public static bool Load()
        {
            Instance = new PlayerManager();
            Log.WriteLine(LogLevel.Info, "PlayerManager Initialized.");
            return true;
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
        public List<GameClient> getAllClients_WhereChannelID(int ChID)
        {
            List<GameClient> AllChannelUsers = ClientManager.Instance.GetAllClients().FindAll(m => m.Player.ChannelID == ChID);
            return AllChannelUsers;
        }
        public List<GameClient> getAllplayers()
        {
           List<GameClient> AllUsers = ClientManager.Instance.GetAllClients();
            return AllUsers;
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
            if (LoggetIn != null)
            {
                this.LoggetIn.Invoke(pPlayer);
            }
        }
        public void InvokeLoggetOut(Player pPlayer)
        {
            if (LoggetOut != null)
            {
                this.LoggetOut.Invoke(pPlayer);
            }
        }
    }
}
