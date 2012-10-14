using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Warrock.Game.Weapons
{
    public class Weapon
    {
        public Dictionary<byte, string> Slots = new Dictionary<byte, string>();
        public Data.WeaponType Type { get; internal set; }//this need to find in direcnory
        public Weapon()
        {
        }
        public string genWeaponString()
        {
            string WeaponFullString = null;
            WeaponFullString = Slots[1] + "," + Slots[2] + "," + Slots[3] + "," + Slots[4] + "," + Slots[4] + "," + Slots[5] + "," + Slots[6] + "," + Slots[7] + ",^,^";
            return WeaponFullString;
        }
    }
}
