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
                InventorySlot = GetDataTypes.GetByte(Row["InventorySlot"]),
            };

            for (int i = 0; i < 5; i++)
            {
               pItem.Class[i] = GetDataTypes.GetByte(Row["Class" + i]);
            }
            return pItem;
        }
    }
}
