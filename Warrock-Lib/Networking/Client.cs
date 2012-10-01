﻿using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Warrock.Util;

namespace Warrock_Lib.Networking
{
    public class Client
    {
        private const int MaxReceiveBuffer = 16384; //16kb

        private int mDisconnected;
		private readonly byte[] receiveBuffer;
		private int mReceiveStart;
        private int mReceiveLength;
		private readonly ConcurrentQueue<ByteArraySegment> sendSegments;
		private int mSending;

        public Socket Socket { get; private set; }
        public string Host { get; private set; }
        public event EventHandler<PacketReceivedEventArgs> OnPacket;
        public event EventHandler<SessionCloseEventArgs> OnDisconnect;

        public Client(Socket socket)
        {
            sendSegments = new ConcurrentQueue<ByteArraySegment>();
            this.Socket = socket;
            Host =  ((IPEndPoint)Socket.RemoteEndPoint).Address.ToString();
            receiveBuffer = new byte[MaxReceiveBuffer];
            Thread recvThread = new Thread(new ThreadStart(recvData));
            recvThread.Start();
        }
        void SendSPConnect()
        {
            WRPacket p = new WRPacket(4608);
            p.addBlock(new Random().Next(111111111, 999999999));
            p.addBlock(77);
            byte[] rPacket = p.getPacket();
            this.Socket.Send(rPacket, 0, rPacket.Length, SocketFlags.None);
        }
        //this cost very high cpu recode later
        private void recvData()
        {
            SendSPConnect();

            while (true)
            {
                try
                {
                   this.mReceiveLength = this.Socket.Receive(receiveBuffer, 0, receiveBuffer.Length, SocketFlags.None);
                }
                catch { }

                if (this.Socket.Connected && mReceiveLength > 0)
                {
                    byte[] packetData = new byte[mReceiveLength];
                    Buffer.BlockCopy(receiveBuffer, 0, packetData, 0, mReceiveLength);
                    string decryptdatat = WRCrypto.deCrypt(packetData);

                    if (OnPacket != null)
                    {
                        try
                        {
                            WRPacket packet = new WRPacket(decryptdatat);
                            this.OnPacket(this, new PacketReceivedEventArgs(packet));
                        }
                        catch
                        {
                            Log.WriteLine(LogLevel.Warn, "EROR Failed Parse Packet From Host {0}", Host);
                            Disconnect();
                        }
                    }

                }
                else
                {
                    Disconnect();
                    break;
                }
            }
        }
        public void Disconnect()
        {
            if (Interlocked.CompareExchange(ref mDisconnected, 1, 0) == 0)
            {
                try
                {
                    Socket.Shutdown(SocketShutdown.Both);
                }
                catch { }
                if (OnDisconnect != null)
                {
                    this.OnDisconnect(this, SessionCloseEventArgs.ConnectionTerminated); //TODO: split
                }
            }
        }

        public void Send(byte[] pBuffer)
        {
            if (mDisconnected != 0) return;
            // Everything we send is from the main thread, so no async sending.
         /*   int len = pBuffer.Length, offset = 0;
            while (true)
            {
                int send = Socket.Send(pBuffer, offset, len, SocketFlags.None);
                if (send == 0)
                {
                    Disconnect();
                    return;
                }
                offset += send;
                if (offset == len) break;
            } */
            
            sendSegments.Enqueue(new ByteArraySegment(pBuffer));
            if (Interlocked.CompareExchange(ref mSending, 1, 0) == 0)
            {
                BeginSend();
            }
        }

        public void SendPacket(WRPacket pPacket)
        {
            Send(pPacket.getPacket());
        }

        private void BeginSend()
        {
            SocketAsyncEventArgs args = new SocketAsyncEventArgs();
            ByteArraySegment segment;
            if (sendSegments.TryPeek(out segment))
            {
                args.Completed += EndSend;
                args.SetBuffer(segment.Buffer, segment.Start, segment.Length);
                // args.SetBuffer(segment.Buffer, segment.Start, Math.Min(segment.Length, 1360));
                try
                {
                    if (!this.Socket.SendAsync(args))
                    {
                            EndSend(this, args);
                    }
                }
                catch (ObjectDisposedException ex)
                {
                    Log.WriteLine(LogLevel.Exception, "Error at BeginSend: {0}", ex.ToString());
                    Disconnect();
                }
            }
        }

        private void EndSend(object sender, SocketAsyncEventArgs pArguments)
        {
            if (mDisconnected != 0) return;

            if (pArguments.BytesTransferred <= 0)
            {
                Disconnect();
                return;
            }

            ByteArraySegment segment;
            if (sendSegments.TryPeek(out segment))
            {
                if (segment.Advance(pArguments.BytesTransferred))
                {
                    ByteArraySegment seg;
                    sendSegments.TryDequeue(out seg); //we try to get it out
                }

                if (sendSegments.Count > 0)
                {
                    this.BeginSend();
                }
                else
                {
                    mSending = 0;
                }
            }
            pArguments.Dispose(); //clears out the whole async buffer
        }
    }
}
