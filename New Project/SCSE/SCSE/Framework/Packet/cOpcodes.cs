using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Packet
{
    class ClientOpcodes
    {
        public const ushort ClientInfo = 0x2001,
        Ping = 0x2002, 
        AcceptHandshake = 0x9000,
        PatchRequest = 0x6100,
        ServerlistRequest = 0x6101,
        LoginAuth = 0x6102,
        Launcher = 0x6104;
    }

    class ServerOpcodes
    {
        public const ushort ServerInfo = 0x2001,
        Handshake = 0x5000,
        MassiveMessage = 0x600D,
        ServerList = 0xA101,
        LoginAuth = 0xA102;
    }



}

