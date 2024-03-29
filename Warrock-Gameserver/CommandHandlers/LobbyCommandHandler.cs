﻿using System;
using System.Collections.Generic;
using Warrock.Game;
using Warrock.CommandHandlers.CommandInfo;
using Warrock.Util;
using Warrock.Lib.Networking;
using Warrock.Lib;

namespace Warrock.CommandHandlers
{
    [ServerModule(InitializationStage.Metadata)]
    public class LobbyCommandHandler
    {
         public static LobbyCommandHandler Instance { get; private set; }
        public delegate void Command(Player pPlayer, params string[] param);
        private readonly Dictionary<string, LobbyCommandInfo> commands = new Dictionary<string, LobbyCommandInfo>();

        public LobbyCommandHandler()
        {
            LoadCommands();
            Log.WriteLine(LogLevel.Info, "{0} Lobbycommand(s) registered.", commands.Count);
        }
        public void LoadCommands()
        {
            RegisterCommand("/testChange", TestChangeRoom, 1);
            RegisterCommand("/test", test, 1);
        }
        private void TestChangeRoom(Player pPlayer, params string[] param)//testpacket
        {
            using(var pack = new WRPacket((int)30000))
            {
                pack.addBlock(1);
                pack.addBlock(51);//type

                pack.addBlock(1);
                pack.addBlock(0);//unk
                pack.addBlock(53);
                pack.addBlock(0);
                pack.addBlock(51);//oldvalue
                pack.addBlock(12);//place1
                pack.addBlock(0);//MasterPlace
                pack.addBlock(0);//value

                pack.addBlock(0);//unk
                pack.addBlock(0);
                pack.addBlock(0);

                pPlayer.pClient.SendPacket(pack);

            }
        }
        private void test(Player pPlayer, params string[] param)
        {
           // Handlers.PacketHelper.SendMessage(pPlayer.pClient, "You Are Gay :D");
            using (var pPacket = new WRPacket((int)GameServerOpcodes.RoomAtion_Response))
            {
                pPacket.addBlock(1);
                pPacket.addBlock(9);//from slot?
                pPacket.addBlock(0);
                pPacket.addBlock(2);
                pPacket.addBlock((ushort)Data.RoomActionType.ChangeRoomSlot);
                pPacket.addBlock(1);
                pPacket.addBlock(0);
                pPacket.addBlock(0);//packetvalue
                pPacket.addBlock(9);//value
                pPacket.addBlock(9);//mastervalue
                pPacket.addBlock(0);//packetvalue2

                pPacket.addBlock(0);
                pPacket.addBlock(0);
                pPacket.addBlock(0);
                pPacket.addBlock(0);
                pPlayer.pClient.SendPacket(pPacket);
            }
        }
        public CommandStatus ExecuteCommand(Player pPlayer, string[] command)
        {
            if (pPlayer == null) return CommandStatus.Error;
            LobbyCommandInfo info;
            if (commands.TryGetValue(command[2].ToLower(), out info))
            {
                if (info.GmLevel > pPlayer.pClient.AccountInfo.Access_level)
                {
                    return CommandStatus.GMLevelTooLow;
                }
                else
                {
                    try
                    {
                        info.Function(pPlayer, command);
                        return CommandStatus.Done;
                    }
                    catch (Exception ex)
                    {
                        string wholeCommand = string.Join(" ", command);
                        Log.WriteLine(LogLevel.Exception, "Exception while handling Ingamecommand '{0}': {1}", wholeCommand, ex.ToString());
                        return CommandStatus.Error;
                    }
                }
            }
            else return CommandStatus.NotFound;
        }
        public string[] GetCommandParams(string command)
        {
            LobbyCommandInfo info;
            if (commands.TryGetValue(command, out info))
            {
                return info.Parameters;
            }
            else return null;
        }

        private void CommandParam(Player pPlayer, params string[] param)
        {
            string input = param[1];
            string request;
            if (!input.StartsWith("/"))
            {
                request = "/" + input;
            }
            else request = input;
            LobbyCommandInfo info;
            if (commands.TryGetValue(request, out info))
            {
                string output = request + ": ";
                if (info.Parameters.Length > 0)
                {
                    foreach (var par in info.Parameters)
                    {
                        output += "[" + par + "] ";
                    }
                }
                else output += "None";
                //patram message
            }
            else
            {
                //"Command not found.");
            }
        }
        [InitializerMethod]
        public static bool Load()
        {
            Instance = new LobbyCommandHandler();
            return true;
        }
        public void RegisterCommand(string command, Command function, byte gmlevel, params string[] param)
        {
            if (commands.ContainsKey(command))
            {
                Log.WriteLine(LogLevel.Warn, "{0} already regis tered as a Lobbycommand.", command);
                return;
            }
            LobbyCommandInfo info = new LobbyCommandInfo(command.ToLower(), function, gmlevel, param);
            commands.Add(command.ToLower(), info);
        }
    }
}
