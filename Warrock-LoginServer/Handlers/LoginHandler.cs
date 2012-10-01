using System;
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
                Row = dbclient.ReadRow("SELECT * FROM Accounts WHERE username='" + username + "'");
            }
            tUser User = tUser.ReadFromDatabase(Row);
            if (Row == null)
            {
                SendAuthResponse(LoginResponse.UserNameNotFound, pClient);
                pClient.Disconnect();
                return;
            }
            else if (User.Password != password)//password hash later
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
                    using(DatabaseClient dbClient = Program.DatabaseManager.GetClient())
                    {
                        dbClient.ExecuteQuery("UPDATE Bann_Time='0' WHERE UserID='" + User.UserID + "'");
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

            using(WRPacket p = new WRPacket((int)LoginServerOpcodes.LoginResponse))
            {
                p.addBlock(User.UserID);
                p.addBlock(0);
                p.addBlock(User.username);
                p.addBlock("nevergetit");
                p.addBlock("nickname");
                p.addBlock(0);
                p.addBlock(User.UserID);
                p.addBlock(User.UserID);
                if (User.Access_level > 0)
                {
                    p.addBlock(123);
                }
                else
                {
                    p.addBlock(0);
                }
                p.addBlock(new Random().Next(111111111, 999999999));
                p.addBlock(Managers.GameServerManager.Instance.ServerCount);
                foreach (var Server in Managers.GameServerManager.Instance.GameServers)
                {
                    ushort utilizationt = (ushort)((1 - Math.Cos(((float)Server.Value.OnlineUsers / Server.Value.PlayerLimit) * Math.PI)) * 3500);
                    p.addBlock(Server.Value.ID);
                    p.addBlock(Server.Value.ServerName);
                    p.addBlock("192.168.1.126");
                    p.addBlock(5530);
                    p.addBlock(utilizationt);
                    p.addBlock(0); //servertype
                }

                //clanshit?
                p.addBlock(-1);
                p.addBlock(-1);
                p.addBlock(-1);
                p.addBlock(-1);
                p.addBlock(-1);
                p.addBlock(-1);
                pClient.SendPacket(p);
               
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
