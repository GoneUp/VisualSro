using System.Runtime.InteropServices;
using System.Text;
using System;

namespace Framework
{
    public class cINI : IDisposable
    {
        #region Native Functions

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        static extern uint GetPrivateProfileString(
           string lpAppName,
           string lpKeyName,
           string lpDefault,
           StringBuilder lpReturnedString,
           uint nSize,
           string lpFileName);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool WritePrivateProfileString(string lpAppName,
           string lpKeyName, string lpString, string lpFileName);

        [DllImport("kernel32.dll")]
        static extern int GetPrivateProfileInt(string lpAppName, string lpKeyName,
           int nDefault, string lpFileName);

        #endregion

        public string myPath;

        public cINI(string Path)
        {
            myPath = Path;
        }

        public bool Exists()
        {
            return System.IO.File.Exists(myPath);
        }

        public string Read(string Section, string Key, string Default)
        {
            StringBuilder str = new StringBuilder(255);
            try
            {
                uint res = GetPrivateProfileString(Section, Key, Default, str, (uint)str.Capacity, myPath);
                return str.ToString();
            }
            finally
            {
                str = null;
            }
        }
        public int ReadInt(string Section, string Key, int Default)
        {
            return GetPrivateProfileInt(Section, Key, Default, myPath);
        }

        public bool Write(string Section, string Key, string Value)
        {
            return WritePrivateProfileString(Section, Key, Value, myPath);
        }



        public void Dispose()
        {
            myPath = null;            
        }
    }
}
