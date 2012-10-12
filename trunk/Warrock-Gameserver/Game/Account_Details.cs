using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Warrock.Lib;
using Warrock.Database;

namespace Warrock.Game
{
    public class Account_Details
    {
        public int Experience { get; set; }
        public byte Level { get; set; }
        public int Dinar { get; set; }
        public int Kills { get; set; }
        public int Deaths { get; set; }
        public int Copons { get; set; }
        public int Cash { get; set; }


        public static Account_Details LoadAccountDetailsFromDataBase(DataRow Row)
        {
            Account_Details Details = new Account_Details
            {
                Experience = GetDataTypes.GetInt(Row["Experience"]),
                Level = GetDataTypes.GetByte(Row["Level"]),
                Dinar = GetDataTypes.GetInt(Row["Dinar"]),
                Kills = GetDataTypes.GetInt(Row["Kills"]),
                Deaths = GetDataTypes.GetInt(Row["Deaths"]),
                Copons = GetDataTypes.GetInt(Row["Copons"]),
                Cash = GetDataTypes.GetInt(Row["Cash"]),
            };
            return Details;
        }
    }
}
