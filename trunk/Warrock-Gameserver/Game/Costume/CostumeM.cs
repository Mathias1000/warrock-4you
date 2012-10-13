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
            this.CustomeCode = "BA02";
        }
    }
}
