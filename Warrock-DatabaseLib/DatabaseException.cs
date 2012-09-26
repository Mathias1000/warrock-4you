using System;

namespace Warrock.Database
{
        [Serializable()]
        public class DatabaseException : Exception
        {
            internal DatabaseException(string sMessage) : base(sMessage) { }
        }
}