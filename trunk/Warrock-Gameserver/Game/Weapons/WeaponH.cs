using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Warrock.Data;

namespace Warrock.Game.Weapons
{
    public sealed class WeaponH : Weapon
    {
        public WeaponH()
        {
            this.Type = WeaponType.WeaponH;
            this.AddDefaultSlots();
        }
        private void AddDefaultSlots()
        {
            this.Slots.Add((byte)WeaponsSlotsH.slot1H, "DA02");
            this.Slots.Add((byte)WeaponsSlotsH.slot2H, "DB01");
            this.Slots.Add((byte)WeaponsSlotsH.slot3H, "DJ01");
            this.Slots.Add((byte)WeaponsSlotsH.slot4H, "DL01");
            this.Slots.Add((byte)WeaponsSlotsH.slot5H, "^");
            this.Slots.Add((byte)WeaponsSlotsH.slot6H, "^");
            this.Slots.Add((byte)WeaponsSlotsH.slot7H, "^");
            this.Slots.Add((byte)WeaponsSlotsH.slot8H, "DI05");
        }
    }
}
