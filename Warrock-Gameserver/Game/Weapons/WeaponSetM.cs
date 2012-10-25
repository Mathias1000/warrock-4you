using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Warrock.Data;

namespace Warrock.Game.WeaponSets
{
    public class WeaponSetM : WeaponSet
    {
        public WeaponSetM()
        {
            this.Type = WeaponSetType.WeaponSetM;
            this.AddDefaultSlots();
        }
        private void AddDefaultSlots()
        {
            this.Slots.Add((byte)WeaponsSlotsM.slot1M, new Weapon("DA02"));
            this.Slots.Add((byte)WeaponsSlotsM.slot2M, new Weapon("DB01"));
            this.Slots.Add((byte)WeaponsSlotsM.slot3M, new Weapon("DF01"));
            this.Slots.Add((byte)WeaponsSlotsM.slot4M,new Weapon("DQ01"));
            this.Slots.Add((byte)WeaponsSlotsM.slot5M, new Weapon("^"));
            this.Slots.Add((byte)WeaponsSlotsM.slot6M, new Weapon("^"));
            this.Slots.Add((byte)WeaponsSlotsM.slot7M, new Weapon("^"));
            this.Slots.Add((byte)WeaponsSlotsM.slot8M, new Weapon("DG24"));
        }
    }
}
