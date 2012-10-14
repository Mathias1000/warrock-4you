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
    public enum WeaponType : byte
    {
        WeaponA = 1,
        WeaponE = 2,
        WeaponH = 3,
        WeaponM = 4,
        WeaponS = 5,
    }
    public enum WeaponsSlotsA : byte
    {
        slot1A = 1,
        slot2A = 2,
        slot3A = 3,
        slot4A = 4,
        slot5A = 5,
        slot6A = 6,
        slot7A = 7,
        slot8A = 8,
    }
    public enum WeaponsSlotsH : byte
    {
        slot1H = 1,
        slot2H = 2,
        slot3H = 3,
        slot4H = 4,
        slot5H = 5,
        slot6H = 6,
        slot7H = 7,
        slot8H = 8,
    }
    public enum WeaponsSlotsE : byte
    {
        slot1E = 1,
        slot2E = 2,
        slot3E = 3,
        slot4E = 4,
        slot5E = 5,
        slot6E = 6,
        slot7E = 7,
        slot8E = 8,
    }
    public enum WeaponsSlotsM : byte
    {
        slot1M = 1,
        slot2M = 2,
        slot3M = 3,
        slot4M = 4,
        slot5M = 5,
        slot6M = 6,
        slot7M = 7,
        slot8M = 8,
    }
    public enum WeaponsSlotsS : byte
    {
        slot1S = 1,
        slot2S = 2,
        slot3S = 3,
        slot4S = 4,
        slot5S = 5,
        slot6S = 6,
        slot7S = 7,
        slot8S = 8,
    }
#endregion
}
