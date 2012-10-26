using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Warrock.Database;
using MySql.Data.MySqlClient;

namespace Warrock
{
    public class Equipment
    {
        public string itemCode { get; set; }
        public long UserID { get; set; }
        public byte Class { get; set; }
        public byte Slot { get; set; }
        public bool Equptet { get; set; }

        public void Save()
        {
            using (DatabaseClient pClientDb = Program.DatabaseManager.GetClient())
            {
                using (var command = new MySqlCommand("Save_equipt"))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@pUserID", this.UserID);
                    command.Parameters.AddWithValue("@pItemCode", this.itemCode);
                    command.Parameters.AddWithValue("@pClass", this.Class);
                    command.Parameters.AddWithValue("@pSlot", this.Slot);
                    pClientDb.ExecuteScalar(command);
                }
            }
        }
        public void Remove()
        {
        }
        public static Equipment LoadFromDatabase(DataRow Row)
        {
            Equipment eq = new Equipment
            {
                UserID = GetDataTypes.GetLong(Row["UserID"]),
                Class = GetDataTypes.GetByte(Row["Class"]),
                itemCode = Row["ItemCode"].ToString(),
                Slot = GetDataTypes.GetByte(Row["Slot"]),
                Equptet = true,
            };
            return eq;
        }
    }
}
