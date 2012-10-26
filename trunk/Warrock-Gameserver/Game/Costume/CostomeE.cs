using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Warrock.Game.Costume
{
    public sealed class CostumeE : Costume
    {
        public override string CustomeCode { get; internal set; }

        public CostumeE()
        {
            this.Class = 0;
            this.CustomeCode = "BA01";
        }
    }
}
