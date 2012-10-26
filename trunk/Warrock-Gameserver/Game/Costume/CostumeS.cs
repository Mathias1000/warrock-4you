using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Warrock.Game.Costume
{
    public sealed class CostumeS : Costume
    {
        public override string CustomeCode { get; internal set; }

        public CostumeS()
        {
            this.Class = 2;
            this.CustomeCode = "BA03";
        }
    }
}
