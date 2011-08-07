using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Framework;

namespace GlobalManager
{
    public static class Codes
    {
        public static string root;

        public const string IdentityName = "GlobalServer";
        public const byte IdentityFlag = 1;
        
        #region Settings
        public static ushort Port;

        public static string MySQL_IP;
        public static string MySQL_DB;
        public static string MySQL_Username;
        public static string MySQL_Password;

        public static ushort Current;
        public static ushort Capacity;
        public static string ServerPassword;

        public static ushort[] CertificationTable;

        #endregion

        //Logs
        public static cLog Logger;

    }
}
