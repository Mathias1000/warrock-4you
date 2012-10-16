using System;
using System.Text;
using Warrock.Lib;
using Warrock.Lib.Networking;
using Warrock.Networking;
using Warrock.Util;
using Warrock.Data;
using Warrock.Game.Chat;
using Warrock.Game;

namespace Warrock.Handlers
{
    public sealed class ChatHandler
    {
        [PacketHandler((int)ClientGameOpcode.ChangeChannel)]
        public static void ChangeChannel(GameClient pClient, WRPacket pPacket)
        {
            byte ChanneID = pPacket.ReadByte(2);
            if (pClient.Authenticated)
            {
                pClient.Player.ChannelID = ChanneID;
                PacketHelper.SendChannelChange(pClient);
            }
        }
        [PacketHandler((int)ClientGameOpcode.ClientChatMessage)]
        public static void RecvChatMessage(GameClient pClient, WRPacket pPacket)
        {
            byte chatType = pPacket.ReadByte(2);
            int TargetID = pPacket.ReadInt(3);
            string TargetName =pPacket.ReadString(4);
            string Message = pPacket.ReadString(5);
            string[] commandArgs = Message.Replace(Convert.ToChar(29), Convert.ToChar(0x20)).Replace(pClient.Player.NickName, "").Split(' ');//encode string and split
            using (var pack = new WRPacket((int)GameServerOpcodes.Chat_Message))
            {
                switch (chatType)
                {
                    case 3://channelChat
                        if(!Chat.ExecuteLobbyCommand(pClient.Player, commandArgs))
                        PacketHelper.WriteChatMessage(pack, Message, ChatType.Lobby_ToChannel, pClient.Player.NickName, pClient.SeassonID, TargetID);
                        PlayerManager.Instance.SendAllPlayerInChannelPacket(pack, pClient.Player.ChannelID);
                        break;
                    case 4:// Room to all in Rom Players
                        if (pClient.Player.pRoom != null)
                        {
                            if (!Chat.ExecuteLobbyCommand(pClient.Player, commandArgs))
                            PacketHelper.WriteChatMessage(pack, Message, ChatType.Room_ToAll, pClient.Player.NickName, pClient.SeassonID, TargetID);
                            pClient.Player.pRoom.SendPacketToAllRoomPlayers(pack);
                        }
                        break;
                    case 5://teamchat
                        if (pClient.Player.pRoom != null)
                        {
                            RoomPlayer pPlayer;
                            if (pClient.Player.pRoom.RoomPlayers.TryGetValue(pClient.Player.UserID, out pPlayer))
                            {
                                if (pPlayer.Team == TeamType.DERBAN)
                                {
                                    PacketHelper.WriteChatMessage(pack, Message, ChatType.Room_ToTeam, pClient.Player.NickName, pClient.SeassonID, TargetID);
                                    pPlayer.pRoom.SendPacketToDERBAN(pack);
                                }
                                else if(pPlayer.Team == TeamType.NIU)
                                {
                                    PacketHelper.WriteChatMessage(pack, Message, ChatType.Room_ToTeam, pClient.Player.NickName, pClient.SeassonID, TargetID);
                                    pPlayer.pRoom.SendPacketToNIU(pack);
                                }
                            }
                        }
                        break;
                    case 6://wisper
                        PacketHelper.WriteChatMessage(pack, Message, ChatType.Whisper, pClient.Player.NickName, pClient.SeassonID, TargetID, TargetName);
                       GameClient TargetCLient = ClientManager.Instance.GetClientByName(TargetName);
                       if (TargetCLient == null) return;
                       TargetCLient.SendPacket(pack);
                        break;
                    case 8://to all
                        if(!Chat.ExecuteLobbyCommand(pClient.Player, commandArgs))
                        PacketHelper.WriteChatMessage(pack, Message, ChatType.Lobby_ToAll, pClient.Player.NickName, pClient.SeassonID, TargetID);
                        PlayerManager.Instance.SendPacketToAllInLobby(pack);
                        break;
                    default:
                        Log.WriteLine(LogLevel.Warn, "Not implented chatType  Found {0}", chatType);
                        break;
                }
                
            }
        }
    }
}
