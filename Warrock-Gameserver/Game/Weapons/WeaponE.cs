using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Warrock.Data;

namespace Warrock.Game.Weapons
{
    public sealed class WeaponE : Weapon
    {
        public WeaponE()
        {
            this.Type = WeaponType.WeaponE;
            this.AddDefaultSlots();
        }
        private void AddDefaultSlots()
        {
            this.Slots.Add((byte)WeaponsSlotsE.slot1E, "DA02");
            this.Slots.Add((byte)WeaponsSlotsE.slot2E, "DB01");
            this.Slots.Add((byte)WeaponsSlotsE.slot3E, "DF01");
            this.Slots.Add((byte)WeaponsSlotsE.slot4E, "DR01");
            this.Slots.Add((byte)WeaponsSlotsE.slot5E, "^");
            this.Slots.Add((byte)WeaponsSlotsE.slot6E, "^");
            this.Slots.Add((byte)WeaponsSlotsE.slot7E, "^");
            this.Slots.Add((byte)WeaponsSlotsE.slot8E, "DI05");
        }
    }
}
