using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Warrock.Database;

namespace Warrock.Lib
{
    public class tUser
    {
        public int UserID { get; set; }
        public string NickName { get; set; }
        public string username { get; set; }
        public string Password { get; set; }
        public byte Access_level { get; set; }
        public bool IsOnline { get; set; }
        public bool Banned { get; set; }
        public long BannTime { get; set; }
        public static tUser ReadFromDatabase(DataRow Row)
        {
            tUser tuser = new tUser
            {
                UserID = GetDataTypes.GetInt(Row["UserID"]),
                NickName = (string)Row["NickName"],
                username = (string)Row["username"],
                Password = (string)Row["Password"],
                Access_level = GetDataTypes.GetByte(Row["Access_Level"]),
                BannTime = GetDataTypes.GetInt(Row["Bann_Time"]),
                Banned = GetDataTypes.GetBool(Row["Banned"]),
                IsOnline = GetDataTypes.GetBool(Row["Online"]),

            };
            return tuser;
        }
    }
}
