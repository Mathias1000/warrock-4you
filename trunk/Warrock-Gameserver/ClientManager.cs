using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Timers;
using Warrock.Util;
using Warrock.Networking;
using System.Net;

namespace Warrock
{
	[ServerModule(Util.InitializationStage.DataStore)]
	public sealed class ClientManager
	{
		public static ClientManager Instance { get; private set; }
		public int GameServerLoad { get { return ClientCount(); } }


		private readonly ConcurrentDictionary<string, GameClient> clientsByName = new ConcurrentDictionary<string, GameClient>();
        private readonly ConcurrentDictionary<int, GameClient> clientsByID = new ConcurrentDictionary<int, GameClient>();
        private readonly List<GameClient> GameClientList = new List<GameClient>();
		public ClientManager()
		{
		}

        public void UpdatePing()
        {
            lock (this)
            {
                foreach (GameClient Client in clientsByName.Values)
                {

                    try
                    {
                        byte[] buffer = new byte[32];
                        System.Net.NetworkInformation.PingOptions pingOptions = new System.Net.NetworkInformation.PingOptions(128, true);
                        System.Net.NetworkInformation.Ping ping = new System.Net.NetworkInformation.Ping();
                        System.Net.NetworkInformation.PingReply pingReply = ping.Send(((IPEndPoint)Client.Socket.RemoteEndPoint).Address, 75, buffer, pingOptions);

                        if (pingReply != null)
                        {
                            switch (pingReply.Status)
                            {
                                case System.Net.NetworkInformation.IPStatus.Success:
                                    Client.Player.Ping = pingReply.RoundtripTime;
                                    break;
                                default:
                                    Client.Player.Ping = 999;
                                    break;
                            }
                        }
                    }
                    catch { Client.Player.Ping = 999; }
                }
            }
        }
		private int ClientCount()
		{
			return clientsByName.Count;
		}

		public GameClient GetClientByName(string name)
		{
			GameClient client;
			if (clientsByName.TryGetValue(name, out client))
			{
				return client;
			}
			else return null;
		}
        public List<GameClient> GetAllClients()
        {
           return this.GameClientList;
        }
        public GameClient GetClientByID(int UserID)
        {
                GameClient client;
                if (clientsByID.TryGetValue(UserID, out client))
                {
                    return client;
                }
                else return null;
        }

		public bool HasClient(string charName)
		{
			return clientsByName.ContainsKey(charName);
		}
		public GameClient GetClientByCharName(string pCharName)
		{
			return clientsByName[pCharName];
		}
        public int GetClientCount()
        {
            return this.clientsByName.Count;
        }
        public GameClient GetClientBySeasson(int SeassonID)
        {
            GameClient pClient = this.GameClientList.Find(m => m.SeassonID == SeassonID);
            return pClient;
        }
		public bool AddClient(GameClient client)
		{
            lock(client)
            {
			if (clientsByID.ContainsKey(client.Player.UserID))
			{
				Log.WriteLine(LogLevel.Warn, "Player {0} is already registered to client manager!", client.Player.NickName);
				return false;
			}
			else
			{
                if (!clientsByID.TryAdd(client.Player.UserID, client))
                {
                    Log.WriteLine(LogLevel.Warn, "Could not add client to list!");
                    return false;
                }
                else
                {
                    clientsByName.TryAdd(client.AccountInfo.username, client);
                    GameClientList.Add(client);
                }
			}
        }
			return true;
		}

		public void RemoveClient(GameClient client)
		{
			if(client.Player == null) return;
            lock (client)
            {
                GameClient deletedbyName;
                GameClient deletedbyID;
                clientsByName.TryRemove(client.Player.NickName, out deletedbyName);

                clientsByID.TryRemove(client.Player.UserID, out deletedbyID);
                if (deletedbyID != client || deletedbyName != client || !GameClientList.Remove(client))
                {
                    Log.WriteLine(LogLevel.Warn, "There was a duplicate client object registered for {0}.", client.Player.NickName);
                }
            }
		}
		[InitializerMethod]
		public static bool Load()
		{
			Instance = new ClientManager()
			{
			};
			Log.WriteLine(LogLevel.Info, "ClientManager initialized.");
			return true;
		}
	}
}
