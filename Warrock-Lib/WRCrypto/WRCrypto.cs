using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Warrock_Lib
{
    public class WRCrypto
    {
        public static string deCrypt(byte[] tBytes)
        {
            for (int i = 0; i < tBytes.Length; i++)
            {
                tBytes[i] = Convert.ToByte(tBytes[i] ^ 0xC3);
            }

            return Encoding.Default.GetString(tBytes);
        }
        public static string Crypt(string sPacket)
        {
            byte[] tBytes = Encoding.Default.GetBytes(sPacket);

            for (int i = 0; i < tBytes.Length; i++)
            {
                tBytes[i] = Convert.ToByte(tBytes[i] ^ 0x96);
            }

            return Encoding.Default.GetString(tBytes);
        }
        public static string GameCrypt(string sPacket)
        {
            byte[] tBytes = Encoding.Default.GetBytes(sPacket);

            for (int i = 0; i < tBytes.Length; i++)
            {
                tBytes[i] = Convert.ToByte(tBytes[i] ^ 0x10);
            }
            return Encoding.Default.GetString(tBytes);
        }
    }
}
