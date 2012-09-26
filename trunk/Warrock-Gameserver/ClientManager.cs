using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Timers;
using Warrock.Util;
using Warrock.Networking;

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

		public void DisconnectAll()
		{
			foreach (var c in clientsByName.Values)
			{
				c.Disconnect();
			}
		}

		readonly List<string> pingTimeouts = new List<string>();
		public void PingCheck()
		{
			lock (clientsByName)
			{
			   
				foreach (var kvp in clientsByName)
				{
					var client = kvp.Value;
					if (!client.Authenticated) continue; //they don't have ping shit, since they don't even send a response.
					if (client.HasPong)
					{
						//Todo SendPing
						client.HasPong = false;
						
					}
					else
					{
							pingTimeouts.Add(kvp.Key);
							Log.WriteLine(LogLevel.Debug, "Ping timeout from {0} ({1})", client.Username, client.Host);
					}
				}

				foreach (var client in pingTimeouts)
				{
					GameClient derp = null;
					clientsByName.TryRemove(client, out derp);
					derp.Disconnect();
				}
				pingTimeouts.Clear();
			}
		}

		public bool HasClient(string charName)
		{
			return clientsByName.ContainsKey(charName);
		}
		public GameClient GetClientByCharName(string pCharName)
		{
			return clientsByName[pCharName];
		}
       
		public bool AddClient(GameClient client)
		{
	  
			if (client.Player == null)
			{
				Log.WriteLine(LogLevel.Warn, "ClientManager trying to add character = null.", client.Username);
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
			}
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
