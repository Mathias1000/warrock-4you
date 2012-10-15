using Warrock.Lib;
using Warrock.Lib.Networking;
using Warrock.Networking;
using Warrock.Util;
using Warrock.Game;
using System.Collections.Generic;
using System;
using Warrock.CommandHandlers.CommandInfo;

namespace Warrock.CommandHandlers
{
    [ServerModule(InitializationStage.Metadata)]
    public class CmdCommandHandler
    {
        public static CmdCommandHandler Instance { get; private set; }
        public delegate void Command(string cmd, params string[] param);
        private readonly Dictionary<string, CmdCommandInfo> commands = new Dictionary<string, CmdCommandInfo>();

        public CmdCommandHandler()
        {
            LoadCommands();
            Log.WriteLine(LogLevel.Info, "{0} Cmdcommand(s) registered.", commands.Count);
        }
        public void LoadCommands()
        {
            RegisterCommand("&test", test);
        }
        private void test(string Cmd, params string[] param)
        {
            Console.WriteLine("this is a testCommand");
        }
        public CommandStatus ExecuteCommand(string cmd, string[] command)
        {
            if (cmd == null) return CommandStatus.Error;
            CmdCommandInfo info;
            if (commands.TryGetValue(command[0].ToLower(), out info))
            {
                try
                {
                    info.Function(cmd, command);
                    return CommandStatus.Done;
                }
                catch (Exception ex)
                {
                    string wholeCommand = string.Join(" ", command);
                    Log.WriteLine(LogLevel.Exception, "Exception while handling command '{0}': {1}", wholeCommand, ex.ToString());
                    return CommandStatus.Error;
                }
            }
            else return CommandStatus.NotFound;
        }
        public string[] GetCommandParams(string command)
        {
            CmdCommandInfo info;
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
            if (!input.StartsWith("g"))
            {
                request = "g" + input;
            }
            else request = input;
            CmdCommandInfo info;
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
            Instance = new CmdCommandHandler();
            return true;
        }
        public void RegisterCommand(string command, Command function, params string[] param)
        {
            if (commands.ContainsKey(command))
            {
                Log.WriteLine(LogLevel.Warn, "{0} already registered as a Cmdcommand.", command);
                return;
            }
            CmdCommandInfo info = new CmdCommandInfo(command.ToLower(), function, param);
            commands.Add(command.ToLower(), info);
        }
    }
}
