using Warrock.Database;
using System.Collections.Generic;
using System.Data;
using System;
using System.Text;
using Warrock.Game.Weapons;
using Warrock.Game.Costume;
using Warrock.Data;
using Warrock.Game.Item;

namespace Warrock.Game
{
    public class Inventory
    {
   
        public Dictionary<pCustome, Costume.Costume> Customes { get; private set; }
        public Dictionary<WeaponType, Weapon> Weapons { get; private set; }
        public List<item> InventoryItems = new List<item>();
        public List<pXItem> InventoryPXItems = new List<pXItem>();
        public Inventory()
        {
            InitAllLists();
        }
        #region ListStuff
        private void InitAllLists()
        {
            InitCustome();
            InitWeapons();
        }
        private void InitWeapons()
        {
         
            WeaponA A = new WeaponA();
            WeaponE E = new WeaponE();
            WeaponH H = new WeaponH();
            WeaponM M = new WeaponM();
            WeaponS S = new WeaponS();
            Weapons = new Dictionary<WeaponType, Weapon>();
            Weapons.Add(WeaponType.WeaponA,A);
            Weapons.Add(WeaponType.WeaponE,E);
            Weapons.Add(WeaponType.WeaponH,H);
            Weapons.Add(WeaponType.WeaponM,M);
            Weapons.Add(WeaponType.WeaponS,S);
        }
        private void InitCustome()
        {

            CostumeA A = new CostumeA();
            CostumeE E = new CostumeE();
            CostumeH H = new CostumeH();
            CostumeM M = new CostumeM();
            CostumeS S = new CostumeS();
            Customes = new Dictionary<pCustome, Costume.Costume>();
            Customes.Add(pCustome.CostumeA, A);// add All Defauls Customes
            Customes.Add(pCustome.CostumeE, E);
            Customes.Add(pCustome.CostumeH, H);
            Customes.Add(pCustome.CostumeM, M);
            Customes.Add(pCustome.CostumeS, S);

        }
        #endregion
        #region GetStuff
        #region Weapon
        public Weapon GetWeaponByType(WeaponType Type)
        {
            return this.Weapons[Type];
        }
        public string GetWeaponStringByType(WeaponType Type)
        {
            return this.Weapons[Type].genWeaponString();
        }
        #endregion
        public string getOpenSlots()
        {
            StringBuilder SB = new StringBuilder();
            if (hasPX("CA01"))
            {
                SB.Append("T,");
            }
            else
            {
                SB.Append("F,");
            }
            if (this.Weapons[WeaponType.WeaponE].Slots[5] == "^"
                && this.Weapons[WeaponType.WeaponM].Slots[5] == "^"
                && this.Weapons[WeaponType.WeaponS].Slots[5] == "^"
                && this.Weapons[WeaponType.WeaponM].Slots[5] == "^"
                && this.Weapons[WeaponType.WeaponH].Slots[5] == "^")
            {
                SB.Append("F,F,");
            }
            else
            {
                SB.Append("T,F,");
            }
            if (hasPX("CA04"))
            {
                SB.Append("T");
            }
            else
            {
                SB.Append("F");
            }
            return SB.ToString();
        }
        public string generateCustomeString()
        {
            int cCount = 0;
            string CostumeInventory = "";
            foreach (Costume.Costume pItem in this.Customes.Values)
            {
                {
                    CostumeInventory += pItem.CustomeCode + "-3-0-" + pItem.expireDate.ToString() + "-0-0-0-0-0-9999-9999,";
                    cCount++;
                }
            }

            for (int i = 0; i < (31 - cCount); i++)
            {
                CostumeInventory += "^,";

            }
            return CostumeInventory;
        }
        public string generateInventoryString()
        {

            string Items = "";

            foreach (item pItem in this.InventoryItems)
            {
                Items += pItem.itemCode + "-1-3-" + pItem.expireDate.ToString() + "-0-0-0-0-0-9999-9999,";//-9999-9999
            }

            for (int i = 0; i < (31 - this.InventoryItems.Count); i++)
            {
                Items += "^,";
            }

            Items = Items.Substring(0, Items.Length - 1);

            return Items; // Inventory (Weapons/PX-Items)

        }
        #region Custome
        public Costume.Costume GetCustomeByType(pCustome Type)
        {
            return this.Customes[Type];
        }
        public string GetCustomStringByType(pCustome Type)
        {
            return this.Customes[Type].genFullCustomeString();
        }
        #endregion
        #endregion
        public bool hasPX(string ID)
        {
            try
            {
                foreach (pXItem pItem in this.InventoryPXItems)
                {
                    if (pItem.itemCode.Contains(ID))
                    {
                        return true;
                    }
                }
            }
            catch { }
            return false;
        }
        public void LoadPXItems(int UserID)
        {
            DataTable ItemRows = null;
            using (DatabaseClient DBClient = Program.DatabaseManager.GetClient())
            {
                ItemRows = DBClient.ReadDataTable("SELECT itemCode, expireDate,Class,BandageSlot FROM INVENTORY WHERE userID = '" + UserID + "' AND");
            }
            if (ItemRows == null)
            {
                return;
            }
            foreach (DataRow row in ItemRows.Rows)
            {
                pXItem pItem = pXItem.LoadFromDatabase(row);
                this.InventoryPXItems.Add(pItem);
            }
        }
        public void LoadItems(int UserID)
        {
            DataTable ItemRows = null;
            using (DatabaseClient DBClient = Program.DatabaseManager.GetClient())
            {
                ItemRows = DBClient.ReadDataTable("SELECT itemCode, expireDate,Class,BandageSlot FROM INVENTORY WHERE userID = '" + UserID + "'");
            }
            if (ItemRows == null)
            {
                return;
            }
            foreach (DataRow row in ItemRows.Rows)
            {
                item pItem = item.LoadFromDatabase(row);
                InventoryItems.Add(pItem);
            }
        }
    }
}
