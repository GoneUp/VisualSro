using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using Framework;
using SilkroadSecurityApi;


namespace GlobalManager.Core
{
    public class Server 
    {

        public static event OnServerStartHandler OnServerStart;
        public static event OnServerStopHandler OnServerStop;
        public static event OnServerErrorHandler OnServerError;
        public static event OnClientConnectHandler OnClientConnect;
        public static event OnClientDisconnectHandler OnClientDisconnect;

        public static Socket ServerSocket;
        public static Socket[] SocketList = new Socket[50]; //[Codes.MaxConnections];
        public static cClientKontext[] ClientList = new cClientKontext[50]; //[Codes.MaxConnections]; 

        public static void Start()
        {
            try
            {
                ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                ServerSocket.Bind(new IPEndPoint(IPAddress.Any, 0));
                ServerSocket.Listen(10);
                ServerSocket.BeginAccept(OnAccept, ServerSocket);
            }
            catch (Exception ex)
            {
                OnServerError(ex);
            }
            finally
            {
                OnServerStart();
            }
                   
        }

        public static void Shutdown()
        {
            try
            {
                ServerSocket.Shutdown(SocketShutdown.Both);
                ServerSocket.Dispose();

                for (int i = 0; i <= SocketList.Length;  i++)
                {
                    SocketList[i].Shutdown(SocketShutdown.Both);
                    SocketList[i].Dispose();
                }
            }
            catch (Exception ex)
            {
                OnServerError(ex);
            }
            finally
            {
                OnServerStop();
            }
        }

        public static void OnAccept(IAsyncResult ar)
        {

            try
            {
                Socket sock = ServerSocket.EndAccept(ar);
               
                if (cClientKontext.FindIndex() != -1)
                {
                    sock.be
                    cClientKontext.Add(sock);
                    OnClientConnect();
                }
                else
                {  
                    if (sock.Connected)
                    {
                        sock.Disconnect(false);
                    }
                    sock.Shutdown(SocketShutdown.Both);
                    sock.Dispose();

                    throw (new Exception ("Max Connections reached!"));
                }
            }
            catch (Exception ex)
            {
                OnServerError(ex);
            }
        }

        public static void OnReceive()
        {

        }

        public static void Send(int Index_, Packet packet)
        {

        }

        public delegate void OnServerStartHandler();
        public delegate void OnServerStopHandler();
        public delegate void OnServerErrorHandler(Exception ex);
        public delegate void OnClientConnectHandler();
        public delegate void OnClientDisconnectHandler();
    }
}
