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
using Warrock.Game.WeaponSets;

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
           if (User.UserID == UserID)
           {
             
               pClient.Player = new Player();
               pClient.Player.Account_Details = User_Details;
               pClient.Player.AccountInfo = User;
               pClient.AccountInfo = User;
    
               pClient.Player.pClient = pClient;
               pClient.Player.pInventory.LoadItems(UserID);
               pClient.Player.pInventory.LoadPXItems(UserID);
               pClient.Player.pInventory.LoadCustomes(UserID);
               pClient.Player.pInventory.LoadEquipment(UserID);
               pClient.Authenticated = true;
               pClient.Player.IsInLobby = true;
               pClient.uniqID2 = pClient.AccountInfo.UserID + 1;
               PacketHelper.SendPlayerInfo(pClient);
               pClient.Player.SetPlayerTag();
               ClientManager.Instance.AddClient(pClient);//register client to server
               PlayerManager.Instance.InvokeLoggetIn(pClient.Player);

               //hier datenpacket for loppy login
           }
       }
       [PacketHandler((int)ClientGameOpcode.ItemShopBuyItem)]
       public static void HandleBuyItem(GameClient pClient, WRPacket pPacket)
       {
           string WeaponCode = pPacket.ReadString(3);
           Data.Weapon BuyWeapon;
           if (!Data.ItemDataProvider.Instance.GetWeaponByCode(out BuyWeapon, WeaponCode))
           {
               PacketHelper.SendMessage(pClient, "Invalid BuyWeapon");
               return;
           }
           byte Period = pPacket.ReadByte(6);
           int Price = BuyWeapon.prices[pPacket.ReadByte(6)];
           int[] convertDays = new int[4] { 3, 7, 15, 30 };
           if (BuyWeapon.validShopItem && Price >= 0 && !pClient.Player.pInventory.hasItem(WeaponCode))
           {
               if (BuyWeapon._isPremium && pClient.Player.Premium <= 0)
               {
                   PacketHelper.SendItemShopError(pClient, Data.ItemShopErr.PremiumOnly);
               }
               else
               {
                   int Dinar = pClient.Player.Dinar -= Price;
                   long StartTime = Program.currTimeStamp + (86400 * convertDays[Period]);
                   if (Dinar >= 0)
                   {

                       if (!BuyWeapon._isValidPx)
                       { 
                           byte pSlot;
                           if (!pClient.Player.pInventory.GetFreeSlot(out pSlot))
                           {
                               PacketHelper.SendMessage(pClient, "Inventory is Full");
                           }
                           Game.Item.item ii = new Game.Item.item
                           {
                               itemCode = BuyWeapon._WeaponCode,
                               expireDate = StartTime,//later when premium system imp
                               InventorySlot = (byte)pSlot,
                               Class = BuyWeapon.Class,
                           };
                           ii.Save(pClient.Player.UserID);
                           pClient.Player.pInventory.AddItem(ii);
                           pClient.Player.Dinar = Dinar;
                           ii.Save(pClient.Player.UserID);
                           pClient.Player.Details_Save();
                           PacketHelper.SendItemShopResultSuccess(pClient,ii);
                       }
                       else
                       {
                           byte pSlot;
                           if (!pClient.Player.pInventory.GetFreeSlot(out pSlot))
                           {
                               PacketHelper.SendMessage(pClient, "Inventory is Full");
                           }
                           Game.Item.pXItem ii = new Game.Item.pXItem
                           {
                               itemCode = BuyWeapon._WeaponCode,
                               expireDate = StartTime,
                               InventorySlot = (byte)pSlot,
                               Class = BuyWeapon.Class,
                           };
                           ii.Save(pClient.Player.UserID);
                           pClient.Player.pInventory.AddItem(ii);
                           pClient.Player.Dinar = Dinar;
                           ii.Save(pClient.Player.UserID);
                           pClient.Player.Details_Save();
                           PacketHelper.SendItemShopResultSuccess(pClient, ii);
                       }
                   }
                   else
                   {
                       PacketHelper.SendItemShopError(pClient, Data.ItemShopErr.NotEnoughDinar);
                   }
               }
           }
           else
           {
               PacketHelper.SendItemShopError(pClient, Data.ItemShopErr.CannotBeBougth);
           }
       }
       [PacketHandler((int)ClientGameOpcode.ChangeEquip)]
       public static void ChangeEquip(GameClient pClient, WRPacket pPacket)
       {
           bool Equip = (pPacket.ReadByte(2) == 0) ? true : false;
           byte Class = pPacket.ReadByte(3);
           if (Equip)
           {
               int TargetSlot = pPacket.ReadInt(7);
               string ItemID = pPacket.ReadString(6);//todo check user has item


               if ((ItemID == "DA02" && TargetSlot == 0) || (ItemID == "DB01" && TargetSlot == 1) || (ItemID == "DF01" && TargetSlot == 2 && (Class == 0 || Class == 1)) || (ItemID == "DG05" && TargetSlot == 2 && Class == 2) || (ItemID == "DC02" && TargetSlot == 2 && Class == 3) || (ItemID == "DJ01" && TargetSlot == 2 && Class == 4)
                || (ItemID == "DR01" && Class == 0 && TargetSlot == 3) || (ItemID == "DQ01" && Class == 1 && TargetSlot == 3) || (ItemID == "DN01" && (Class == 2 || Class == 3) && TargetSlot == 3) || (ItemID == "DL01" && Class == 4 && TargetSlot == 3))
               {
                   pClient.Player.pInventory.WeaponsSets[(Data.WeaponSetType)Class].Slots[(byte)TargetSlot].WeaponString = ItemID;
                   Equipment eq = new Equipment
                   {
                       Class = Class,
                       Slot = (byte)TargetSlot,
                       UserID = pClient.Player.UserID,
                       itemCode = ItemID,
                   };
                   pClient.Player.pInventory.AddEquip(eq);
                   string Weapon = pClient.Player.pInventory.WeaponsSets[(Data.WeaponSetType)Class].genWeaponString();
                   PacketHelper.EquipmentItem(pClient, Class, Weapon);
               }
           }
           else
           {
               int TargetSlot = pPacket.ReadInt(5);
               string ItemID = pPacket.ReadString(6);//todo check user has item


               if (pClient.Player.pInventory.WeaponsSets[(Data.WeaponSetType)Class].Slots[(byte)TargetSlot].WeaponString == ItemID)
               {
                   if (pClient.Player.pInventory.WeaponsSets[(Data.WeaponSetType)Class].Slots[(byte)TargetSlot].WeaponString != "^")
                   {
                       pClient.Player.pInventory.WeaponsSets[(Data.WeaponSetType)Class].Slots[(byte)TargetSlot].WeaponString = "^";
                       pClient.Player.pInventory.RemoveEquip(pClient.Player.UserID, Class, (byte)TargetSlot);
                       string Weapon = pClient.Player.pInventory.WeaponsSets[(Data.WeaponSetType)Class].genWeaponString();
                       
                       PacketHelper.EquipmentItem(pClient, Class, Weapon);
                   }
               }
               else
               {
                   //cheating?
               }
           }
       }
    }
}
