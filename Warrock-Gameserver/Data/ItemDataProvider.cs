using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Warrock.Util;
using Warrock.Database;
using System.Data;
using Warrock.Game.WeaponSets;

namespace Warrock.Data
{
    [ServerModule(Util.InitializationStage.DataStore)]
    public class ItemDataProvider
    {
        public Dictionary<ushort, Weapon> Weapons { get; private set; }
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
          //  LoadItems(); need later to check weapon and buy weapon..
        }
        private void LoadItems()
        {
            try
            {
                Weapons = new Dictionary<ushort, Weapon>();
                DataTable ItemTab = null;
                using (DatabaseClient DBclient = Program.DatabaseManager.GetClient())
                {
                    ItemTab = DBclient.ReadDataTable("SELECT * FROM Items");
                }
                if (ItemTab != null)
                {
                    foreach (DataRow row in ItemTab.Rows)
                    {
                        Weapon Weapon = Weapon.LoadFromDatabase(row);
                        Weapons.Add(Weapon.WeaponID, Weapon);
                    }
                    Log.WriteLine(LogLevel.Info, "Load {0} Weapons From Database",Weapons.Count);
                }
            }
            catch (Exception ex)
            {
                Log.WriteLine(LogLevel.Error, "Failed Load Weapons {0}", ex.ToString());
            }
        }
    }
}
