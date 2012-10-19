using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Warrock.Data;

namespace Warrock.Game.Events
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class RoomEventAttribute : Attribute
    {
       public RoomActionType Type { get; set; }
       public RoomEventAttribute(RoomActionType type)
       {
           this.Type = type;
       }

    }
}
