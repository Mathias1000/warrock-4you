﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Warrock.Util;
using Warrock_InterLib.NetworkObjects;
using Warrock_InterLib.Networking;

namespace Warrock.InterServer
{
    [ServerModule(Util.InitializationStage.Services)]
    public sealed class LoginConnector : AbstractConnector
    {
        public static LoginConnector Instance { get; private set; }

        public LoginConnector(string ip, int port)
        {
            try
            {
                Connect(ip, port);
                Log.WriteLine(LogLevel.Info, "Connected to server @ {0}:{1}", ip, port);
                this.client.OnPacket += new EventHandler<InterPacketReceivedEventArgs>(ClientOnPacket);
                this.client.OnDisconnect += new EventHandler<SessionCloseEventArgs>(ClientOnDisconnect);
                this.client.SendInterPass(Config.Instance.InterServerPassword);
                InterHandler.TryAssiging(this);
            }
            catch
            {
                Log.WriteLine(LogLevel.Error, "Couldn't connect to server @ {0}:{1}", ip, port);
                Console.ReadLine();
                Environment.Exit(7);
            }
        }

        void ClientOnDisconnect(object sender, SessionCloseEventArgs e)
        {
            Log.WriteLine(LogLevel.Error, "Disconnected from server.");
            this.client.OnPacket -= new EventHandler<InterPacketReceivedEventArgs>(ClientOnPacket);
            this.client.OnDisconnect -= new EventHandler<SessionCloseEventArgs>(ClientOnDisconnect);
        }

        void ClientOnPacket(object sender, InterPacketReceivedEventArgs e)
        {
            try
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
                    Log.WriteLine(LogLevel.Debug, "Unhandled packet: {0}", e.Packet);
                }
            }
            catch (Exception ex)
            {
                Log.WriteLine(LogLevel.Exception, ex.ToString());
            }
        }

        [InitializerMethod]
        public static bool Load()
        {
            return Load(Config.Instance.LoginServerIP, Config.Instance.LoginInterServerPort);
        }

        public static bool Load(string ip, int port)
        {
            try
            {
                Instance = new LoginConnector(ip, port);
                return true;
            }
            catch { return false; }
        }

        public void SendPacket(InterPacket packet)
        {
            if (this.client == null) return;
            this.client.SendPacket(packet);
        }
    }
}
