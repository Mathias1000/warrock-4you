using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Warrock.Data;

namespace Warrock.Game.Weapons
{
    public sealed class WeaponA : Weapon
    {
        public WeaponA()
        {
            this.AddDefaultSlots();
        }
        private void AddDefaultSlots()
        {
            this.Slots.Add((byte)WeaponsSlotsA.slot1A, "DA02");
            this.Slots.Add((byte)WeaponsSlotsA.slot2A, "DB01");
            this.Slots.Add((byte)WeaponsSlotsA.slot3A, "DC02");
            this.Slots.Add((byte)WeaponsSlotsA.slot4A, "DN01");
            this.Slots.Add((byte)WeaponsSlotsA.slot5A, "^");
            this.Slots.Add((byte)WeaponsSlotsA.slot6A, "^");
            this.Slots.Add((byte)WeaponsSlotsA.slot7A, "^");
            this.Slots.Add((byte)WeaponsSlotsA.slot8A, "DT06");
        }
    }
}
