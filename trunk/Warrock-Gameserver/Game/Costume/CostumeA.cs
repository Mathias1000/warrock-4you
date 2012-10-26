using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Warrock.Game.Costume
{
    public sealed class CostumeA : Costume
    {
        public override string CustomeCode { get; internal set; }

        public CostumeA()
        {
            this.Class = 3;
            this.CustomeCode = "BA04";
        }

    }
}
