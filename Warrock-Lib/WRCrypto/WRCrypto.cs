using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Warrock_Lib
{
    public class WRCrypto
    {
        public static string LoginDecrypt(byte[] tBytes)
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
        public static string Crypt2(string sPacket)
        {
            byte[] tTemp = Encoding.Default.GetBytes(sPacket);

            for (int i = 0; i < tTemp.Length; i++)
            {
                tTemp[i] = Convert.ToByte(tTemp[i] ^ 0x11); //45
            }
            return Encoding.Default.GetString(tTemp);
        }
        public static string GameDecrypt(byte[] sPacket)
        {
            byte[] tTemp = sPacket;

            for (int i = 0; i < tTemp.Length; i++)
            {
                tTemp[i] = Convert.ToByte(tTemp[i] ^ 0x45);
            }
            return Encoding.Default.GetString(tTemp);
        }
    }

}
