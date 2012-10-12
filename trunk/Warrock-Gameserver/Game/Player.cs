using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using Warrock.Networking;
using Warrock.Lib;
using Warrock.Database;

namespace Warrock.Game
{
    public class Player
    {
        #region ProzedureNames
        private const string details_Save = "details_Save";
        private const string AccountInfo_Save = "AccountInfo_Save";
        #endregion
        #region Variabels
        public GameClient pClient { get;  set; }
        public Account_Details Account_Details { get; set; }
        public tUser AccountInfo { get; set; }
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
        public void Details_Save()
        {
            using (DatabaseClient pClientDb = Program.DatabaseManager.GetClient())
            {
                using (var command = new MySqlCommand(details_Save))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@pExp", this.Experience);
                    command.Parameters.AddWithValue("@pLevel", this.Level);
                    command.Parameters.AddWithValue("@pDinar", this.Dinar);
                    command.Parameters.AddWithValue("@pDeaths", this.Deaths);
                    command.Parameters.AddWithValue("@pCopons", this.Copons);
                    command.Parameters.AddWithValue("@pCash", this.Cash);
                    pClientDb.ExecuteQueryWithParameters(command);
                }
            }
        }
        public void AccountInfo_Save()
        {
            using (var command = new MySqlCommand(AccountInfo_Save))
            {
                command.Connection = new MySqlConnection(Program.LoginDbConnectionString);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@pIsOnline", this.IsOnline);
                command.Parameters.AddWithValue("@pNickName", this.NickName);
                command.Parameters.AddWithValue("@pBanned", this.Banned);
                command.Parameters.AddWithValue("@pBannTime", this.BannTime);
                command.ExecuteNonQuery();
                command.Dispose();
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
        }
        public Player(int UserID)
        {
            this.pClient = pClient;
            this.pClient.Player = this;
            this.AccountInfo = pClient.User;
        }
    }
}
