using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Warrock.Game.Costume
{
    public sealed class CostumeM : Costume
    {
        public override string CustomeCode { get; internal set; }

        public CostumeM()
        {
            this.Class = 1;
            this.CustomeCode = "BA02";
        }
    }
}
