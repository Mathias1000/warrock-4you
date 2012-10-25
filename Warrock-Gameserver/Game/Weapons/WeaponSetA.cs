using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Warrock.Data;

namespace Warrock.Game.WeaponSets
{
    public sealed class WeaponSetA : WeaponSet
    {
        public WeaponSetA()
        {
            this.Type = WeaponSetType.WeaponSetA;
            this.AddDefaultSlots();
        }
        private void AddDefaultSlots()
        {
            this.Slots.Add((byte)WeaponsSlotsA.slot1A, new Weapon("DA02"));
            this.Slots.Add((byte)WeaponsSlotsA.slot2A, new Weapon("DB01"));
            this.Slots.Add((byte)WeaponsSlotsA.slot3A, new Weapon("DC02"));
            this.Slots.Add((byte)WeaponsSlotsA.slot4A, new Weapon("DN01"));
            this.Slots.Add((byte)WeaponsSlotsA.slot5A, new Weapon("^"));
            this.Slots.Add((byte)WeaponsSlotsA.slot6A, new Weapon("^"));
            this.Slots.Add((byte)WeaponsSlotsA.slot7A, new Weapon("^"));
            this.Slots.Add((byte)WeaponsSlotsA.slot8A, new Weapon("DT06"));
        }
    }
}
