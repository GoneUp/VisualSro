using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Framework.Opcodes;
using Framework.SilkroadSecurityApi;

using GlobalManager.Core.Components;

namespace GlobalManager.Core.Packets
{
    public class Gateway
    {

        public static void decodeServerLogin(int index, Packet packet)
        {
            string clientname = packet.ReadString();
            string password = packet.ReadString();
            ushort ServerID = packet.ReadUShort();

            Sockets.ClientSocket client;
            client = Server.GetClientSocket(index);


            if (password == Codes.ServerPassword)
            {
                if (CheckCertification(ServerID))
                {
                    switch (clientname)
                    {
                        case "GatewayServer":
                        case "GameServer":
                        case "AgentServer":
                        case "DownloadServer":
                            Server.Manager.Add(index, ServerID);

                            Codes.Logger.LogThis(string.Format("[Manager:Auth] : Authorized " + clientname + " [Id: {0}, Ip: {1}]", ServerID, client.IP), 3);
                            SendServerInfo(index, true);
                            break;

                        default:
                            Codes.Logger.LogThis("[Manager:Auth] Unknown Clientname: " + clientname, 2);
                            SendServerInfo(index, false);
                            Server.Disconnect(index);
                            break;
                    }
                }
                else
                {
                    //Not in Cert
                    Codes.Logger.LogThis("[Manager:Auth] Unknown ServerId: " + ServerID, 2);
                    SendServerInfo(index, false);
                    Server.Disconnect(index);
                }
            }
            else
            {
                //Wrong password
                Codes.Logger.LogThis("[Manager:Auth] Wrong Password: " + password, 2);
                SendServerInfo(index, false);
                Server.Disconnect(index);
            }
        }

        private static void SendServerInfo(int index, bool AuthSucceed)
        {
            throw new Exception("BÖSE !! ServerInfo wird bereits vom Handshake benutzt und kann nicht manuell gesendet werden dies würde ihn abfangen");
            //Packet packet = new Packet(Convert.ToUInt16(ServerOpcodes.ServerInfo), false);
            //packet.WriteString("GlobalManger");
            //packet.WriteByte(AuthSucceed);
            //Server.GetClientSocket(index).Send(packet);
        }

        private static bool CheckCertification(ushort ServerId)
        {
            foreach (var tmpId in Codes.CertificationTable)
            {
                if (tmpId == ServerId) return true;
            }

            return false;
        }

        public static void decodeServerInit(int index, Packet packet)
        {
            ushort ServerId = packet.ReadUShort();
            var server = Server.Manager[ServerId];
            if (server != null)
            {
                server.Online = true;
                server.StartTime = DateTime.Now;
                SendServerInit(index, true);
            }
            else
            {
                SendServerInit(index, false);
                Codes.Logger.LogThis("[Manager:Init] Init Failed: " + ServerId, 2);
            }
        }

        private static void SendServerInit(int index, bool Succeed)
        {
            Packet packet = new Packet(Convert.ToUInt16(ServerOpcodes.Server_Init), false);
            packet.WriteByte(Succeed);
            Server.GetClientSocket(index).Send(packet);
        }

        public static void decodeServerShutdown(int index, Packet packet)
        {
            ushort ServerID = packet.ReadUShort();
            Server.Manager.Shutdown(ServerID);
            Server.Disconnect(index);
        }

        private static void SendServerShutdown(int index, bool Succeed)
        {
            Packet packet = new Packet(Convert.ToUInt16(ServerOpcodes.Server_Init), false);
            packet.WriteByte(Succeed);
            Server.GetClientSocket(index).Send(packet);
        }
    }

}
