using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GlobalManager.Core.Packets
{
    public class ServerManager
    {

        public Dictionary<int, cGatewayServer> GateWayServers = new Dictionary<int,cGatewayServer>();
        public Dictionary<int, cGameServer> GameServers = new Dictionary<int, cGameServer>();
        public Dictionary<int, cDownloadServer> DownloadServers = new Dictionary<int, cDownloadServer>();
        public Dictionary<int, cAgentServer> AgentServers = new Dictionary<int, cAgentServer>();


        public class cServer : IDisposable
        {
            public int Index;
            public ushort  ServerId;
            public bool Authorized;
                        
            public string ServerIp;
            public ushort  ServerPort;
            public DateTime StartTime;
            public bool Online;

            public int CurrectUser;
            public int MaxUser;

            public void Dispose()
            {
                
            }
       }

        public class cGameServer : cServer
        {
            //General Server Data
            public UInt32 MobCount = 0;
            public UInt32 ItemCount = 0;
            public UInt32 NpcCount = 0;
            //Statistic Data (Todo??)
            public UInt32 AllItemsCount = 0;
            public UInt32 AllPlayersCount = 0;
            public UInt32 AllSkillsCount = 0;
            //Settings
            public ushort Server_XPRate = 1;
            public ushort Server_SPRate = 1;
            public ushort Server_GoldRate = 1;
            public ushort Server_DropRate = 1;
            public ushort Server_SpawnRate = 1;
        }

        public class cGatewayServer : cServer
        {
        }

        public class cDownloadServer : cServer
        {
        }

        public class cAgentServer : cServer
        {
        }


        
    }
}
