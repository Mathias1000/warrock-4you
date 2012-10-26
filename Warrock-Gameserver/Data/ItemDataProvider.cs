using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Warrock.Util;
using Warrock.Database;
using System.Data;
using Warrock.Game.WeaponSets;
using Warrock.Game.Costume;

namespace Warrock.Data
{
    [ServerModule(Util.InitializationStage.DataStore)]
    public class ItemDataProvider
    {
        public Dictionary<string, Weapon> WeaponsByCode { get; private set; }
        public Dictionary<byte, List<Costume>> Costume { get; private set; }
        public static ItemDataProvider Instance { get; private set; }
        [InitializerMethod]
        public static bool Load()
        {

            Instance = new ItemDataProvider()
            {
            };
            Log.WriteLine(LogLevel.Info, "ItemDataProvider initialized.");
            return true;
        }
        public ItemDataProvider()
        {
            LoadItems();
            LoadCostome();
        }
        private void LoadCostome()
        {
            int count = 0;
            this.Costume = new Dictionary<byte, List<Game.Costume.Costume>>();
            List<Costume> A = new List<Costume>();
            List<Costume> E = new List<Costume>();
            List<Costume> H = new List<Costume>();
            List<Costume> M = new List<Costume>();
            List<Costume> S = new List<Costume>();
            DataTable CostumeTab = null;
            using (DatabaseClient DBclient = Program.DatabaseManager.GetClient())
            {
                CostumeTab = DBclient.ReadDataTable("SELECT * FROM Costume_Data");
            }
            if (CostumeTab != null)
            {
                foreach (DataRow row in CostumeTab.Rows)
                {
                    byte Class = GetDataTypes.GetByte(row["Class"]);
                    string CostumeCode = row["Code"].ToString();
                    Costume pCostume = new Costume
                    {
                        Class = Class,
                        CustomeCode = CostumeCode,
                    };
                    switch ((pCustome)Class)
                    {
                        case pCustome.CostumeA:
                            A.Add(pCostume);
                            break;
                        case pCustome.CostumeE:
                            E.Add(pCostume);
                            break;
                        case pCustome.CostumeH:
                            H.Add(pCostume);
                            break;
                        case pCustome.CostumeS:
                            S.Add(pCostume);
                            break;
                        case pCustome.CostumeM:
                            H.Add(pCostume);
                            break;
                        default:
                            Log.WriteLine(LogLevel.Warn, "Unkown CostomeClass {0}", CostumeCode);
                            break;
                    }
                    count++;
                }
                this.Costume.Add(0, E);
                this.Costume.Add(1, M);
                this.Costume.Add(2, S);
                this.Costume.Add(3, A);
                this.Costume.Add(4, H);
            }
        }
        public bool GetCostome(out Costume costume,byte Class,string Code)
        {
            costume = null;
            List<Costume> costumes;
            if (this.Costume.TryGetValue(Class, out costumes))
            {
                costume = costumes.Find(m => m.CustomeCode == m.CustomeCode);
                if (costume == null)
                {
                    return false;
                }
                return true;
            }
            return false;
        }
        public bool GetWeaponByCode(out Weapon Weapon,string ItemCode)
        {
            Weapon W;
            if (this.WeaponsByCode.TryGetValue(ItemCode, out W))
            {
                Weapon = W;
                return true;
            }
            Weapon = null;
            return false;
        }
        private void LoadItems()
        {
            try
            {
                WeaponsByCode = new Dictionary<string, Weapon>();
                DataTable ItemTab = null;
                using (DatabaseClient DBclient = Program.DatabaseManager.GetClient())
                {
                    ItemTab = DBclient.ReadDataTable("SELECT * FROM item_data");
                }
                if (ItemTab != null)
                {
                    foreach (DataRow row in ItemTab.Rows)
                    {
                        Data.Weapon W = Data.Weapon.LoadFromDatabase(row);
                        this.WeaponsByCode.Add(W._WeaponCode, W);
                    }
                    Log.WriteLine(LogLevel.Info, "Load {0} Weapons From Database",this.WeaponsByCode.Count);
                }
            }
            catch (Exception ex)
            {
                Log.WriteLine(LogLevel.Error, "Failed Load Weapons {0}", ex.ToString());
            }
        }
    }
}
