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

		public ClientManager()
		{
		}

        public void UpdatePing()
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
		public bool AddClient(GameClient client)
		{
            clientsByName.TryAdd("test", client);
	  
			/*if (client.Player.PlayerName == null)
			{
				Log.WriteLine(LogLevel.Warn, "ClientManager trying to add player = null.", client.AccountInfo.username);
				return false;
			}
			else if (clientsByName.ContainsKey(client.Player.PlayerName))
			{
				Log.WriteLine(LogLevel.Warn, "Player {0} is already registered to client manager!", client.Player.PlayerName);
				return false;
			}
			else
			{
				if (!clientsByName.TryAdd(client.Player.PlayerName, client))
				{
					Log.WriteLine(LogLevel.Warn, "Could not add client to list!");
					return false;
				}
			}*/
			return true;
		}

		public void RemoveClient(GameClient client)
		{
			if(client.Player == null) return;
			GameClient deleted;
			clientsByName.TryRemove(client.Player.PlayerName, out deleted);

			if (deleted != client)
			{
				Log.WriteLine(LogLevel.Warn, "There was a duplicate client object registered for {0}.", client.Player.PlayerName);
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
