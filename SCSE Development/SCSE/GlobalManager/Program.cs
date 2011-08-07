using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using System.IO;

using Framework;

namespace GlobalManager
{
    public class Program
    {
        [MTAThread]
        public static void Main(string[] args)
        {
            Console.Title = "SR_GlobalManager";

            //Initilize instances
            Codes.root = Environment.CurrentDirectory;
            Codes.Logger = new cLog(Codes.root, "_GlobalManager");

            Mutex globalMutex = new Mutex(false, "SR_GlobalManager");
            if (globalMutex.WaitOne(0, false))
            {
                try
                {
                    cINI INI = new cINI(Codes.root + "\\Server.ini");
                    if (INI.Exists())
                    {
                        LoadSettings(INI);
                        //==============================//
                        Core.Server.Start();
                        //==============================//
                        string[] consoleInput;
                        do
                        {
                            consoleInput = Console.ReadLine().Split(' ');
                            switch (consoleInput[0])
                            {
                                //Ignore this commands
                                case "":
                                case "exit":
                                    break;
                                //==============================//
                                case "clear":
                                    Console.Clear();
                                    break;
                                case "close":
                                case "quit":
                                    Console.WriteLine("Do you mean 'exit' ?");
                                    break;

                                default:
                                    Console.WriteLine("The command '" + consoleInput[0] + "' is unknown.");
                                    break;
                            }

                        } while (consoleInput[0] != "exit");

                        consoleInput = null;
                        INI.Dispose();
                        INI = null;
                    }
                    else
                    {
                        throw new FileNotFoundException("Server.ini was not found.");
                    }
                }
                catch (Exception ex)
                {
                    Codes.Logger.LogThis(ex.Message, 1);

                }
                finally
                {
                    Console.WriteLine("The application has ended.\nPress any key to continue.");
                    Console.ReadKey();
                }
            }
            else // Already running.
            {
                Console.WriteLine("SR_GlobalManager is already executed.");
            }

            //Close all
            Core.Server.Shutdown();

            Codes.Logger.Dispose();
        }

        private static void LoadSettings(cINI INI)
        {
            Codes.Port = Convert.ToUInt16(INI.ReadInt("GlobalManager", "Port", 19000));
            Codes.Capacity = Convert.ToUInt16(INI.ReadInt("GlobalManager", "Capacity", 100));
            Codes.ServerPassword = INI.Read("GlobalManager", "Password", null);

            int CertCount = INI.ReadInt("GlobalManager", "ServerCount", 0);
            Codes.CertificationTable = new ushort[CertCount];

            for (int i = 0; i < CertCount; i++)
            {
                Codes.CertificationTable[i] = Convert.ToUInt16(INI.ReadInt("GlobalManager", "ServerId_" + (i + 1), 0));
            }
        }

    }
}
