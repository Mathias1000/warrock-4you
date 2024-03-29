﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using MySql.Data.MySqlClient;
using Warrock.Networking;
using Warrock.Lib;
using Warrock.Database;
using Warrock.Lib.Networking;
using Warrock.Game.WeaponSets;
using Warrock.Data;


namespace Warrock.Game
{
    public class Player
    {
        #region ProzedureNames
        private const string details_Save = "details_Save";
        private const string AccountInf_Save = "AccountInfo_Save";
        #endregion
        #region Variabels
        public Inventory pInventory { get; set; }
        public GameClient pClient { get;  set; }
        public Account_Details Account_Details { get; set; }
        public tUser AccountInfo { get; set; }
        public byte ChannelID { get; set; }
        public bool IsInLobby { get; set; }
        public int Premium { get; set; }
        public int PlayerSeeRoomListPage { get; set; } 
        public PlayerRoom pRoom { get; set; }
        public Game.Game PlayGame { get; set; }
        #region Private
        private long ping { get; set; }
        #endregion
     
        #endregion
        #region Indexer Variabels
        #region Player
        public long Ping { get { return this.ping; } set { this.ping = value; } }
        #endregion
        #region AccountInfo
        public string NickName { get { return this.AccountInfo.NickName; } set {this.AccountInfo.NickName = value; } }
        public bool IsOnline { get { return ClientManager.Instance.HasClient(NickName); } }
        public byte Acces_level { get { return this.AccountInfo.Access_level; } set { this.AccountInfo.Access_level = value; } }
        public int SeassonID { get { return this.AccountInfo.SeassonID; } set { this.AccountInfo.SeassonID = value; } }
        public int UserID { get { return this.AccountInfo.UserID; } set { this.AccountInfo.UserID = value; } }
        public string AccountName { get { return this.AccountInfo.username; } }
        public string Password { get { return this.AccountInfo.Password; }}
        public bool Banned { get { return this.AccountInfo.Banned; } set { this.AccountInfo.Banned = value; } }
        public long BannTime { get { return this.AccountInfo.BannTime; } set { this.AccountInfo.BannTime = value; } }
        #endregion
        #region Account_Details
        public int Experience { get { return this.Account_Details.Experience; } set { this.Account_Details.Experience = value; } }
        public byte Level { get { return this.Account_Details.Level; } set { this.Account_Details.Level = value; } }
        public int Dinar { get { return this.Account_Details.Dinar; } set { this.Account_Details.Dinar = value; } }
        public int Kills { get { return this.Account_Details.Kills; } set { this.Account_Details.Kills = value; } }
        public int Deaths { get { return this.Account_Details.Deaths; } set { this.Account_Details.Deaths = value; } }
        public int Copons { get { return this.Account_Details.Copons; } set { this.Account_Details.Copons = value; } }
        public int Cash { get { return this.Account_Details.Cash; } set { this.Account_Details.Cash = value; } }
        #endregion
        #endregion
        #region Methods
        public void SetPlayerTag()
        {
            switch (pClient.Player.Acces_level)
            {
                case 1:
                    this.NickName = "[Tester]" + this.NickName;
                    break;
                case 2:
                    this.NickName = "[MOD]" + this.NickName;
                    break;
                case 3:
                    this.NickName = "[GM]" + this.NickName;
                    break;
                case 4:
                    pClient.Player.NickName = "[Dev]" + this.NickName;
                    break;
                case 5:
                    pClient.Player.NickName = "[Admin]" + this.NickName;
                    break;
                case 6:
                    pClient.Player.NickName = "[ServerAdmin]" + this.NickName;
                    break;
            }
        }
        public void WritePlayerInfo(WRPacket pPacket)
        {
          //  pPacket.AddBlockString("1 GS206 347 16825046 559 hhqqee12 -1 NULL -1 -1 0 0 -1 0 3 9432 6459 0 16390 105 626 0 0 0 0 0 F,F,F,F DA02,DB01,DF01,DR01,^,^,^,^ DA02,DB01,DF01,DQ01,^,^,^,^ DA02,DB01,DG05,DN01,^,^,^,^ DA02,DB01,DC02,I000,^,^,^,^ DA02,^,DJ01,DL01,^,^,^,^ DN05-1-1-12110219-0-0-0-0-0,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^ BA01,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^ BA02,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^ BA03,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^ BA04,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^ BA05,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^ ^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^ 0 1 0 ");

           pPacket.addBlock(1);
            pPacket.addBlock("GameServer32");
            pPacket.addBlock(this.pClient.SeassonID);
            pPacket.addBlock(this.UserID);
            pPacket.addBlock(this.pClient.SeassonID);
            pPacket.addBlock(this.NickName);
            ////////////////////////////////////////////////////
            pPacket.addBlock(-1); // //Clan ID
            pPacket.addBlock("NULL"); // Clan Name
            pPacket.addBlock(-1); ///1 = Member, 0 = Leader ?
            pPacket.addBlock(-1); // Clan Master
            ////////////////////////////////////////////////////
            pPacket.addBlock(4);//premium
            pPacket.addBlock(0);
            pPacket.addBlock(-1);
            pPacket.addBlock(0);
            pPacket.addBlock(this.Level);

            pPacket.addBlock(this.Experience);
            pPacket.addBlock(0); // Some UDP Stuff
            pPacket.addBlock(0);
            ////////////////////////////////////////////////////
            pPacket.addBlock(this.Dinar);
            pPacket.addBlock(this.Kills);
            pPacket.addBlock(this.Deaths);
            ////////////////////////////////////////////////////
            pPacket.addBlock(0);
            pPacket.addBlock(0);
            pPacket.addBlock(0);
            pPacket.addBlock(0);
            pPacket.addBlock(0);
            //////////////////////////////////////////////////////
            pPacket.addBlock(this.pInventory.getOpenSlots());

            /* Weapons*/
            
            pPacket.addBlock(this.pInventory.GetWeaponStringByType(WeaponSetType.WeaponSetE));
            pPacket.addBlock(this.pInventory.GetWeaponStringByType(WeaponSetType.WeaponSetM));
            pPacket.addBlock(this.pInventory.GetWeaponStringByType(WeaponSetType.WeaponSetS));
            pPacket.addBlock(this.pInventory.GetWeaponStringByType(WeaponSetType.WeaponSetA));
            pPacket.addBlock(this.pInventory.GetWeaponStringByType(WeaponSetType.WeaponSetH));

            pPacket.addBlock(this.pInventory.generateInventoryString());//items (Weapons/PX-Items)

            pPacket.addBlock(this.pInventory.GetCustomStringByType(pCustome.CostumeE));
            pPacket.addBlock(this.pInventory.GetCustomStringByType(pCustome.CostumeM));
            pPacket.addBlock(this.pInventory.GetCustomStringByType(pCustome.CostumeS));
            pPacket.addBlock(this.pInventory.GetCustomStringByType(pCustome.CostumeA));
            pPacket.addBlock(this.pInventory.GetCustomStringByType(pCustome.CostumeH));

            pPacket.addBlock(this.pInventory.generateCustomeString());//CostumeInventorystring

            pPacket.addBlock(0);
            pPacket.addBlock(1); // 0 = No Zombie 1 = All Channel 2 = Only Zombie all above = Fucked
            pPacket.addBlock(0);

        }
        public void RemovePlayerTag()
        {

            switch (pClient.Player.Acces_level)
            {
                case 1:
                    this.NickName.Replace("[Tester]", "");
                    break;
                case 2:
                    this.NickName.Replace("[MOD]", "");
                    break;
                case 3:
                    this.NickName.Replace("[GM]", "");
                    break;
                case 4:
                    pClient.Player.NickName.Replace("[Dev]", "");
                    break;
                case 5:
                    pClient.Player.NickName.Replace("[Admin]", "");
                    break;
                case 6:
                    pClient.Player.NickName.Replace("[ServerAdmin]", "");
                    break;
            }
        }
        public string GetPlayerDatabaseName()
        {
            string NameNoTag = this.NickName;
            switch (pClient.Player.Acces_level)
            {
                case 1:
                    NameNoTag.Replace("[Tester]", "");
                    break;
                case 2:
                    NameNoTag.Replace("[MOD]", "");
                    break;
                case 3:
                    NameNoTag.Replace("[GM]", "");
                    break;
                case 4:
                    NameNoTag.Replace("[Dev]", "");
                    break;
                case 5:
                    NameNoTag.Replace("[Admin]", "");
                    break;
                case 6:
                    NameNoTag.Replace("[ServerAdmin]", "");
                    break;
                   
            }
            return NameNoTag;
        }
        public void Details_Save()
        {
            using (DatabaseClient pClientDb = Program.DatabaseManager.GetClient())
            {
                using (var command = new MySqlCommand(details_Save))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@pUserID", this.UserID);
                    command.Parameters.AddWithValue("@pExp", this.Experience);
                    command.Parameters.AddWithValue("@pLevel", this.Level);
                    command.Parameters.AddWithValue("@pDinar", this.Dinar);
                    command.Parameters.AddWithValue("@pDeaths", this.Deaths);
                    command.Parameters.AddWithValue("@pCopons", this.Copons);
                    command.Parameters.AddWithValue("@pCash", this.Cash);
                    pClientDb.ExecuteScalar(command);
                }
            }
        }
        public void AccountInfo_Save()
        {
            using (DatabaseClient pClientDb = Program.LoginDatabaseManager.GetClient())
            {
                using (var command = new MySqlCommand(AccountInf_Save))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@pUserID", this.UserID);
                    command.Parameters.AddWithValue("@pIsOnline", Convert.ToInt16(this.IsOnline));
                    command.Parameters.AddWithValue("@pNickName", this.GetPlayerDatabaseName());
                    command.Parameters.AddWithValue("@pBanned", Convert.ToInt16(this.Banned));
                    command.Parameters.AddWithValue("@pBannTime", this.BannTime);
                    pClientDb.ExecuteScalar(command);
                }
            }
        }

        public void Save()
        {
            this.AccountInfo_Save();
            this.Details_Save();
        }
        #endregion
        public Player()
        {
            this.AccountInfo = new tUser();
            this.pInventory = new Inventory();
        }
    }
}
