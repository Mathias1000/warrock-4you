﻿using System;
using System.Net.Sockets;
using System.Reflection;
using Warrock_Lib;
using Warrock_InterLib.Networking;
using Warrock.Util;
using Warrock.InterServer;
using Warrock_LoginServer;

namespace Warrock.InterServer
{
    public sealed class GameConnection : InterClient
    {
        public GameServerStatus Status { get; set; }
        public string Name { get; set; }
        public byte ID { get; set; }
        public string IP { get; set; }
        public ushort Port { get; set; }
        public int Load { get; private set; }

        public GameConnection(Socket sock) : base(sock)
        {
            Status = GameServerStatus.Low;
            this.OnPacket += new EventHandler<InterPacketReceivedEventArgs>(WorldConnection_OnPacket);
            this.OnDisconnect += new EventHandler<SessionCloseEventArgs>(WorldConnection_OnDisconnect);
        }

        void WorldConnection_OnDisconnect(object sender,SessionCloseEventArgs e)
        {
            /*if (IsAWorld)
            {
                this.OnPacket -= new EventHandler<InterPacketReceivedEventArgs>(WorldConnection_OnPacket);
                this.OnDisconnect -= new EventHandler<InterLib.Networking.SessionCloseEventArgs>(WorldConnection_OnDisconnect);
                WorldConnection derp;
                if (WorldManager.Instance.Worlds.TryRemove(ID, out derp))
                {
                    Log.WriteLine(LogLevel.Info, "World {0} disconnected.", ID);
                }
                else
                {
                    Log.WriteLine(LogLevel.Info, "Could not remove world {0}!?", ID);
                }
            }*/
        }

        void WorldConnection_OnPacket(object sender, InterPacketReceivedEventArgs e)
        {
            if (e.Client.Assigned == false)
            {
                if (e.Packet.OpCode == InterHeader.Auth)
                {
                    string pass;
                    if (!e.Packet.TryReadString(out pass))
                    {
                        Log.WriteLine(LogLevel.Error, "Couldn't read pass from inter packet.");
                        e.Client.Disconnect();
                        return;
                    }

                    if (!pass.Equals("test"))
                    {
                        Log.WriteLine(LogLevel.Error, "Inter password incorrect");
                        e.Client.Disconnect();
                        return;
                    }
                    else
                    {
                        e.Client.Assigned = true;
                    }
                }
                else
                {
                    Log.WriteLine(LogLevel.Info, "Not authenticated and no auth packet first.");
                    e.Client.Disconnect();
                    return;
                }
            }
            else
            {
                MethodInfo method = InterHandlerStore.GetHandler(e.Packet.OpCode);
                if (method != null)
                {
                    Action action = InterHandlerStore.GetCallback(method, this, e.Packet);
                    if (Worker.Instance == null)
                    {
                        action();
                    }
                    else
                    {
                        Worker.Instance.AddCallback(action);
                    }
                }
                else
                {
                    Log.WriteLine(LogLevel.Debug, "Unhandled interpacket: {0}", e.Packet);
                }
            }
        }
    }
}
