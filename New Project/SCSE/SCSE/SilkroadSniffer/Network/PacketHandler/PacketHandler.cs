using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SilkroadSniffer.Network.PacketHandler
{
    public class PacketHandler
    {
        public static void NewPacketToHandle(phPacket packet)
        {
            try
            {
                Program.mainWindow.AddPacket(packet);
                switch (packet.Opcode)
                {
                    #region Client Opcodes

                    #endregion

                    #region Server Opcodes

                    #endregion

                    default:
                        Program.mainWindow.AddUnknowPacket(packet);
                        break;
                }
            }
            catch (System.Net.Sockets.SocketException se)
            {
                System.Windows.Forms.MessageBox.Show("SocketException:\nMsg:" + se.Message + "\nStack:\n" + se.StackTrace);
            }
            catch (Exception ex)
            {
                Program.mainWindow.AddFaildPacket(packet, ex);
            }
        }
    }
}
