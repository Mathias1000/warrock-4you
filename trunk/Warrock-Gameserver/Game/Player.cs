using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Warrock.Networking;

namespace Warrock.Data
{
    public class Player
    {
        #region Variabels
        public GameClient Client { get; set; }
        public string PlayerName { get; set; }

        public long Ping { get; set; }
        #endregion
    }
}
