using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

namespace Framework
{
    public class cLog : IDisposable
    {

        private string m_path;
        private string m_file;
        private StreamWriter m_writer;

        private DateTime m_logStart;

        private object m_lock;

        public cLog(string path, string name)
        {
            m_path = path + "\\";
            m_file = name;

            if (Directory.Exists(m_path) == false)
            {
                Directory.CreateDirectory(m_path);
            }

            m_logStart = DateTime.Now;
            m_lock = new object();

            m_writer = new StreamWriter(m_path + m_logStart.ToString("dd-MM-yyyy") + "_" + m_file + ".log", true);
            m_writer.AutoFlush = true;
            //m_writer.WriteLine("Log started [" + m_logStart.ToLongDateString() + " " + m_logStart.ToLongTimeString() + "]");
        }

        public byte Level
        {
            get { return m_LogLevel; }
            set { m_LogLevel = value; }
        }
        private byte m_LogLevel;

        public void LogThis(string format, byte level, params object[] args)
        {
            LogThis(string.Format(format, args), level);
        }

        public void LogThis(string text, byte level)
        {
            lock (m_lock)
            {
                if (m_writer != null)
                {
                    m_writer.WriteLine(getLogDateString() + text);

                    //Log Level:
                    //0 = Aus
                    //1 = Errors
                    //2 = Errors & Warnings
                    //3 = Errors & Warnings & Info
                    //4 = Errors & Warnings & Info & Custom
                    if (m_LogLevel >= level)
                    {
                        switch (level)
                        {
                            case 1: //Error
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine(getPrintDateString() + text);
                                break;

                            case 2: //Warning
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine(getPrintDateString() + text);
                                break;

                            case 3: //Info
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.WriteLine(getPrintDateString() + text);
                                break;

                            case 4: //Debug/Custom Message
                                Console.ForegroundColor = ConsoleColor.Blue;
                                Console.WriteLine(getPrintDateString() + text);
                                break;
                        }
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }

                }
            }
        }

        public string getLogDateString()
        {
            var logTime = DateTime.Now;
            return string.Format("{0}\t{1}\t", logTime.ToString("dd-MM-yyyy"), logTime.ToString("HH:mm:ss"));
        }

        public string getPrintDateString()
        {
            var logTime = DateTime.Now;
            return string.Format("{0}\t", logTime.ToString("HH:mm:ss"));
        }


        public void Dispose()
        {
            lock (m_lock)
            {
                m_path = null;
                m_writer.Flush();
                m_writer.Close();
                m_writer.Dispose();
                m_writer = null;
            }
        }

    }
}
