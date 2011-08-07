using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Opcodes
{
    //NOTE: Enums are better because we can switch them easier
    //class ClientOpcodes
    //{
    //    public const ushort ClientInfo = 0x2001,
    //    Ping = 0x2002,
    //    AcceptHandshake = 0x9000,
    //    PatchRequest = 0x6100,
    //    ServerlistRequest = 0x6101,
    //    LoginAuth = 0x6102,
    //    Launcher = 0x6104;
    //}

    //class ServerOpcodes
    //{
    //    public const ushort ServerInfo = 0x2001,
    //    Handshake = 0x5000,
    //    MassiveMessage = 0x600D,
    //    ServerList = 0xA101,
    //    LoginAuth = 0xA102;
    //}

    public enum ClientOpcodes
    {
        None = 0x0000,

        ClientInfo = 0x2001,
        Ping = 0x2002,

        HandshakeResponse = 0x5000,
        AcceptHandshake = 0x9000,

        PatchRequest = 0x6100,
        ServerListRequest = 0x6101,
        LoginRequest = 0x6102,
        NewsRequest = 0x6104,

        Server_Init = 0x3001,
	    Server_Shutdown = 0x3002,
	    Server_Info = 0x3003,
	    Server_UpdateReply = 0x3004,

        Admin_Auth = 0x3005,
        Admin_UpdateServer = 0x3006,
        Admin_GetInfo = 0x3007,
        Admin_UpdateInfo = 0x3008,
         
        GateWay_SendUserAuth = 0x3010,
        AgentServer_CheckUserAuth = 0x3011,
    }

    public enum ServerOpcodes
    {
        None = 0x0000,
        ServerInfo = 0x2001,

        Handshake = 0x5000,

        MassiveMessage = 0x600D,

        ServerList = 0xA101,
        LoginResponse = 0xA102,

        Server_Auth = 0xC000,
        Server_Init = 0xC001,
        Server_Shutdown = 0xC002,
        ShardInfo = 0xC003,
        Server_Update = 0xC004,

        Admin_Auth = 0xC005,
        Admin_UpdateServer = 0xC006,
        Admin_GetInfo = 0xC007,
        Admin_UpdateInfo = 0xC008,
         
        GateWay_SendUserAuth = 0xC010,
        AgentServer_CheckUserAuth = 0xC011,
    }




}

