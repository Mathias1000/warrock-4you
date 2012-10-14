using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Warrock.Data;

namespace Warrock.Game.Weapons
{
    public sealed class WeaponS : Weapon
    {
        public WeaponS()
        {
            this.Type = WeaponType.WeaponS;
            this.AddDefaultSlots();
        }
        private void AddDefaultSlots()
        {
            this.Slots.Add((byte)WeaponsSlotsS.slot1S, "DA02");
            this.Slots.Add((byte)WeaponsSlotsS.slot2S, "DB01");
            this.Slots.Add((byte)WeaponsSlotsS.slot3S, "DG05");
            this.Slots.Add((byte)WeaponsSlotsS.slot4S, "DN01");
            this.Slots.Add((byte)WeaponsSlotsS.slot5S, "^");
            this.Slots.Add((byte)WeaponsSlotsS.slot6S, "^");
            this.Slots.Add((byte)WeaponsSlotsS.slot7S, "^");
            this.Slots.Add((byte)WeaponsSlotsS.slot8S, "DC34");
        }
    }
}
