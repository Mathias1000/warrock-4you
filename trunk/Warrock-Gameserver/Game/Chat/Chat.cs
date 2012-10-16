using System;
using Warrock.CommandHandlers;
using Warrock.CommandHandlers.CommandInfo;
using Warrock.Handlers;

namespace Warrock.Game.Chat
{
    public class Chat
    {
        public static bool ExecuteLobbyCommand(Player pPlayer, string[] args)
        {
            if (pPlayer.AccountInfo.Access_level > 0)
            {
                CommandStatus Status = LobbyCommandHandler.Instance.ExecuteCommand(pPlayer, args);
                switch (Status)
                {
                    case CommandStatus.Error:
                        PacketHelper.SendMessage(pPlayer.pClient, "Error executing command.");
                        break;
                    case CommandStatus.GMLevelTooLow:
                        PacketHelper.SendMessage(pPlayer.pClient, "You Access Level is To Low!");
                        break;
                    case CommandStatus.NotFound:
                        PacketHelper.SendMessage(pPlayer.pClient, "Command not found.");
                        break;
                }
                return true;
            }
            return false;
        }

    }
}
