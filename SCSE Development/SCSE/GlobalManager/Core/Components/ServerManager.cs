using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GlobalManager.Core.Components
{
    public class ServerManager : IDisposable
    {
        public Dictionary<int, cGatewayServer> GatewayServers;
        public Dictionary<int, cDownloadServer> DownloadServers;
        public Dictionary<int, cAgentServer> AgentServers;
        public Dictionary<int, cGameServer> GameServers;


        public ServerManager()
        {
            GatewayServers = new Dictionary<int, cGatewayServer>();
            DownloadServers = new Dictionary<int, cDownloadServer>();
            AgentServers = new Dictionary<int, cAgentServer>();
            GameServers = new Dictionary<int, cGameServer>();
        }

        public void Shutdown()
        {
            //Shut all servers down.
            foreach (var server in GatewayServers.Values)
            {
                server.Shutdown();
            }
            foreach (var server in DownloadServers.Values)
            {
                server.Shutdown();
            }
            foreach (var server in AgentServers.Values)
            {
                server.Shutdown();
            }
            foreach (var server in GameServers.Values)
            {
                server.Shutdown();
            }

            Dispose();
        }

        #region IDisposable

        private bool disposed;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    //Free managed
                    GatewayServers.Clear();
                    DownloadServers.Clear();
                    AgentServers.Clear();
                    GameServers.Clear();
                }
                //Free Unmanaged and large fields.
                GatewayServers = null;
                DownloadServers = null;
                AgentServers = null;
                GameServers = null;

                disposed = true;
            }
        }

        ~ServerManager()
        {
            Dispose(false);
        }

        #endregion
    }
}
