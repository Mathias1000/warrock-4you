using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Warrock.Util;
using Warrock.Lib.Networking;
using Warrock.Networking;
using Warrock.Database;
using Warrock.Lib;
using Warrock.Game;


namespace Warrock.Handlers
{
   public sealed class CharacterHandler
    {
       [PacketHandler((int)ClientGameOpcode.GetPlayerInfo)]
       public static void GetPlayer(GameClient pClient, WRPacket pPacket)
       {
           long UserID = pPacket.ReadLong(2);
           pClient.uniqIDisCRC = pPacket.ReadInt(6);
           string Passport = pPacket.ReadString(16);
           DataRow AccountRow = null;
           DataRow AccountDetailsRow = null;
           using (DatabaseClient DbClient = Program.LoginDatabaseManager.GetClient())
           {
            
               AccountRow = DbClient.ReadRow("SELECT * FROM Accounts WHERE UserID='" + UserID + "'");
           }
           using(DatabaseClient DbClient = Program.DatabaseManager.GetClient())
           {
               AccountDetailsRow = DbClient.ReadRow("SELECT * FROM Account_Details WHERE UserID='" + UserID + "'");
           }
           if (AccountDetailsRow == null&& AccountRow != null)
           {
               //Todo Insert When Failed?
               return;
           }
           if (AccountRow == null || AccountDetailsRow == null)
           {
               return;
           }
           tUser User = tUser.ReadFromDatabase(AccountRow);
           Account_Details User_Details = Account_Details.LoadAccountDetailsFromDataBase(AccountDetailsRow);
           
           if (User == null || User_Details == null){return; }
           if (User.Password == Passport)
           {
             
               pClient.Player = new Player();
               pClient.Player.Account_Details = User_Details;
               pClient.Player.AccountInfo = User;
               pClient.AccountInfo = User;
    
               pClient.Player.pClient = pClient;
               pClient.Player.pInventory.LoadItems(UserID);
               pClient.Player.pInventory.LoadPXItems(UserID);
               pClient.Player.pInventory.LoadCustomes(UserID);
               pClient.Authenticated = true;
               pClient.Player.IsInLobby = true;
               PacketHelper.SendPlayerInfo(pClient);
               pClient.Player.SetPlayerTag();
               ClientManager.Instance.AddClient(pClient);//register client to server
               PlayerManager.Instance.InvokeLoggetIn(pClient.Player);

               //hier datenpacket for loppy login
           }
       }
    }
}
