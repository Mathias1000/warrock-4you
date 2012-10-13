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
        public Dictionary<Warrock.Game.Costume.Costume, string> CustomeList { get; private set; }
        public Dictionary<Weapon, string> Weapons { get; private set; }
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
            Weapons = new Dictionary<Weapon, string>();
            Weapons.Add(A, A.genWeaponString());
            Weapons.Add(E, E.genWeaponString());
            Weapons.Add(H, H.genWeaponString());
            Weapons.Add(M, M.genWeaponString());
            Weapons.Add(S, M.genWeaponString());
        }
        private void InitCustome()
        {

            CostumeA A = new CostumeA();
            CostumeE E = new CostumeE();
            CostumeH H = new CostumeH();
            CostumeM M = new CostumeM();
            CostumeS S = new CostumeS();
            CustomeList = new Dictionary<Warrock.Game.Costume.Costume, string>();
            CustomeList.Add(A, A.genFullCustomeString());// add All Defauls Customes
            CustomeList.Add(E, E.genFullCustomeString());
            CustomeList.Add(H, H.genFullCustomeString());
            CustomeList.Add(M, M.genFullCustomeString());
            CustomeList.Add(S, S.genFullCustomeString());

        }
        #endregion
        public void LoadCustome()
        {
        }
    }
}
