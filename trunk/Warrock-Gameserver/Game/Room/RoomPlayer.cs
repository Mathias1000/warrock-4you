using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Warrock.Networking;
using Warrock.Data;
using Warrock.Lib.Networking;
using Warrock.Game.Game;
using Warrock.Lib;

namespace Warrock.Game
{
    public class RoomPlayer
    {
        public GameClient pClient { get; set; }
        public TeamType Team { get; set; }
        public int UserID { get; set; }
        public bool isReady { get; set; }
        public PlayerRoom pRoom { get; set; }
        public bool isMaster { get; set; }
        public int Life { get; set; }
        public float PosX { get; set; }
        public float PosY { get; set; }
        public float PosZ { get; set; }
        public byte RoomSlot { get; set; }
        public int CurrentWeapon { get; set; }
        public bool isLiving { get; set; }
        public int FuckPacket = 1000;
        public int FuckPacket2 = 165000;
        public string chooseClass = "1";
        public bool isSpawned { get; set; }
        public bool isReadyToSpawn { get; set; }

        public bool isIngame { get; set; }
        public void WriteResetSlot(WRPacket pack, byte MasterSlot)
        {

            pack.addBlock(0);
            pack.addBlock(this.pClient.SeassonID);
            pack.addBlock(this.RoomSlot); // Position in Room
            pack.addBlock(0); // ?
            pack.addBlock(MasterSlot);
            pack.addBlock(this.pClient.Player.Experience);
            pack.addBlock(this.pClient.Player.Dinar);
        }

        public void WriteInfo(WRPacket pPacket)
        {
            this.isReady = true;
            pPacket.addBlock(this.pClient.Player.UserID); // User ID
            pPacket.addBlock(this.pClient.SeassonID); // Socket ID
            pPacket.addBlock(this.RoomSlot); // ID of Player In Room
            pPacket.addBlock(Convert.ToInt16(this.isReady)); //Room Ready State of Player(0 = not ready, 1 = ready)
            pPacket.addBlock((byte)TeamType.DERBAN); // Player Team in Room
            pPacket.addBlock(Convert.ToInt16(this.isReady)); //Room Ready State of Player(0 = not ready, 1 = ready)
            pPacket.addBlock(0);
            pPacket.addBlock(0);
            pPacket.addBlock(1000);
            pPacket.addBlock(this.pClient.Player.NickName); // Nickname of Player ololo
            pPacket.addBlock(7); // ? Clan
            pPacket.addBlock(7); // ? Clan
            pPacket.addBlock(7); // ? Clan
            pPacket.addBlock(this.pClient.uniqID2);// userID plus 1 ( Session Calculator ) 
            pPacket.addBlock(Convert.ToByte(this.pClient.Player.AccountInfo.isover18)); // UserID ( Age in KR ( 1 = over 18 , 0 = 14 ) ) 
            pPacket.addBlock(this.pClient.uniqIDisCRC); // CRCCheck of UserID
            pPacket.addBlock(this.pClient.Player.Premium);//premium _Client.getPremium()
            pPacket.addBlock(0);
            pPacket.addBlock(7);
            pPacket.addBlock(this.pClient.Player.Kills);//kills _Client.getKills()
            pPacket.addBlock(this.pClient.Player.Deaths);//deaths _Client.getDeaths()
            pPacket.addBlock(1000); ///WRPoints -> KR
            if (pClient.Player.pInventory.hasPX("CK01")) // Disgiuse Badge (Smiley Level)
            {
                pPacket.addBlock(-1); // Smiley Level
            }
            else
            {
                pPacket.addBlock(this.pClient.Player.Experience); // Client EXP
            }
            pPacket.addBlock(7);
            pPacket.addBlock(7);
            pPacket.addBlock(1);//Gewähle Klasse - in 30000 150 ?
            if (this.pRoom.RoomMaster.pClient.nIP == this.pClient.nIP && this.pRoom.RoomMaster.pClient.nIP == this.pClient.lIP)
                pPacket.addBlock(this.pClient.IPToInt("127.0.0.1")); // Player it self, send dam 127.0.0.1 to fix bugs
            else if (this.pRoom.RoomMaster.pClient.nIP == this.pClient.nIP && this.pRoom.RoomMaster.pClient.nIP != this.pClient.lIP)
                pPacket.addBlock(this.pClient.lIP);
            else
                pPacket.addBlock(this.pClient.nIP);       // Remote IP (UDP Connection)
            pPacket.addBlock(this.pRoom.RoomMaster.pClient.nPort); //network port
            pPacket.addBlock(this.pRoom.RoomMaster.pClient.lIP); // local ip 
            pPacket.addBlock(this.pRoom.RoomMaster.pClient.lPort); // local port 
            pPacket.addBlock(0);

        }

        public void PlayerSpawnTick()
        {
            Game.Game PlayerGame = pClient.Player.PlayGame;
            using (var SpawnPack = new WRPacket((int)GameServerOpcodes.GameSpawnTick))
            {
                if (PlayerGame is ZombiDefence || PlayerGame is ZombiServervival)
                {
                    SpawnPack.addBlock(-1);
                    SpawnPack.addBlock(PlayerGame.RoundTimeSpend);
                    SpawnPack.addBlock(2); // Points
                    SpawnPack.addBlock(2); // Total Points
                    SpawnPack.addBlock(30);
                }
                else
                {

                    SpawnPack.addBlock(PlayerGame.RoundTimeSpend); // Spawn Counter
                    SpawnPack.addBlock(PlayerGame.RoomTimeLeft); // Time Left
                    SpawnPack.addBlock(((this.pRoom.Mode == RoomMode.Explosive) ? PlayerGame.CurrentRound : -1)); //Current Round
                    SpawnPack.addBlock(0); // ?
                    if (PlayerGame is FFAGame)
                    {
                        FFAGame FFA = PlayerGame as FFAGame;
                        SpawnPack.addBlock(10 + (5 * FFA.Rounds)); // Mission Kills (FFA)
                        SpawnPack.addBlock(FFA.HighestKills); // Current Leader Kills (FFA)
                    }
                    else if (PlayerGame is Deathmatch)
                    {
                        Deathmatch DeathMatch = PlayerGame as Deathmatch;
                        SpawnPack.addBlock(DeathMatch.KillsDeberanLeft); // Score DERBERAN
                        SpawnPack.addBlock(DeathMatch.KillsNIULeft); // Score NIU
                    }
                    else if (PlayerGame is Explosiv)
                    {
                        Explosiv explosive = PlayerGame as Explosiv;
                        SpawnPack.addBlock(explosive.RoundsWonDerb);
                        SpawnPack.addBlock(explosive.RoundsWonNIU);
                    }
                    else if (PlayerGame is Conquest)
                    {
                        SpawnPack.addBlock(90000); // Players a life DERB
                        SpawnPack.addBlock(90000);  // Players a life NIU
                    }
                    SpawnPack.addBlock(30);
                    this.pClient.SendPacket(SpawnPack);

                }
            }
        }
    }
}}
