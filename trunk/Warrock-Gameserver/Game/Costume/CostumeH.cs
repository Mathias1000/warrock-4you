using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Warrock.Game.Costume
{
    public sealed class CostumeH : Costume
    {
        public override string CustomeCode { get; internal set; }

        public CostumeH()
        {
            this.CustomeCode = "BA05";

        }
    }
}
