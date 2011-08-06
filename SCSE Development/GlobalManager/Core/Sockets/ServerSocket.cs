using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;
using System.Net.Sockets;

namespace GlobalManager.Core.Sockets
{
    public class ServerSocket
    {
        private Socket m_socket;

        private bool m_closing;

        public bool Listen()
        {
            if (m_socket == null)
            {
                m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            }

            try
            {
                m_socket.Bind(new IPEndPoint(IPAddress.Any, 19000)); //Codes.Port
                m_socket.Listen(10); //Codes.Capacity
                m_socket.BeginAccept(new AsyncCallback(OnClientConnect), null);
                return true;
            }
            catch (SocketException se)
            {
                Codes.Logger.LogThis(se.Message, 1);
                //if (se.SocketErrorCode == SocketError.AddressAlreadyInUse)
                //{
                return false;
                //}
            }
            catch (Exception ex)
            {
                Codes.Logger.LogThis(ex.Message, 1);
                return false;
            }
        }

        public void Shutdown()
        {
            m_closing = true;
            m_socket.Close();
            m_socket = null;
        }

        public void OnClientConnect(IAsyncResult ar)
        {
            if (m_closing == false)
            {
                Socket worker = m_socket.EndAccept(ar);
                //CapacityCheck
                Server.Connect(worker);
            }
        }
    }
}
