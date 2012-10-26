using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Warrock.Game.Costume
{
    public class Costume
    {
        public virtual string CustomeCode { get; internal set; }
        public virtual long expireDate { get; internal set; }
        public byte Class { get; internal set; }

        public string genFullCustomeString()
        {
            string customstring = null;
            customstring += CustomeCode+",^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^";
            return customstring;
        }
    }
}
