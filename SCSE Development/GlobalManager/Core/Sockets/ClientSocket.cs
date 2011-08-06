using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net.Sockets;
using Framework;
using Framework.SilkroadSecurityApi;

namespace GlobalManager.Core.Sockets
{
    public class ClientSocket : IDisposable
    {
        private int m_index;
        private Socket m_socket;

        private bool m_closing;
        private bool m_disposed;

        private string m_IP;
        public string IP
        {
            get { return m_IP; }
        }

        private Framework.SilkroadSecurityApi.Security m_security;
        //public Framework.SilkroadSecurityApi.Security Security
        //{
        //    get { return m_security; }
        //}

        private TransferBuffer m_recv_buffer;
        private List<Packet> m_recv_packets;
        private List<KeyValuePair<TransferBuffer, Packet>> m_send_buffers;

        public ClientSocket(int index, Socket connectingSocket)
        {
            m_index = index;
            m_socket = connectingSocket;
            m_IP = m_socket.RemoteEndPoint.ToString().Split(':')[0];


            m_recv_buffer = new TransferBuffer(8192, 0, 0);

            BeginPacketRecv();

            m_security = new Framework.SilkroadSecurityApi.Security();
            m_security.ChangeIdentity(Codes.IdentityName, Codes.IdentityFlag);
            m_security.GenerateSecurity(true, true, true);
        }

        private void BeginPacketRecv()
        {
            try
            {
                m_socket.BeginReceive(m_recv_buffer.Buffer, m_recv_buffer.Offset, m_recv_buffer.Buffer.Length, SocketFlags.None, new AsyncCallback(WaitForData), m_socket);
            }
            catch (SocketException se)
            {
                Codes.Logger.LogThis(se.Message, 1);
            }
            catch (Exception ex)
            {
                Codes.Logger.LogThis(ex.Message, 1);
                Server.Disconnect(m_index);
            }
        }

        private void WaitForData(IAsyncResult ar)
        {
            if (m_closing || m_disposed)
            {
                throw new ObjectDisposedException("ClientSocket", "[ClientSocket::WaitForData] Can not WaitForData because socket is closing or already disposed.");
            }

            try
            {
                Socket worker = (Socket)ar.AsyncState;
                m_recv_buffer.Size = worker.EndReceive(ar);
                m_security.Recv(m_recv_buffer);
                BeginPacketRecv();
            }
            catch (SocketException se)
            {
                Codes.Logger.LogThis(se.Message, 1);
                if (se.SocketErrorCode == SocketError.ConnectionReset) //Client Disconnected
                {
                    Server.Disconnect(m_index);
                }
                else
                {
                    Codes.Logger.LogThis(se.Message, 4);
                    Console.Beep(); //Dummy breakpoint
                }
            }
            catch (ObjectDisposedException ode)
            {
                Codes.Logger.LogThis(ode.Message, 1);
                Console.Beep(); //Dummy breakpoint
            }
            catch (Exception ex)
            {
                Codes.Logger.LogThis(ex.Message, 1);
                Server.Disconnect(m_index);
            }

        }

        public void Send(Packet packet)
        {
            m_security.Send(packet);
        }

        public void ProcessIncoming()
        {
            if (m_disposed || m_closing)
            {
                throw new ObjectDisposedException("ClientSocket", "[ClientSocket::ProcessIncoming] Not allowed to ProcessIncoming because closing or already disposed.");
            }

            //Process            
            m_recv_packets = m_security.TransferIncoming();
            if (m_recv_packets != null)
            {
                lock (m_recv_packets)
                {
                    foreach (var packet in m_recv_packets)
                    {
                        Server.HandlePacket(m_index, packet); //Run through packet handler                                     
                    }
                }
            }
        }

        public void ProcessOutgoing()
        {
            if (m_disposed || m_closing)
            {
                throw new ObjectDisposedException("m_socket,m_security", "[ClientSocket::ProcessOutgoing] Not allowed to ProcessOutgoing because closing or already disposed.");
            }

            //Process
            m_send_buffers = m_security.TransferOutgoing();
            if (m_send_buffers != null)
            {
                lock (m_send_buffers)
                {
                    foreach (var keyPair in m_send_buffers)
                    {
                        var buffer = keyPair.Key;
                        m_socket.Send(buffer.Buffer, buffer.Offset, buffer.Size, SocketFlags.None); //Send Packet
                    }
                }
            }

        }




        public void Dispose()
        {
            lock (this)
            {
                m_closing = true;
                if (m_socket.Connected)
                {
                    m_socket.Shutdown(SocketShutdown.Both);
                }
                m_socket.Close();
                m_socket = null;

                m_IP = null;

                m_security = null;
                m_recv_buffer = null;
                m_recv_packets = null;
                m_send_buffers = null;

                m_disposed = true;
            }
        }
    }
}
