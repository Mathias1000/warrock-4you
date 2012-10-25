using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Warrock.Game.WeaponSets
{
    public class WeaponSet
    {
        public Dictionary<byte, Weapon> Slots = new Dictionary<byte, Weapon>();
        public Data.WeaponSetType Type { get; internal set; }//this need to find in direcnory
        public WeaponSet()
        {
        }
        public string genWeaponString()
        {
            string WeaponFullString = null;
            WeaponFullString = Slots[1].WeaponString + "," + Slots[2].WeaponString + "," + Slots[3].WeaponString + "," + Slots[4].WeaponString + "," + Slots[5].WeaponString + "," + Slots[6].WeaponString + "," + Slots[7].WeaponString + "," + Slots[8].WeaponString + ",^,^";
            return WeaponFullString;
        }
    }
}
