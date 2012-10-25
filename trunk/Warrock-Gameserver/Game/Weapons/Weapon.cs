using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Warrock.Game.WeaponSets
{
    public class Weapon
    {
        public string WeaponString { get; set; }

        public Weapon(string WeaponString)
        {
            this.WeaponString = WeaponString;
        }
    }
}
