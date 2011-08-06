using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace SilkroadSniffer.Network
{
    public class ProxyConnection
    {

        private static Socket m_ProxySocket;
        private static byte[] m_buffer;

        private static bool m_isClosing;

        public static bool Connect(ushort ProxyPort)
        {
            if (m_ProxySocket == null)
            {
                m_ProxySocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            }
            if (m_ProxySocket.Connected)
            {
                m_isClosing = true;
                m_ProxySocket.Shutdown(SocketShutdown.Both);
                m_ProxySocket.Close();
                m_ProxySocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            }
            if (m_buffer == null)
            {
                m_buffer = new byte[8192];
            }
            try
            {
                m_isClosing = false;
                m_ProxySocket.Connect(IPAddress.Loopback, ProxyPort);
                m_ProxySocket.BeginReceive(m_buffer, 0, 8192, SocketFlags.None, new AsyncCallback(WaitForData), m_ProxySocket);

                m_packetQueue = new List<phPacket>();
                m_packetProcessor = new Thread(ThreadedPacketProcessing);
                m_packetProcessor.IsBackground = true;
                m_packetProcessor.Start();

                return true;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                return false;
            }
        }

        private static void WaitForData(IAsyncResult ar)
        {
            if (m_ProxySocket != null)
            {
                if (m_isClosing == false)
                {
                    try
                    {
                        Socket worker = (Socket)ar.AsyncState;
                        if (worker.EndReceive(ar) > 0)
                        {
                            var packet = new phPacket(m_buffer);
                            lock (m_packetQueue)
                            {
                                m_packetQueue.Add(packet);
                            }
                            worker.BeginReceive(m_buffer, 0, 8192, SocketFlags.None, new AsyncCallback(WaitForData), worker);
                        }
                        else //Connection Ended
                        {
                            m_isClosing = true;
                            if (m_ProxySocket.Connected)
                            {
                                m_ProxySocket.Shutdown(SocketShutdown.Both);
                                m_ProxySocket.Close();
                            }
                            m_ProxySocket = null;
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Windows.Forms.MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private static List<phPacket> m_packetQueue;
        private static Thread m_packetProcessor;

        private static void ThreadedPacketProcessing()
        {
            while (m_isClosing == false)
            {
                if (m_packetQueue.Count == 0)
                {
                    System.Threading.Thread.Sleep(10);
                }
                if (m_packetQueue.Count > 0)
                {
                    var packet = m_packetQueue[0];
                    PacketHandler.PacketHandler.NewPacketToHandle(packet);
                    lock (m_packetQueue)
                    {
                        m_packetQueue.RemoveAt(0);
                    }
                }


            }
        }

    }
}
