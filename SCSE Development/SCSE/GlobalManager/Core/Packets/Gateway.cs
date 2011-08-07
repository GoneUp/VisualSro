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
            ushort ServerId = packet.ReadUShort();

            Sockets.ClientSocket client;
            client = Server.GetClientSocket(index);


            if (password == Codes.ServerPassword)
            {
                if (CheckCertification(ServerId))
                {
                    switch (clientname)
                    {
                        case "GateWayServer":
                            var tmp_gw = new cGatewayServer();
                            tmp_gw.Index = index;
                            tmp_gw.ID = ServerId;
                            tmp_gw.Online = false;
                            tmp_gw.Authorized = true;
                            tmp_gw.IP = client.IP;
                            tmp_gw.Port = client.Port;
                            Server.Manager.GatewayServers.Add(ServerId, tmp_gw);

                            Codes.Logger.LogThis(string.Format("[Manager:Auth] : Authorized GateWayServer [Id: {0}, Ip: {1}]", ServerId, client.IP), 3);
                            SendServerInfo(index, true);
                            break;

                        case "GameServer":
                            var tmp_game = new cGameServer();
                            tmp_game.Index = index;
                            tmp_game.ID = ServerId;
                            tmp_game.Online = false;
                            tmp_game.Authorized = true;
                            tmp_game.IP = client.IP;
                            tmp_game.Port = client.Port;
                            Server.Manager.GameServers.Add(ServerId, tmp_game);

                            Codes.Logger.LogThis(string.Format("[Manager:Auth] : Authorized GameServer [Id: {0}, Ip: {1}]", ServerId, client.IP), 3);
                            SendServerInfo(index, true);
                            break;

                        case "AgentServer":
                            var tmp_agent = new cAgentServer();
                            tmp_agent.Index = index;
                            tmp_agent.ID = ServerId;
                            tmp_agent.Online = false;
                            tmp_agent.Authorized = true;
                            tmp_agent.IP = client.IP;
                            tmp_agent.Port = client.Port;
                            Server.Manager.AgentServers.Add(ServerId, tmp_agent);

                            Codes.Logger.LogThis(string.Format("[Manager:Auth] : Authorized AgentServer [Id: {0}, Ip: {1}]", ServerId, client.IP), 3);
                            SendServerInfo(index, true);
                            break;

                        case "DownloadServer":
                            var tmp_down = new cDownloadServer();
                            tmp_down.Index = index;
                            tmp_down.ID = ServerId;
                            tmp_down.Online = false;
                            tmp_down.Authorized = true;
                            tmp_down.IP = client.IP;
                            tmp_down.Port = client.Port;
                            Server.Manager.DownloadServers.Add(ServerId, tmp_down);

                            Codes.Logger.LogThis(string.Format("[Manager:Auth] : Authorized DownloadServer [Id: {0}, Ip: {1}]", ServerId, client.IP), 3);
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
                    Codes.Logger.LogThis("[Manager:Auth] Unknown ServerId: " + ServerId, 2);
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
            if (Server.Manager.GatewayServers.ContainsKey(ServerId))
            {
                Server.Manager.GatewayServers[ServerId].Online = true;
                Server.Manager.GatewayServers[ServerId].StartTime = DateTime.Now;
                SendServerInit(index, true);
            }
            else if (Server.Manager.GameServers.ContainsKey(ServerId))
            {
                Server.Manager.GameServers[ServerId].Online = true;
                Server.Manager.GameServers[ServerId].StartTime = DateTime.Now;
                SendServerInit(index, true);
            }
            else if (Server.Manager.DownloadServers.ContainsKey(ServerId))
            {
                Server.Manager.DownloadServers[ServerId].Online = true;
                Server.Manager.DownloadServers[ServerId].StartTime = DateTime.Now;
                SendServerInit(index, true);
            }
            else if (Server.Manager.AgentServers.ContainsKey(ServerId))
            {
                Server.Manager.AgentServers[ServerId].Online = true;
                Server.Manager.AgentServers[ServerId].StartTime = DateTime.Now;
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
            ushort ServerId = packet.ReadUShort();
            if (Server.Manager.GatewayServers.ContainsKey(ServerId))
            {
                Server.Manager.GatewayServers[ServerId].Online = false;
                Server.Manager.GatewayServers[ServerId].Shutdown();
                //Server.Manager.GatewayServers.Remove(ServerId);
                //SendServerShutdown(index, true);

                Codes.Logger.LogThis("[Manager:Shutdown] Turned off GatewayServer: " + ServerId, 3);
            }
            else if (Server.Manager.GameServers.ContainsKey(ServerId))
            {
                Server.Manager.GameServers[ServerId].Online = false;
                Server.Manager.GameServers.Remove(ServerId);
                SendServerShutdown(index, true);

                Codes.Logger.LogThis("[Manager:Shutdown] Turned off GameServer: " + ServerId, 3);
            }
            else if (Server.Manager.DownloadServers.ContainsKey(ServerId))
            {
                Server.Manager.DownloadServers[ServerId].Online = false;
                Server.Manager.DownloadServers.Remove(ServerId);
                SendServerShutdown(index, true);

                Codes.Logger.LogThis("[Manager:Shutdown] Turned off DownServer: " + ServerId, 3);
            }
            else if (Server.Manager.AgentServers.ContainsKey(ServerId))
            {
                Server.Manager.AgentServers[ServerId].Online = false;
                Server.Manager.AgentServers.Remove(ServerId);
                SendServerShutdown(index, true);

                Codes.Logger.LogThis("[Manager:Shutdown] Turned off AgentServer: " + ServerId, 3);
            }
            else
            {
                SendServerInit(index, false);
                Codes.Logger.LogThis("[Manager:Shutdown] Shutdown Failed: " + ServerId, 2);
            }



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
