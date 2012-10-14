using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Warrock.Database;

namespace Warrock.Game.Item
{
    public class item
    {
        public string itemCode { get; set; }
        public long expireDate { get; set; }
        public byte Class { get; set; }
        public byte BandageSlot { get; set; }

        public static item LoadFromDatabase(DataRow Row)
        {
            item pItem = new item
            {
                itemCode = Row["ItemCode"].ToString(),
                expireDate = GetDataTypes.GetLong(Row["expireDate"]),
                Class = GetDataTypes.GetByte(Row["Class"]),
                BandageSlot = GetDataTypes.GetByte(Row["BandageSlot"]),
            };
            return pItem;
        }
    }
}
