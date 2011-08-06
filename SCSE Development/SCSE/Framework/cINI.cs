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

        [DllImport("kernel32.dll")]
        static extern uint GetPrivateProfileSectionNames(IntPtr lpszReturnBuffer,
           uint nSize, string lpFileName);

        #endregion

        public string m_file;

        public cINI(string file)
        {
            m_file = file;
        }

        public bool Exists()
        {
            return System.IO.File.Exists(m_file);
        }

        public string Read(string Section, string Key, string Default)
        {
            StringBuilder str = new StringBuilder(255);
            try
            {
                uint res = GetPrivateProfileString(Section, Key, Default, str, (uint)str.Capacity, m_file);
                return str.ToString();
            }
            finally
            {
                str = null;
            }
        }
        public int ReadInt(string Section, string Key, int Default)
        {
            return GetPrivateProfileInt(Section, Key, Default, m_file);
        }

        public string[] GetSections()
        {
            uint MAX_BUFFER = 32767;
            IntPtr pReturnedString = Marshal.AllocCoTaskMem((int)MAX_BUFFER);
            uint bytesReturned = GetPrivateProfileSectionNames(pReturnedString, MAX_BUFFER, m_file);
            if (bytesReturned == 0)
            {
                Marshal.FreeCoTaskMem(pReturnedString);
                return null;
            }
            string local = Marshal.PtrToStringAnsi(pReturnedString, (int)bytesReturned).ToString();
            Marshal.FreeCoTaskMem(pReturnedString);
            //use of Substring below removes terminating null for split
            return local.Substring(0, local.Length - 1).Split('\0');
        }

        public bool Write(string Section, string Key, string Value)
        {
            return WritePrivateProfileString(Section, Key, Value, m_file);
        }

        

        public void Dispose()
        {
            m_file = null;
        }
    }
}
