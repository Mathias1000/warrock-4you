using System;
using System.IO;
using System.Text;
using Warrock.Util;
using System.Runtime.InteropServices;

namespace Warrock_Lib.Networking
{
    public class WRPacket : IDisposable
    {
        [DllImport("winmm.dll")]
        private static extern long timeGetTime();

        public int OPCode;
        private long lTimestamp;
        private string[] sBlocks = new string[0];
        public WRPacket(int opcode)//for send server packets
        {
            OPCode = opcode;
            lTimestamp = 0;
        }
        public WRPacket(string EncryptData)//parse Client packets
        {
            sBlocks = EncryptData.Split(new char[] { ' ' });
            lTimestamp = long.Parse(sBlocks[0]);
            OPCode = int.Parse(sBlocks[1]);
        }
        
        public void addBlock(object oBlock)
        {
            Array.Resize(ref sBlocks, sBlocks.Length + 1);
            sBlocks[sBlocks.Length - 1] = Convert.ToString(oBlock);
        }

        public byte[] getPacket()
        {
            string sPacket = string.Empty;

            sPacket =
                Convert.ToString(timeGetTime()) + // Timestamp
                Convert.ToChar(0x20) + // Space
                Convert.ToString(OPCode) + // Operation Code
                Convert.ToChar(0x20); // Space

            for (int i = 0; i < sBlocks.Length; i++)
            {
                sPacket += sBlocks[i].Replace(Convert.ToChar(0x20), Convert.ToChar(0x1D)) + Convert.ToChar(0x20);
            }

            sPacket = WRCrypto.Crypt(sPacket + Convert.ToChar(0x0A));

            return Encoding.Default.GetBytes(sPacket);
        }
        public void Dispose()
        {
            this.OPCode = 0;
            this.sBlocks = null;
            this.lTimestamp = 0;
        }

          ~WRPacket()
          {
              Dispose();
          }
    }
}