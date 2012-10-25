using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Warrock.Data
{
    public struct tItem
    {
        public string itemCode { get;  set; }
        public long expireDate { get; set; }
        public int SlotCostume { get; set; }
        public int SlotEngineer { get; set; }
        public int SlotMedic { get; set; }
        public int SlotSniper { get; set; }
        public int SlotAssault { get; set; }
        public int SlotHeavyTrooper { get; set; }
    }
    public struct bItem
    {

        public string WaffenName { get;  set; }
        public int Level { get;  set; }
        public int Tage3 { get; set; }
        public int Tage7 { get; set; }
        public int Tage15 { get; set; }
        public int Tage30 { get; set; }
        public int Premium { get; set; }

    }
    #region Weapon Enums
    public enum WeaponSetType : byte
    {
        WeaponSetE = 0,
        WeaponSetM = 1,
        WeaponSetS = 2,
        WeaponSetA = 3,
        WeaponSetH = 4,
    }
    public enum WeaponsSlotsA : byte
    {
        slot1A = 0,
        slot2A = 1,
        slot3A = 2,
        slot4A = 3,
        slot5A = 4,
        slot6A = 5,
        slot7A = 6,
        slot8A = 7,
    }
    public enum WeaponsSlotsH : byte
    {
        slot1H = 0,
        slot2H = 1,
        slot3H = 2,
        slot4H = 3,
        slot5H = 4,
        slot6H = 5,
        slot7H = 6,
        slot8H = 7,
    }
    public enum WeaponsSlotsE : byte
    {
        slot1E = 0,
        slot2E = 1,
        slot3E = 2,
        slot4E = 3,
        slot5E = 4,
        slot6E = 5,
        slot7E = 6,
        slot8E = 7,
    }
    public enum WeaponsSlotsM : byte
    {
        slot1M = 0,
        slot2M = 1,
        slot3M = 2,
        slot4M = 3,
        slot5M = 4,
        slot6M = 5,
        slot7M = 6,
        slot8M = 7,
    }
    public enum WeaponsSlotsS : byte
    {
        slot1S = 0,
        slot2S = 1,
        slot3S = 2,
        slot4S = 3,
        slot5S = 4,
        slot6S = 5,
        slot7S = 6,
        slot8S = 7,
    }
#endregion
}
