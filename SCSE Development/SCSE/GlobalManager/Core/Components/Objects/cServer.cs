using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GlobalManager.Core.Components
{
    public class cServer : IDisposable
    {
        public int Index;
        public ushort ID;
        public bool Authorized;

        public string IP;
        public ushort Port;
        public DateTime StartTime;
        public bool Online;

        public int CurrectUser;
        public int MaxUser;

        public bool RequestedShutdown;
        public void Shutdown()
        {
            RequestedShutdown = true;
            this.Dispose();
        }

        #region IDisposable

        private bool disposed = false;
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
                    //Free other state (managed objects)
                }
                //Free your own state (unmanaged objects)
                //Set large filed to null.
                IP = null;

                //Mark as disposed
                disposed = true;
            }
        }

        // Use C# destructor syntax for finalization code.
        ~cServer()
        {
            // Simply call Dispose(false).
            Dispose(false);
        }

        #endregion

    }
}
