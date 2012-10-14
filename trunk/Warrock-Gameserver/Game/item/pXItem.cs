using System.Data;
using Warrock.Database;

namespace Warrock.Game.Item
{
    public class pXItem : item
    {

        public static pXItem LoadFromDatabase(DataRow Row)
        {
            pXItem pItem = new pXItem
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
