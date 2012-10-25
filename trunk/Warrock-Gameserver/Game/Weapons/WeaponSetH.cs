using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Warrock.Data;

namespace Warrock.Game.WeaponSets
{
    public sealed class WeaponSetH : WeaponSet
    {
        public WeaponSetH()
        {
            this.Type = WeaponSetType.WeaponSetH;
            this.AddDefaultSlots();
        }
        private void AddDefaultSlots()
        {
            this.Slots.Add((byte)WeaponsSlotsH.slot1H, new Weapon("DA02"));
            this.Slots.Add((byte)WeaponsSlotsH.slot2H, new Weapon("DB01"));
            this.Slots.Add((byte)WeaponsSlotsH.slot3H, new Weapon("DJ01"));
            this.Slots.Add((byte)WeaponsSlotsH.slot4H, new Weapon("DL01"));
            this.Slots.Add((byte)WeaponsSlotsH.slot5H, new Weapon("^"));
            this.Slots.Add((byte)WeaponsSlotsH.slot6H, new Weapon("^"));
            this.Slots.Add((byte)WeaponsSlotsH.slot7H, new Weapon("^"));
            this.Slots.Add((byte)WeaponsSlotsH.slot8H, new Weapon("DI05"));
        }
    }
}
