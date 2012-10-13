using System;
using MySql.Data.MySqlClient;
using Warrock.Util;
using Warrock_Lib.Networking;
using Warrock_LoginServer.Networking;
using Warrock_Lib;
using Warrock.Database;
using Warrock.Lib;
using System.Data;

namespace Warrock_LoginServer.Handlers
{
    public sealed class LoginHandler
    {
        [PacketHandler((int)ClientLoginOpcodes.Login)]
        public static void Login(LoginClient pClient, WRPacket pPacket)
        {
            string password = pPacket.ReadString(5);
            string username = pPacket.ReadString(4);
            DataRow Row = null;

            using (DatabaseClient dbclient = Program.DatabaseManager.GetClient())
            {
                Row = dbclient.ReadRow("SELECT *FROM Accounts WHERE username='" + username + "'");
            }
            if (Row == null)
            {
                SendAuthResponse(LoginResponse.UserNameNotFound, pClient);
                pClient.Disconnect();
                return;
            }
           tUser User = tUser.ReadFromDatabase(Row);
            if (User.Password != password)//password hash later
            {
                SendAuthResponse(LoginResponse.WrongPassword, pClient);
                pClient.Disconnect();
                return;
            }
            else if (User.Banned)
            {
                SendAuthResponse(LoginResponse.Banned, pClient);
                pClient.Disconnect();
                return;
            }
            else if (User.IsOnline)
            {
                SendAuthResponse(LoginResponse.AlreadyLogged, pClient);
                pClient.Disconnect();
                return;
            }
            else if (User.BannTime > 0)
            {
                long nowTime = DateTime.Now.ToFileTime();
                if (nowTime < User.BannTime)
                {
                    SendAuthResponse(LoginResponse.Banned, pClient);
                    pClient.Disconnect();
                    return;
                }
                else
                {
                    using (DatabaseClient ccClient = Program.DatabaseManager.GetClient())
                    {
                        using (var command = new MySqlCommand("unban_User"))
                        {
                            command.CommandType = System.Data.CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@pUserID", User.UserID);
                            ccClient.ExecuteScalar(command);
                        }
                    }
                    SendAuthResponse(LoginResponse.Banned, pClient);
                    pClient.Disconnect();
                    return;
                }

            }
            if (User.NickName == "")
            {
                SendAuthResponse(LoginResponse.ChangeNickName, pClient);
                return;
            }
            pClient.User = User;
            pClient.Admin = User.Access_level;
            pClient.IsAuthenticated = true;
            pClient.AccountID = User.UserID;

            SendServerList(pClient);
        }
        public static void SendServerList(LoginClient pClient)
        {
            using (var pack = new WRPacket((int)LoginServerOpcodes.LoginResponse))
            {
                
                pack.addBlock(1);
                pack.addBlock(pClient.User.UserID);
                pack.addBlock(0);
                pack.addBlock(pClient.User.username);
                pack.addBlock(pClient.User.Password);
                pack.addBlock(pClient.User.NickName);
                pack.addBlock(0);
                pack.addBlock(Convert.ToInt16(pClient.User.isover18)); // Client.getAge() -> 1 = over 18 , 0 = 14 or Younger
                pack.addBlock(0); // UDP Session :(TODO)
                if (pClient.User.Access_level > 2)
                {
                    pack.addBlock(123); // Spectate and Yellow Font
                }
                else
                {
                    pack.addBlock(0);
                }
                pack.addBlock(pClient.User.Password); // SessionKey / PassPort

                pack.addBlock(Managers.GameServerManager.Instance.ServerCount);
                foreach (var Server in Managers.GameServerManager.Instance.GameServers)
                {
                    ushort utilizationt = (ushort)((1 - Math.Cos(((float)Server.Value.OnlineUsers / Server.Value.PlayerLimit) * Math.PI)) * 3500);
                    pack.addBlock(Server.Value.ID);
                    pack.addBlock(Server.Value.ServerName);
                    pack.addBlock(Server.Value.IP);
                    pack.addBlock(Server.Value.Port);
                    pack.addBlock(utilizationt);
                    pack.addBlock(0); //servertype 1 = Aduilt 0 = eninty
                }
                //Clan shit here :(TODO)
                pack.addBlock(-1);
                pack.addBlock(-1);
                pack.addBlock(-1);
                pack.addBlock(-1);
                pack.addBlock(-1);
                pack.addBlock(-1);
                pClient.SendPacket(pack);
            }
        }
        public static void SendAuthResponse(LoginResponse Code, LoginClient pClient)
        {
            using (var p = new WRPacket(4352))
            {
                p.addBlock((int)Code);
                pClient.SendPacket(p);
            }
        }
        [PacketHandler((int)ClientLoginOpcodes.PatchRequest)]
        public static void VersionInfo(LoginClient pClient, WRPacket pPacket)
        {
            using (var pack = new WRPacket((int)LoginServerOpcodes.SendPatchVersion))
            {
                pack.addBlock(0);
                pack.addBlock(19);
                pack.addBlock(33);
                pack.addBlock(47);
                pack.addBlock(0);
                pack.addBlock(0);
                pack.addBlock("http://patch.warrock.net/k2network/warrock/");
                pClient.SendPacket(pack);
            }
        }
    }
}
