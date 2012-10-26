using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Warrock.Database;
using MySql.Data.MySqlClient;

namespace Warrock.Game.Item
{
    public class item
    {
        public byte InventorySlot { get; set; }
        public string itemCode { get; set; }
        public long expireDate { get; set; }
        public byte[] Class { get; set; }

        public static item LoadFromDatabase(DataRow Row)
        {
            item pItem = new item
            {
                itemCode = Row["ItemCode"].ToString(),
                expireDate = GetDataTypes.GetLong(Row["expireDate"]),
                InventorySlot = GetDataTypes.GetByte(Row["InventorySlot"]),
            };
            pItem.Class = new byte[5];
            for (int i = 0; i < 5; i++)
            {
               pItem.Class[i] = GetDataTypes.GetByte(Row["Class" + i]);
            }
            return pItem;
        }
        public void Save(int UserID)
        {
            bool isPX = false;
            if (this is pXItem)
            {
                isPX = true;
            }
            using (DatabaseClient pClientDb = Program.DatabaseManager.GetClient())
            {
                using (var command = new MySqlCommand("Save_Item"))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@pUserID", UserID);
                    command.Parameters.AddWithValue("@pIsPX", Convert.ToByte(isPX));

                    for (int i = 0; i < 5; i++)
                    {
                        command.Parameters.AddWithValue("@pClass"+i+"", this.Class[i]);
                    }
                    command.Parameters.AddWithValue("@pInventorySlot", this.InventorySlot);
                    command.Parameters.AddWithValue("@pexpireDate", this.expireDate);
                    command.Parameters.AddWithValue("@pItemCode", this.itemCode);
                    pClientDb.ExecuteScalar(command);
                }
            }
        }
        public string genItemString()
        {
            return "DA04,^,^,^,^,^,^,^,^,^";// for buyweaon??
        }
    }
}
