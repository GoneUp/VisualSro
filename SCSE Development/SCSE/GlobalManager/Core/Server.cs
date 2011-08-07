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
        private static Dictionary<int, Sockets.ClientSocket> m_clientSockets;
        private static int m_clientCounter;

        private static Components.ServerManager m_serverManager;
        public static Components.ServerManager Manager
        {
            get
            {
                return m_serverManager;
            }
        }

        public static void Start()
        {
            m_serverSocket = new Sockets.ServerSocket();
            m_clientSockets = new Dictionary<int, Sockets.ClientSocket>();

            m_serverManager = new Components.ServerManager();            
            if (m_serverSocket.Listen())
            {
                //Start Components.
                StartPacketProcessor();

            }
            else
            {
                throw new Exception("Server start faild (Can not Listen for Clients)");
            }
        }

        public static void Shutdown()
        {
            StopPacketProcessor();
            m_serverSocket.Shutdown();
            m_serverManager.Shutdown();
        }

        public static void Connect(Socket connectingSocket)
        {
            lock (m_clientSockets)
            {
                m_clientCounter++;
                m_clientSockets.Add(m_clientCounter++, new Sockets.ClientSocket(m_clientCounter, connectingSocket));

                Codes.Current++;
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

                    Codes.Current--;
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

                case ClientOpcodes.Server_Init:
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

        private static void StartPacketProcessor()
        {
            doPacketProcessing = true;

            thPacketProcessor = new System.Threading.Thread(ThreadedPacketProcessing);
            thPacketProcessor.IsBackground = true;
            thPacketProcessor.Start();
        }

        private static void StopPacketProcessor()
        {
            doPacketProcessing = false;
            while (thPacketProcessor != null)
            {
                System.Threading.Thread.Sleep(1);
            }
        }
        private static bool doPacketProcessing;
        private static System.Threading.Thread thPacketProcessor;
        private static void ThreadedPacketProcessing()
        {
            while (doPacketProcessing)
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
