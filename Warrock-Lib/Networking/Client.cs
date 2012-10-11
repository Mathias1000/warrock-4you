using System;
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
        public ClientType ccType { get; set; }

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

            
            this.Socket.BeginReceive(receiveBuffer, 0, receiveBuffer.Length, SocketFlags.None, new AsyncCallback(arrivedData), null);
        }
        public virtual void SendPacket(WRPacket Packet)
        {
            this.Socket.Send(Packet.getLoginPacket());//as default
        }  
        private void arrivedData(IAsyncResult iAr)
        {
            try
            {
                mReceiveLength = this.Socket.EndReceive(iAr);

                if (mReceiveLength > 1 && Socket.Connected)
                {
                    byte[] packetData = new byte[mReceiveLength];
                    Buffer.BlockCopy(receiveBuffer, 0, packetData, 0, mReceiveLength);
                    string decryptdatat = null;
                    if (this.ccType == ClientType.LoginClient)
                    {
                        decryptdatat = WRCrypto.LoginDecrypt(packetData);
                    }
                    else if (this.ccType == ClientType.GameClient)
                    {
                        decryptdatat = WRCrypto.GameDecrypt(packetData);
                    }
                    if (OnPacket != null && decryptdatat != null)
                    {
                        try
                        {
                            WRPacket packet = new WRPacket(decryptdatat);
                            this.OnPacket(this, new PacketReceivedEventArgs(packet));
                        }
                        catch (Exception ex)
                        {
                            Log.WriteLine(LogLevel.Warn, "ERROR Failed Parse Packet From Host {0}", Host);
                            Disconnect();
                        }
                    }
                }
                else
                {
                    this.Disconnect();//??
                }
                this.Socket.BeginReceive(receiveBuffer, 0, receiveBuffer.Length, SocketFlags.None, new AsyncCallback(arrivedData), null);
            }
            catch {  this.Disconnect(); }
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
