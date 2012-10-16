using System;
using System.Collections.Generic;

using System.Text;
using Warrock.Lib.Networking;
using Warrock.Networking;
using System.Net;
using System.Net.Sockets;
using Warrock.Util;
using Warrock.Game;

namespace Warrock_Emulator.UdpServers
{
    internal class cUDPServers
    {
       private static UdpClient UDPSocket_1 { get; set; }
       private static UdpClient UDPSocket_2 { get; set; }

        private static byte[] Seed = { 0x45, 0x11 };//1 = Packet decryption 2 = crypt

        private static Socket UDPSocket1 { get;  set; }
        private static Socket UDPSocket2 { get;  set; }

        private static byte[] dataBuffer1 = new byte[1024];
        private static byte[] dataBuffer2 = new byte[1024];

        public static bool SetupUDPServer()//todo change port later
        {
            try
            {

                UDPSocket1 = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                UDPSocket2 = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                UDPSocket1.Bind(new IPEndPoint(IPAddress.Any, 5350));
                UDPSocket2.Bind(new IPEndPoint(IPAddress.Any, 5351));

                EndPoint ReadPoint1 = new IPEndPoint(IPAddress.Any, 0);
                EndPoint ReadPoint2 = new IPEndPoint(IPAddress.Any, 0);

                UDPSocket1.BeginReceiveFrom(dataBuffer1, 0, dataBuffer1.Length, SocketFlags.None, ref ReadPoint1, new AsyncCallback(onReceiveA), UDPSocket1);
                UDPSocket2.BeginReceiveFrom(dataBuffer2, 0, dataBuffer2.Length, SocketFlags.None, ref ReadPoint2, new AsyncCallback(onReceiveB), UDPSocket2);
                return true;
            }
            catch (Exception E) { Log.WriteLine(LogLevel.Error,E.ToString()); }

            return false;
        }
        private static void onReceiveA(IAsyncResult iAr)
        {
            try
            {
                Socket UDPSocket = (Socket)iAr.AsyncState;
                EndPoint RemotePoint = new IPEndPoint(IPAddress.Any, 0);
                int BufferLength = UDPSocket.EndReceiveFrom(iAr, ref RemotePoint);
                if (BufferLength > 0)
                {
                    byte[] packetBuffer = new byte[BufferLength];
                    Array.Copy(dataBuffer1, 0, packetBuffer, 0, BufferLength);

                    byte[] Response = AnalyzePacket(packetBuffer, (IPEndPoint)RemotePoint);

                    if (Response.Length > 0)
                    {
                        UDPSocket1.SendTo(Response, (IPEndPoint)RemotePoint);
                    }
                }
                

                EndPoint ReadPoint1 = new IPEndPoint(IPAddress.Any, 0);
                UDPSocket1.BeginReceiveFrom(dataBuffer1, 0, dataBuffer1.Length, SocketFlags.None, ref ReadPoint1, new AsyncCallback(onReceiveA), UDPSocket1);
            }
            catch (SocketException)
            {
                UDPSocket1.Close();
                UDPSocket1 = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                UDPSocket1.Bind(new IPEndPoint(IPAddress.Any, 5350));
                EndPoint ReadPoint1 = new IPEndPoint(IPAddress.Any, 0);
                UDPSocket1.BeginReceiveFrom(dataBuffer1, 0, dataBuffer1.Length, SocketFlags.None, ref ReadPoint1, new AsyncCallback(onReceiveA), UDPSocket1);
            }

        }

        private static void onReceiveB(IAsyncResult iAr)
        {
            try
            {
                Socket UDPSocket = (Socket)iAr.AsyncState;
                EndPoint RemotePoint = new IPEndPoint(IPAddress.Any, 0);
                int BufferLength = UDPSocket.EndReceiveFrom(iAr, ref RemotePoint);

                if (BufferLength > 0)//todo add ban?
                {
                    byte[] packetBuffer = new byte[BufferLength];
                    Array.Copy(dataBuffer2, 0, packetBuffer, 0, BufferLength);

                    byte[] Response = AnalyzePacket(packetBuffer, (IPEndPoint)RemotePoint);

                    if (Response.Length > 0)
                    {
                        UDPSocket2.SendTo(Response, (IPEndPoint)RemotePoint);
                    }
                }
               

                EndPoint ReadPoint1 = new IPEndPoint(IPAddress.Any, 0);
                UDPSocket2.BeginReceiveFrom(dataBuffer2, 0, dataBuffer2.Length, SocketFlags.None, ref ReadPoint1, new AsyncCallback(onReceiveB), UDPSocket2);
            }
            catch (SocketException)
            {
                UDPSocket2.Close();
                UDPSocket2 = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                UDPSocket2.Bind(new IPEndPoint(IPAddress.Any, 5351));
                EndPoint ReadPoint1 = new IPEndPoint(IPAddress.Any, 0);
                UDPSocket2.BeginReceiveFrom(dataBuffer2, 0, dataBuffer2.Length, SocketFlags.None, ref ReadPoint1, new AsyncCallback(onReceiveB), UDPSocket2);
            }

        }
        private  static byte[] AnalyzePacket(byte[] buffer, IPEndPoint EndPoint)
        {
            byte[] Response = new Byte[1] { 0x00 };
            if (buffer[0] == 0x10 && buffer[1] == 0x01 && buffer[2] == 0x01) // Auth Packet
            {
                int targetUser = (buffer[buffer.Length - 4] << 24) | (buffer[buffer.Length - 3] << 16) | (buffer[buffer.Length - 2] << 8) | buffer[buffer.Length - 1];
                byte[] SessionIDBytes = new byte[2] { buffer[5], buffer[4] };
                ushort sessionID = BitConverter.ToUInt16(SessionIDBytes, 0); // ?

                GameClient pTarget = Warrock.ClientManager.Instance.GetClientBySeasson(sessionID);
                if (pTarget != null)
                {
                    if (pTarget.SeassonID== sessionID)
                    {
                        pTarget.setRemoteEndPoint(EndPoint);
                    }
                }

                Response = new Byte[14] { 
                    0x10, 0x01, 0x01,               // Auth Packet Header
                    0x00,
                    0x14, 0xe7,                     // Server Port (SHORT = 2 Byte)
                    0x00, 0x00, 0x00, 0x00,
                    buffer[buffer.Length - 4],      // UserID - 4bytes
                    buffer[buffer.Length - 3],      // UserID - 4bytes
                    buffer[buffer.Length - 2],      // UserID - 4Bytes
                    buffer[buffer.Length - 1] };    // UserID - 4 Bytes
            }
            else if (buffer[0] == 0x10 && buffer[1] == 0x10 && buffer[2] == 0x00) // IP Packet
            {
                if (buffer[14] == 0x21)
                {
                    try
                    {

                        byte[] LocalIPBytes = new byte[4] { buffer[34], buffer[35], buffer[36], buffer[37] };

                        LocalIPBytes[0] = Convert.ToByte(LocalIPBytes[0] ^ Seed[0]);
                        LocalIPBytes[1] = Convert.ToByte(LocalIPBytes[1] ^ Seed[0]);
                        LocalIPBytes[2] = Convert.ToByte(LocalIPBytes[2] ^ Seed[0]);
                        LocalIPBytes[3] = Convert.ToByte(LocalIPBytes[3] ^ Seed[0]);

                        byte[] LocalPortBytes = new byte[2] { buffer[32], buffer[33] };

                        LocalPortBytes[0] = Convert.ToByte(LocalPortBytes[0] ^ Seed[0]);
                        LocalPortBytes[1] = Convert.ToByte(LocalPortBytes[1] ^ Seed[0]);

                        IPEndPoint LocalIPeo = new IPEndPoint(new IPAddress(BitConverter.ToUInt32(LocalIPBytes, 0)), BitConverter.ToUInt16(LocalPortBytes, 0));

                        byte[] SessionIDBytes = new byte[2] { buffer[5], buffer[4] };
                        ushort SessionID = BitConverter.ToUInt16(SessionIDBytes, 0);

                        GameClient Target = Warrock.ClientManager.Instance.GetClientBySeasson(SessionID);
                        if (Target != null)
                        {
                            Target.setLocalEndPoint(LocalIPeo); // LocalIP
                        }

                        byte[] IPBytes = EndPoint.Address.GetAddressBytes();
                        byte[] PortBytes = BitConverter.GetBytes(EndPoint.Port);

                        Response = new Byte[65] 
                    {
                        0x10, 0x10, 0x00, 0x00, buffer[4], buffer[5],
			            0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x21,
			            0x0, 0x0, 0x41, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
			            0x0, 0x0, 0x0, 0x1, 0x11, 0x13, 0x11, 
                        PortBytes[1], PortBytes[0], IPBytes[3], IPBytes[2], IPBytes[1], IPBytes[0], /* Remote Stuff */
			            0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x01,
			            0x11, 0x13, 0x11, 
                        LocalPortBytes[1], LocalPortBytes[0],LocalIPBytes[3], LocalIPBytes[2], LocalIPBytes[1], LocalIPBytes[0], /* Local Stuff */
                        0x19, 0x19, 0x19, 0x19, 0x19, 0x19, 0x19, 0x19, 0x11
                    };

                    }
                    catch (Exception E) { Log.WriteLine(LogLevel.Error,E.ToString()); }
                }
                else if (buffer[14] == 0x31) // Tunneling
                {
                    Response = buffer;

                    byte[] SessionIDBytes = new byte[2] { buffer[5], buffer[4] };
                    ushort SessionID = BitConverter.ToUInt16(SessionIDBytes, 0);

                    ushort RoomID = BitConverter.ToUInt16(buffer, 7);
                    byte Channel = 1;
                    GameClient Target = Warrock.ClientManager.Instance.GetClientBySeasson(SessionID);

                    if (Target != null)
                    {
                        Channel = Target.Player.ChannelID;

                        
                        PlayerRoom Room = Warrock.RoomManager.Instance.GetRoomByChanneldAndID (Channel, Convert.ToInt32(RoomID));
                        if (Room != null)
                        {
                            if (Room.RoomPlayers.Count > 1)
                            {
                                foreach (RoomPlayer Player in Room.RoomPlayers.Values)
                                {
                                    //try
                                    //{
                                    byte[] sendBuffer = buffer;
                                    UDPSocket1.SendTo(sendBuffer, Player.pClient.RemoteEndPoint);
                                    //UDPSocket2.SendTo(sendBuffer, Player.remoteEndPoint);
                                    //}
                                    //catch (Exception ex) { Log.WriteError(ex.Message.ToString()); }
                                }
                            }
                        }
                    }
                }
            }
            return Response;
        }
       
    }
}
