using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Warrock.Data;
using System.Data;
using Warrock.Database;
using MySql.Data.MySqlClient;

namespace Warrock.Game.WeaponSets
{
    public class Weapon
    {
        public string WeaponString { get; set; }
        public WeaponSetType Weaponclass { get; set; }
        public byte WeaponSlot { get; set; }
        public Equipment equip { get; set; }
        public Weapon(string WeaponString)
        {
            this.WeaponString = WeaponString;
        }
    }
}
