using System;
using System.Collections.Generic;

using System.Text;

using System.Net;
using System.Net.Sockets;
using Warrock.Util;

namespace Warrock_Emulator.UdpServers
{
   public class cUDPServers
    {
        public UdpClient UDPClient1 = null;
        public UdpClient UDPClient2 = null;
       //todo Make udp for Multiple servers
        public byte[] recivedBytes = new byte[1];

        public enum UDPPackets : int
        {
            AuthPacket,
            IPPacket,
            TunnelPacket, // Ping 999
            TunnelPacket2 // Ping 0
        }

        public UDPPackets AnalyzePacket()
        {
            if (recivedBytes[0] == 0x10 && recivedBytes[1] == 0x01 && recivedBytes[2] == 0x01)
            {
                return UDPPackets.AuthPacket;
            }
            else if (recivedBytes[0] == 0x10 && recivedBytes[1] == 0x10 && recivedBytes[2] == 0x00 && recivedBytes[14] == 0x21)
            {
                return UDPPackets.IPPacket;
            }
            else if (recivedBytes[0] == 0x10 && recivedBytes[1] == 0x10 && recivedBytes[2] == 0x00 && recivedBytes[14] == 0x31)
            {
                return UDPPackets.TunnelPacket;
            }

            return ((UDPPackets)0);
        }

        public byte[] GetDGRAM(IPEndPoint IPeo, UDPPackets PacketType)
        {
            byte[] Response = new byte[1];

            if (PacketType == UDPPackets.AuthPacket)
            {
                Response = new Byte[14] { 0x10, 0x01, 0x01, 0x00, 0x14, 0xe7, 0x00, 0x00, 0x00, 0x00,
                    recivedBytes[recivedBytes.Length - 4], 
                    recivedBytes[recivedBytes.Length - 3],
                    recivedBytes[recivedBytes.Length - 2], 
                    recivedBytes[recivedBytes.Length - 1] };


                int tID = (recivedBytes[recivedBytes.Length - 4] << 24) | (recivedBytes[recivedBytes.Length - 3] << 16) | (recivedBytes[recivedBytes.Length - 2] << 8) | recivedBytes[recivedBytes.Length - 1];
                byte[] Session = new byte[2] { recivedBytes[5], recivedBytes[4] };
                int SessionID = BitConverter.ToUInt16(Session, 0);
                /*foreach (cWRClient c in Program.cCollector.getAllPlayers())//later
                {
                    if (c is cWRClient && c.getUserID() == tID)
                    {
                        c.setSessionID(SessionID);
                        c.setNetwork(IPeo);
                        c.RemoteNetwork = IPeo;
                    }
                }*/
            }
            else if (PacketType == UDPPackets.IPPacket)
            {
                byte[] RemoteIP = IPeo.Address.GetAddressBytes();
                byte[] newIPBytes = new byte[4] { RemoteIP[3], RemoteIP[2], RemoteIP[1], RemoteIP[0] };

                byte[] RemotePort = BitConverter.GetBytes(IPeo.Port);
                byte[] newPortBytes = new byte[2] { RemotePort[1], RemotePort[0] };

                byte[] LocalIP = new byte[4] { recivedBytes[33], recivedBytes[34], recivedBytes[35], recivedBytes[36] };
                byte[] newLIPBytes = new byte[4] { LocalIP[3], LocalIP[2], LocalIP[1], LocalIP[0] };

                newLIPBytes[0] = Convert.ToByte(newLIPBytes[0] ^ 0x45);
                newLIPBytes[1] = Convert.ToByte(newLIPBytes[1] ^ 0x45);
                newLIPBytes[2] = Convert.ToByte(newLIPBytes[2] ^ 0x45);
                newLIPBytes[3] = Convert.ToByte(newLIPBytes[3] ^ 0x45);

                byte[] LocalPort = new byte[2] { recivedBytes[37], recivedBytes[38] };
                byte[] newLPortBytes = new byte[2] { LocalPort[1], LocalPort[0] };

                newLPortBytes[0] = Convert.ToByte(newLPortBytes[0] ^ 0x45);
                newLPortBytes[1] = Convert.ToByte(newLPortBytes[1] ^ 0x45);
                byte[] Session = new byte[2] { recivedBytes[5], recivedBytes[4] };
                int SessionID = BitConverter.ToUInt16(Session, 0);
               /* foreach (cWRClient c in Program.cCollector.getAllPlayers())
                {
                    if (c is cWRClient && c.getSessionID() == SessionID)
                    {
                        c.setUPLocalNetwork(IPeo);
                        c.setNetwork(IPeo);
                    }
                }*/


                //PARSE SERVER BYTES
                newLIPBytes[0] = Convert.ToByte(newLIPBytes[0] ^ 0x11);
                newLIPBytes[1] = Convert.ToByte(newLIPBytes[1] ^ 0x11);
                newLIPBytes[2] = Convert.ToByte(newLIPBytes[2] ^ 0x11);
                newLIPBytes[3] = Convert.ToByte(newLIPBytes[3] ^ 0x11);
                newLPortBytes[0] = Convert.ToByte(newLPortBytes[0] ^ 0x11);
                newLPortBytes[1] = Convert.ToByte(newLPortBytes[1] ^ 0x11);


                newIPBytes[0] = Convert.ToByte(newLIPBytes[0] ^ 0x45);
                newIPBytes[1] = Convert.ToByte(newLIPBytes[1] ^ 0x45);
                newIPBytes[2] = Convert.ToByte(newLIPBytes[2] ^ 0x45);
                newIPBytes[3] = Convert.ToByte(newLIPBytes[3] ^ 0x45);
                newPortBytes[0] = Convert.ToByte(newLPortBytes[0] ^ 0x45);
                newPortBytes[1] = Convert.ToByte(newLPortBytes[1] ^ 0x45);

                Response = new Byte[65] 
                {
                   0x10, 0x10, 0x00, 0x00, recivedBytes[4], recivedBytes[5],
			    0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x21,
			    0x0, 0x0, 0x41, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
			    0x0, 0x0, 0x0, 0x1, 0x11, 0x13, 0x11, 
                newPortBytes[1], newPortBytes[0], newIPBytes[3], newIPBytes[2], newIPBytes[1], newIPBytes[0], /* Remote Stuff */
			    0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x01,
			    0x11, 0x13, 0x11, 
                newLPortBytes[1], newLPortBytes[0],newLIPBytes[3], newLIPBytes[2], newLIPBytes[1], newLIPBytes[0], /* Local Stuff */
                0x19, 0x19, 0x19, 0x19, 0x19, 0x19, 0x19, 0x19, 0x11
                };


            }
            else if (PacketType == UDPPackets.TunnelPacket)
            {
                Response = recivedBytes;

              /*  int roomID = BitConverter.ToUInt16(recivedBytes, 7);//later
                cWRRoom r = Program.rCollector.getRoomByID(roomID, 1);
                foreach (cWRClient c in r.Players)
                {
                    UDPClient1.Send(recivedBytes, recivedBytes.Length, c.RemoteNetwork);
                }*/
            }

            return Response;
        }

        private void RecvUDP2()
        {
            IPEndPoint remoteEP2 = new IPEndPoint(IPAddress.Any, 5350);
            UDPClient2 = new UdpClient(5351);
            try
            {
                for (; ; )
                {
                    byte[] rRef = UDPClient2.Receive(ref remoteEP2);
                    recivedBytes = rRef;
                    byte[] rSend = GetDGRAM(remoteEP2, AnalyzePacket());
                    UDPClient2.Send(rSend, rSend.Length, remoteEP2);
                }
            }
            catch (Exception ex)
            {
                Log.WriteLine(LogLevel.Error, "---------------------------------");
                Log.WriteLine(LogLevel.Error, "UDP Socket #2 Fail:");
                Log.WriteLine(LogLevel.Error, ex.Message.ToString());
                Log.WriteLine(LogLevel.Error, "---------------------------------");
            }

        }
        private void RecvUDP1()
        {
            IPEndPoint remoteEP1 = new IPEndPoint(IPAddress.Any, 5350);
            UDPClient1 = new UdpClient(5350);

            try
            {
                for (; ; )
                {
                    byte[] rRef = UDPClient1.Receive(ref remoteEP1);
                    recivedBytes = rRef;
                    byte[] rSend = GetDGRAM(remoteEP1, AnalyzePacket());
                    UDPClient1.Send(rSend, rSend.Length, remoteEP1);
                }
            }
            catch (Exception ex)
            {
                Log.WriteLine(LogLevel.Error, "---------------------------------");
                Log.WriteLine(LogLevel.Error, "UDP Socket #1 Fail:");
                Log.WriteLine(LogLevel.Error, ex.Message.ToString());
                Log.WriteLine(LogLevel.Error, "---------------------------------");
            }

        }
        public bool SetupUDPServer()
        {
            try
            {
                System.Threading.Thread RecvThread1 = new System.Threading.Thread(new System.Threading.ThreadStart(RecvUDP1));
                RecvThread1.Start();

                System.Threading.Thread RecvThread2 = new System.Threading.Thread(new System.Threading.ThreadStart(RecvUDP2));
                RecvThread2.Start();
                Log.WriteLine(LogLevel.Info, "UDPServers Startet Succes");
                return true;

            }
            catch (Exception ex)
            {

                Log.WriteLine(LogLevel.Error,"---------------------------------");
                Log.WriteLine(LogLevel.Error, "Create UDP Thread Failed");
                Log.WriteLine(LogLevel.Error, ex.Message.ToString());
                Log.WriteLine(LogLevel.Error, "---------------------------------");
                return false;
            }
        }
    }
}
