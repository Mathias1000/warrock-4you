using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Warrock.Data;

namespace Warrock.Game.WeaponSets
{
    public sealed class WeaponSetE : WeaponSet
    {
        public WeaponSetE()
        {
            this.Type = WeaponSetType.WeaponSetE;
            this.AddDefaultSlots();
        }
        private void AddDefaultSlots()
        {
            this.Slots.Add((byte)WeaponsSlotsE.slot1E, new Weapon("DA02"));
            this.Slots.Add((byte)WeaponsSlotsE.slot2E, new Weapon("DB01"));
            this.Slots.Add((byte)WeaponsSlotsE.slot3E, new Weapon("DF01"));
            this.Slots.Add((byte)WeaponsSlotsE.slot4E, new Weapon("DR01"));
            this.Slots.Add((byte)WeaponsSlotsE.slot5E, new Weapon("^"));
            this.Slots.Add((byte)WeaponsSlotsE.slot6E, new Weapon("^"));
            this.Slots.Add((byte)WeaponsSlotsE.slot7E, new Weapon("^"));
            this.Slots.Add((byte)WeaponsSlotsE.slot8E, new Weapon("DI05"));
        }
    }
}
