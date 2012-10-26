using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Warrock.Database;

namespace Warrock.Data
{
    public class Weapon
    {
        public int _ID  { get; private set; }
        public int _Damage { get; private set; }
        public int[] prices { get; private set; }

        public String _WeaponCode { get; private set; }
        public String _ItemName { get; private set; }

        public bool _isValidWeapon { get; private set; }
        public bool _isPremium { get; private set; }
        public bool validShopItem { get; private set; }
        public bool _isValidPx { get; private set; }
        public ushort WID { get; private set; }
        public byte[] Slots { get; private set; }
        public byte[] Class { get; set; }

        public static Weapon LoadFromDatabase(DataRow Row)
        {
            Weapon W = new Weapon
            {
                _ID = GetDataTypes.GetInt(Row["ID"]),
                _WeaponCode = Row["ItemCode"].ToString(),
                WID = GetDataTypes.GetUshort(Row["WID"]),
                _ItemName = Row["Name"].ToString(),
                _Damage = GetDataTypes.GetInt(Row["Damage"]),
                _isPremium = GetDataTypes.GetBool(Row["OnlyPremium"]),
                _isValidWeapon = GetDataTypes.GetBool(Row["ValidWeapon"]),
                _isValidPx = GetDataTypes.GetBool(Row["ValidPX"]),
                validShopItem = GetDataTypes.GetBool(Row["ValidShop"]),
            };
            W.Slots = new byte[8];
            W.prices = new int[4];
            W.Class = new byte[5];
            for (int i = 0; i < 8; i++)
            {
                W.Slots[i] = GetDataTypes.GetByte(Row["Slot" + i]);
            }
            for (int i = 0; i < 4; i++)
            {
                W.prices[i] = GetDataTypes.GetInt(Row["Price" + i]);
            }
            for (int i = 0; i < 5; i++)
            {
                W.Class[i] = GetDataTypes.GetByte(Row["Class" + i]);
            }
            return W;
        }
    }
}
