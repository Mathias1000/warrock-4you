using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Warrock.Data;
using System.Data;
using Warrock.Database;

namespace Warrock.Game.WeaponSets
{
    public class Weapon
    {
        public string WeaponString { get; set; }

        public string WeaponName { get; set; }
        public int WeaponDamage { get; set; }
        public ushort Wid { get; set; }
        public WeaponSetType Weaponclass { get; set; }
        public byte WeaponSlot { get; set; }
        public ushort WeaponID { get; set; }
        public int Price { get; set; }

        public Weapon(string WeaponString)
        {
            this.WeaponString = WeaponString;
        }
        public Weapon()
        {
        }
        public static Weapon LoadFromDatabase(DataRow row)
        {
            Weapon W = new Weapon
            {
                //WeaponID = GetDataTypes.GetUshort(row["ID"]),
                //todo
            };
            return W;
        }
    }
}
