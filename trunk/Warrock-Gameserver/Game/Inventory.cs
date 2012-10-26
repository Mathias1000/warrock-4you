using Warrock.Database;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Data;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using Warrock.Game.WeaponSets;
using Warrock.Game.Costume;
using Warrock.Data;
using Warrock.Game.Item;
using Warrock.Util;

namespace Warrock.Game
{
    public class Inventory
    {
        public List<Equipment> EquipmentList { get; private set; }
        public Dictionary<pCustome, Costume.Costume> EquiptCustomes { get; private set; }
        public Dictionary<pCustome, Costume.Costume> InvCustomes { get; private set; }
        public Dictionary<WeaponSetType, WeaponSet> WeaponsSets { get; private set; }
        public ConcurrentDictionary<int, item> InventoryItems = new ConcurrentDictionary<int, item>();
        private Mutex locker = new Mutex();
        public Inventory()
        {
            InitAllLists();
            this.Release();
        }
        #region ListStuff
        private void InitAllLists()
        {
            InitCustome();
            InitWeapons();
        }
        private void InitWeapons()
        {
         
            WeaponSetA A = new WeaponSetA();
            WeaponSetE E = new WeaponSetE();
            WeaponSetH H = new WeaponSetH();
            WeaponSetM M = new WeaponSetM();
            WeaponS S = new WeaponS();
            WeaponsSets = new Dictionary<WeaponSetType, WeaponSet>();
            WeaponsSets.Add(WeaponSetType.WeaponSetA, A);
            WeaponsSets.Add(WeaponSetType.WeaponSetE, E);
            WeaponsSets.Add(WeaponSetType.WeaponSetH, H);
            WeaponsSets.Add(WeaponSetType.WeaponSetM, M);
            WeaponsSets.Add(WeaponSetType.WeaponSetS, S);
        }
        private void InitCustome()
        {

            CostumeA A = new CostumeA();
            CostumeE E = new CostumeE();
            CostumeH H = new CostumeH();
            CostumeM M = new CostumeM();
            CostumeS S = new CostumeS();
            InvCustomes = new Dictionary<pCustome, Costume.Costume>();
            EquiptCustomes = new Dictionary<pCustome, Costume.Costume>();
            EquiptCustomes.Add(pCustome.CostumeA, A);// add All Defauls Customes
            EquiptCustomes.Add(pCustome.CostumeE, E);
            EquiptCustomes.Add(pCustome.CostumeH, H);
            EquiptCustomes.Add(pCustome.CostumeM, M);
            EquiptCustomes.Add(pCustome.CostumeS, S);

        }
        #endregion
        #region GetStuff
        #region Weapon
        public WeaponSet GetWeaponByType(WeaponSetType Type)
        {
            return this.WeaponsSets[Type];
        }
        public string GetWeaponStringByType(WeaponSetType Type)
        {
            return this.WeaponsSets[Type].genWeaponString();
        }
        #endregion
        public bool hasPX(string ID)
        {
            try
            {
                foreach (pXItem pItem in this.InventoryItems.Values)
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
        public bool hasItem(string ID)
        {
            try
            {
                foreach (pXItem pItem in this.InventoryItems.Values)
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
            if (this.WeaponsSets[WeaponSetType.WeaponSetE].Slots[5].WeaponString == "^"
                && this.WeaponsSets[WeaponSetType.WeaponSetM].Slots[5].WeaponString == "^"
                && this.WeaponsSets[WeaponSetType.WeaponSetS].Slots[5].WeaponString == "^"
                && this.WeaponsSets[WeaponSetType.WeaponSetM].Slots[5].WeaponString == "^"
                && this.WeaponsSets[WeaponSetType.WeaponSetH].Slots[5].WeaponString == "^")
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
            foreach (Costume.Costume pItem in this.InvCustomes.Values)
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
            CostumeInventory.Substring(0, CostumeInventory.Length - 1);
            return CostumeInventory;
        }
        public string generateInventoryString()
        {

            string Items = "";
            foreach (item pItem in this.InventoryItems.Values)
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
            return this.EquiptCustomes[Type];
        }
        public string GetCustomStringByType(pCustome Type)
        {
            return this.EquiptCustomes[Type].genFullCustomeString();
        }
        #endregion
        #endregion
        public void LoadCustomes(long UserID)
        {
            DataTable ItemRows = null;
            DataTable InvCustiomes = null;
            using (DatabaseClient DBClient = Program.DatabaseManager.GetClient())
            {
                ItemRows = DBClient.ReadDataTable("SELECT Class,BandageCode,Equipt FROM costumes WHERE userID = '" + UserID + "' AND Equipt='1'");
                InvCustiomes = DBClient.ReadDataTable("SELECT Class,BandageCode,Equipt FROM costumes WHERE userID = '" + UserID + "' AND Equipt='0'");
            }
            if (ItemRows == null)
            {
                return;
            }
            foreach (DataRow row in ItemRows.Rows)
            {
                byte pClass = GetDataTypes.GetByte(row["Class"]);
                string BandageCode = row["BandageCode"].ToString();
                switch ((pCustome)pClass)
                {
                    case pCustome.CostumeA:
                        this.EquiptCustomes[pCustome.CostumeA].CustomeCode = BandageCode;
                        break;
                    case pCustome.CostumeE:
                        this.EquiptCustomes[pCustome.CostumeE].CustomeCode = BandageCode;
                        break;
                    case pCustome.CostumeH:
                        this.EquiptCustomes[pCustome.CostumeH].CustomeCode = BandageCode;
                        break;
                    case pCustome.CostumeM:
                        this.EquiptCustomes[pCustome.CostumeM].CustomeCode = BandageCode;
                        break;
                    case pCustome.CostumeS:
                        this.EquiptCustomes[pCustome.CostumeS].CustomeCode = BandageCode;
                        break;
                    default :
                        Log.WriteLine(LogLevel.Warn, "Unkown custome{0} by class and BandageCode {1} ", pClass, BandageCode);
                        break;
                }
            }
            foreach (DataRow row in InvCustiomes.Rows)
            {
                byte pClass = GetDataTypes.GetByte(row["Class"]);
                string BandageCode = row["BandageCode"].ToString();
                Costume.Costume Costome = new Costume.Costume();
                Costome.CustomeCode = BandageCode;
                this.InvCustomes.Add((pCustome)pClass, Costome);
            }
        }
        public void LoadPXItems(long UserID)
        {
            DataTable ItemRows = null;
            using (DatabaseClient DBClient = Program.DatabaseManager.GetClient())
            {
                ItemRows = DBClient.ReadDataTable("SELECT * FROM INVENTORY WHERE userID = '" + UserID + "' AND IsPX='1'");
            }
            if (ItemRows == null)
            {
                return;
            }
            foreach (DataRow row in ItemRows.Rows)
            {
                pXItem pItem = pXItem.LoadFromDatabase(row);
                this.InventoryItems.TryAdd(pItem.InventorySlot,pItem);
            }
        }
        public void LoadItems(long UserID)
        {
            DataTable ItemRows = null;
            using (DatabaseClient DBClient = Program.DatabaseManager.GetClient())
            {
                ItemRows = DBClient.ReadDataTable("SELECT * FROM INVENTORY WHERE userID = '" + UserID + "' AND IsPX='0'");
            }
            if (ItemRows == null)
            {
                return;
            }
            foreach (DataRow row in ItemRows.Rows)
            {
                item pItem = item.LoadFromDatabase(row);
                InventoryItems.TryAdd(pItem.InventorySlot,pItem);
            }
        }
        public void LoadEquipment(long UserID)
        {
            this.EquipmentList = new List<Equipment>();

            DataTable eqRows = null;
            using (DatabaseClient DBClient = Program.DatabaseManager.GetClient())
            {
                eqRows = DBClient.ReadDataTable("SELECT * FROM equipment WHERE userID = '" + UserID + "'");
            }
            if (eqRows == null)
            {
                return;
            }
            foreach (DataRow row in eqRows.Rows)
            {
                Equipment eq = Equipment.LoadFromDatabase(row);
                eq.UserID = UserID;
                this.EquipmentList.Add(eq);
            }
            foreach (var eq in this.EquipmentList)//set equipment from database
            {
                this.WeaponsSets[(WeaponSetType)eq.Class].Slots[eq.Slot].WeaponString = eq.itemCode;
                this.WeaponsSets[(WeaponSetType)eq.Class].Slots[eq.Slot].equip = eq;
            }
        }
        public void RemoveEquip(long UserID, byte Class, byte pSlot)
        {
            try
            {
                this.Enter();
                Equipment eq = this.EquipmentList.Find(m => m.Slot == pSlot && m.Class == Class && m.UserID == UserID);
                if (eq != null)
                {
                    eq.itemCode = "^";//in db for empty slot
                    eq.Save();
                }
            }
            finally
            {
                this.Release();
            }

        }
        public void AddEquip(Equipment eq)
        {
            try
            {
                this.Enter();
                this.EquipmentList.Add(eq);
                eq.Save();
            }
            finally
            {
                this.Release();
            }
        }
        public void AddItem(item pItem)
        {
            try
            {
                //todo add in to database
                this.InventoryItems.TryAdd(pItem.InventorySlot,pItem);
            }
            finally { this.Release(); }
        }
        public void AddPXItem(pXItem pItem)
        {
            try
            {
                this.Enter();
                //todo in to database
                this.InventoryItems.TryAdd(pItem.InventorySlot,pItem);
            }
            finally { this.Release(); }
        }
        public void RemoveItem(item pItem)
        {
            try
            {
                this.Enter();
                //todo remove into database
                item RemovedItem;
                this.InventoryItems.TryRemove(pItem.InventorySlot, out RemovedItem);
            }
            finally { this.Release(); }
        }
        public bool GetFreeSlot(out byte pSlot)
        {
            pSlot = 0;
            IEnumerable<int> keyRange = Enumerable.Range(0, 30);
            var freeKeys = keyRange.Except(this.InventoryItems.Keys);
            if (freeKeys.Count() == 0)
                return false; // no free slot

            pSlot = (byte)freeKeys.First();
            return true;
        }
        public void RemovePXItem(pXItem pItem)
        {
            try
            {
                //todo remove into database
                item Removoed;
                this.InventoryItems.TryRemove((int)pItem.InventorySlot,out Removoed);
            }
            finally { this.Release(); }
        }
        public void Release()
        {
            try
            {
                locker.ReleaseMutex();
            }
            catch { }
        }
        public void Enter()
        {
            locker.WaitOne();
        }
    }
}
