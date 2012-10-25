using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Warrock.Data;

namespace Warrock.Game.WeaponSets
{
    public sealed class WeaponS : WeaponSet
    {
        public WeaponS()
        {
            this.Type = WeaponSetType.WeaponSetS;
            this.AddDefaultSlots();
        }
        private void AddDefaultSlots()
        {
            this.Slots.Add((byte)WeaponsSlotsS.slot1S, new Weapon("DA02"));
            this.Slots.Add((byte)WeaponsSlotsS.slot2S, new Weapon("DB01"));
            this.Slots.Add((byte)WeaponsSlotsS.slot3S, new Weapon("DG05"));
            this.Slots.Add((byte)WeaponsSlotsS.slot4S, new Weapon("DN01"));
            this.Slots.Add((byte)WeaponsSlotsS.slot5S, new Weapon("^"));
            this.Slots.Add((byte)WeaponsSlotsS.slot6S, new Weapon("^"));
            this.Slots.Add((byte)WeaponsSlotsS.slot7S, new Weapon("^"));
            this.Slots.Add((byte)WeaponsSlotsS.slot8S, new Weapon("DC34"));
        }
    }
}
