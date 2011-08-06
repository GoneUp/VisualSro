using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

using Framework;
using Framework.Opcodes;
using Framework.SilkroadSecurityApi;


namespace GlobalManager.Core
{
    public class Server
    {
        private static Sockets.ServerSocket m_serverSocket;

        private static int m_clientCounter;
        private static Dictionary<int, Sockets.ClientSocket> m_clientSockets;
        public static Packets.ServerManager Manager;

        public static void Start()
        {
            m_serverSocket = new Sockets.ServerSocket();
            m_serverSocket.Listen();

            m_clientSockets = new Dictionary<int, Sockets.ClientSocket>();


            //Start PacketProcessor
            thPacketProcessor = new System.Threading.Thread(ThreadedPacketProcessing);
            thPacketProcessor.IsBackground = true;
            thPacketProcessor.Start();
        }

        public static void Shutdown()
        {

        }

        public static void Connect(Socket connectingSocket)
        {
            lock (m_clientSockets)
            {
                m_clientCounter++;
                m_clientSockets.Add(m_clientCounter++, new Sockets.ClientSocket(m_clientCounter, connectingSocket));

                //increase capacity counter
            }
        }

        public static void Disconnect(int index)
        {
            lock (m_clientSockets)
            {
                if (m_clientSockets.ContainsKey(index))
                {
                    m_clientSockets[index].Dispose();
                    m_clientSockets.Remove(index);

                    //Decrease capacity counter
                }
                else
                {
                    Codes.Logger.LogThis("ClientIndex " + index + " was not found, this could interfere the login system (AlreadyLoggedIn,Maximum IP)", 2);
                }
            }
        }

        public static void HandlePacket(int index, Packet packet)
        {
            var opcode = (ClientOpcodes)packet.Opcode;
            var displayName = opcode.ToString("G");
            switch (opcode)
            {
                case ClientOpcodes.AcceptHandshake:

                    break;
                case ClientOpcodes.ClientInfo:
                    Packets.Gateway.decodeServerLogin(index, packet);
                    break;
                case ClientOpcodes.Ping:
                    break;
                    
                case ClientOpcodes .Server_Init:
                    Packets.Gateway.decodeServerInit(index, packet);
                    break;

                case ClientOpcodes.Server_Shutdown:
                    Packets.Gateway.decodeServerShutdown(index, packet);
                    break;

                default:

                    break;
            }
        }

        #region PacketProcessor

        private static System.Threading.Thread thPacketProcessor;
        private static void ThreadedPacketProcessing()
        {
            while (true)
            {
                var clientList = m_clientSockets;
                //Process Incoming
                foreach (var client in clientList)
                {
                    try
                    {
                        client.Value.ProcessIncoming();
                    }
                    catch (Exception ex)
                    {

                        throw ex;
                    }
                }

                //Process Outgoing
                foreach (var client in clientList)
                {
                    try
                    {
                        client.Value.ProcessOutgoing();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }

                System.Threading.Thread.Sleep(1);
            }
        }

        #endregion
        #region "Helper Functions"
        public static Sockets.ClientSocket GetClientSocket(int index)
        {
            if (m_clientSockets.ContainsKey(index))
            {
                return m_clientSockets[index];
            }
            return null;
        }

        #endregion


    }
}
