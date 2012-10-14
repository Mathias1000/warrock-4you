using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Warrock.Data;
using Warrock.Game.Weapons;
using Warrock.Game.Costume;

namespace Warrock.Game
{
    public class Inventory
    {
   
        public Dictionary<pCustome, Costume.Costume> Customes { get; private set; }
        public Dictionary<WeaponType, Weapon> Weapons { get; private set; }

        public Inventory()
        {
            InitAllLists();
        }
        #region ListStuff
        private void InitAllLists()
        {
            InitCustome();
            InitWeapons();
        }
        private void InitWeapons()
        {
         
            WeaponA A = new WeaponA();
            WeaponE E = new WeaponE();
            WeaponH H = new WeaponH();
            WeaponM M = new WeaponM();
            WeaponS S = new WeaponS();
            Weapons = new Dictionary<WeaponType, Weapon>();
            Weapons.Add(WeaponType.WeaponA,A);
            Weapons.Add(WeaponType.WeaponE,E);
            Weapons.Add(WeaponType.WeaponH,H);
            Weapons.Add(WeaponType.WeaponM,M);
            Weapons.Add(WeaponType.WeaponS,S);
        }
        private void InitCustome()
        {

            CostumeA A = new CostumeA();
            CostumeE E = new CostumeE();
            CostumeH H = new CostumeH();
            CostumeM M = new CostumeM();
            CostumeS S = new CostumeS();
            Customes = new Dictionary<pCustome, Costume.Costume>();
            Customes.Add(pCustome.CostumeA, A);// add All Defauls Customes
            Customes.Add(pCustome.CostumeE, E);
            Customes.Add(pCustome.CostumeH, H);
            Customes.Add(pCustome.CostumeM, M);
            Customes.Add(pCustome.CostumeS, S);

        }
        #endregion
        #region GetStuff
        #region Weapon
        public Weapon GetWeaponByType(WeaponType Type)
        {
            return this.Weapons[Type];
        }
        public string GetWeaponStringByType(WeaponType Type)
        {
            return this.Weapons[Type].genWeaponString();
        }
        #endregion
        public string getOpenSlots()
        {
            StringBuilder SB = new StringBuilder();
            if (false)//hasPX("CA01")//add later
            {
                SB.Append("T,");
            }
            else
            {
                SB.Append("F,");
            }
            if (this.Weapons[WeaponType.WeaponE].Slots[5] == "^"
                && this.Weapons[WeaponType.WeaponM].Slots[5] == "^"
                && this.Weapons[WeaponType.WeaponS].Slots[5] == "^"
                && this.Weapons[WeaponType.WeaponM].Slots[5] == "^"
                && this.Weapons[WeaponType.WeaponH].Slots[5] == "^")
            {
                SB.Append("F,F,");
            }
            else
            {
                SB.Append("T,F,");
            }
            if (false)//hasPX("CA04") add later
            {
                SB.Append("T");
            }
            else
            {
                SB.Append("F");
            }
            return SB.ToString();
        }
        #region Custome
        public Costume.Costume GetCustomeByType(pCustome Type)
        {
            return this.Customes[Type];
        }
        public string GetCustomStringByType(pCustome Type)
        {
            return this.Customes[Type].genFullCustomeString();
        }
        #endregion
        #endregion
        public void LoadCustome()
        {
        }
    }
}
