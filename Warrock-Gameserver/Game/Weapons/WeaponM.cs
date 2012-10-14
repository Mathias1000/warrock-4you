using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Warrock.Data;

namespace Warrock.Game.Weapons
{
    public class WeaponM : Weapon
    {
        public WeaponM()
        {
            this.Type = WeaponType.WeaponM;
            this.AddDefaultSlots();
        }
        private void AddDefaultSlots()
        {
            this.Slots.Add((byte)WeaponsSlotsM.slot1M, "DA02");
            this.Slots.Add((byte)WeaponsSlotsM.slot2M, "DB01");
            this.Slots.Add((byte)WeaponsSlotsM.slot3M, "DF01");
            this.Slots.Add((byte)WeaponsSlotsM.slot4M, "DQ01");
            this.Slots.Add((byte)WeaponsSlotsM.slot5M, "^");
            this.Slots.Add((byte)WeaponsSlotsM.slot6M, "^");
            this.Slots.Add((byte)WeaponsSlotsM.slot7M, "^");
            this.Slots.Add((byte)WeaponsSlotsM.slot8M, "DG24");
        }
    }
}
