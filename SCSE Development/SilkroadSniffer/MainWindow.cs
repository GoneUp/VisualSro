using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SilkroadSniffer
{
    public partial class MainWindow : Form
    {
        Framework.cINI opcodesINI;
        Framework.cLog PacketLog;
        Framework.cLog UnknowPacketLog;
        Framework.cLog FaildPacketLog;

        public MainWindow()
        {
            InitializeComponent();
            PacketLog = new Framework.cLog(Application.StartupPath, "Packets");
            UnknowPacketLog = new Framework.cLog(Application.StartupPath, "UnknowPackets");
            FaildPacketLog = new Framework.cLog(Application.StartupPath, "FaildPackets");
            CheckForIllegalCrossThreadCalls = false;
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            LoadOpcodeList();
        }

        private void LoadOpcodeList()
        {
            opcodesINI = new Framework.cINI(Application.StartupPath + "\\Opcodes.ini");
            var opcodes = opcodesINI.GetSections();
            foreach (var opcode in opcodes)
            {
                if (opcode == opcodesINI.Read(opcode, "Opcode", "None"))
                {
                    string Name = opcodesINI.Read(opcode, "Name", "Unknow");
                    string Type = opcodesINI.Read(opcode, "Type", "Unknow");
                    string Modifier = opcodesINI.Read(opcode, "Modifier", "None");

                    var lvOpcode = new ListViewItem(opcode);
                    lvOpcode.SubItems.Add(Type);
                    lvOpcode.SubItems.Add(Name);
                    lvOpcode.SubItems.Add(Modifier);
                    lvOpcodeList.Items.Insert(0, lvOpcode);
                }
            }
        }

        private void contextMenuOpcodeList_Opening(object sender, CancelEventArgs e)
        {
            if (lvOpcodeList.SelectedItems.Count == 1)
            {
                var item = lvOpcodeList.SelectedItems[0];
                opcodeToolStripMenuItem.Text = "Opcode: " + item.Text;

                toolStripComboType.SelectedItem = item.SubItems[1].Text;

                toolStripTxtName.Text = item.SubItems[2].Text;

                switch (item.SubItems[3].Text)
                {
                    case "None":
                        noneToolStripMenuItem.Checked = true;
                        break;
                    case "Listen":
                        listenToolStripMenuItem.Checked = true;
                        break;
                    case "Ignore":
                        ignoreToolStripMenuItem.Checked = true;
                        break;
                }
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void noneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listenToolStripMenuItem.Checked)
            {
                listenToolStripMenuItem.Checked = false;
            }
            if (ignoreToolStripMenuItem.Checked)
            {
                ignoreToolStripMenuItem.Checked = false;
            }
            //noneToolStripMenuItem.Checked = true;
            if (lvOpcodeList.SelectedItems.Count == 1)
            {
                var item = lvOpcodeList.SelectedItems[0];
                item.SubItems[3].Text = "None";
                opcodesINI.Write(item.Text, "Modifier", "None");
            }
        }

        private void listenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (noneToolStripMenuItem.Checked)
            {
                noneToolStripMenuItem.Checked = false;
            }
            if (ignoreToolStripMenuItem.Checked)
            {
                ignoreToolStripMenuItem.Checked = false;
            }
            //listenToolStripMenuItem.Checked = true;
            if (lvOpcodeList.SelectedItems.Count == 1)
            {
                var item = lvOpcodeList.SelectedItems[0];
                item.SubItems[3].Text = "Listen";
                opcodesINI.Write(item.Text, "Modifier", "Listen");
            }
        }

        private void ignoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (noneToolStripMenuItem.Checked)
            {
                noneToolStripMenuItem.Checked = false;
            }
            if (listenToolStripMenuItem.Checked)
            {
                listenToolStripMenuItem.Checked = false;
            }
            //ignoreToolStripMenuItem.Checked = true;
            if (lvOpcodeList.SelectedItems.Count == 1)
            {
                var item = lvOpcodeList.SelectedItems[0];
                item.SubItems[3].Text = "Ignore";
                opcodesINI.Write(item.Text, "Modifier", "Ignore");
            }
        }

        private void toolStripComboType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvOpcodeList.SelectedItems.Count == 1)
            {
                var item = lvOpcodeList.SelectedItems[0];
                item.SubItems[1].Text = (string)toolStripComboType.SelectedItem;
                opcodesINI.Write(item.Text, "Type", (string)toolStripComboType.SelectedItem);
            }
        }

        private void toolStripTxtName_TextChanged(object sender, EventArgs e)
        {
            if (toolStripTxtName.Focused == true)
            {
                if (lvOpcodeList.SelectedItems.Count == 1)
                {
                    var item = lvOpcodeList.SelectedItems[0];
                    if (string.IsNullOrEmpty(toolStripTxtName.Text) == false && string.IsNullOrWhiteSpace(toolStripTxtName.Text) == false)
                    {
                        item.SubItems[2].Text = toolStripTxtName.Text;
                        opcodesINI.Write(item.Text, "Name", toolStripTxtName.Text);
                    }
                    else
                    {
                        item.SubItems[2].Text = "Unknow";
                        opcodesINI.Write(item.Text, "Name", "Unknow");
                    }
                }
            }
        }

        public void AddPacket(phPacket packet)
        {
            string type = "Unknow";
            bool enrypted = false;
            switch (packet.SecurityCount)
            {
                case 1:
                    type = "C->S";
                    break;
                case 2: //Server
                    type = "S->C";
                    break;
                case 3:
                    type = "C->S";
                    enrypted = true;
                    break;
                case 4:
                    type = "S->C";
                    enrypted = true;
                    break;
            }
            PacketLog.LogThis(string.Format("Opcode:{0} Type:{1} Encrypted:{2} Data:{3}", packet.OpcodeHEX, type, enrypted, packet.ToString()), 0);

            //Add to OpcodeList
            var opcode = lvOpcodeList.FindItemWithText(packet.OpcodeHEX, true, 0);
            if (opcode == null)
            {
                var lvOpcode = new ListViewItem(packet.OpcodeHEX);
                lvOpcode.SubItems.Add(type);
                lvOpcode.SubItems.Add("Unknow");
                lvOpcode.SubItems.Add("None");
                lvOpcodeList.Items.Insert(0, lvOpcode);

                //Write INI
                opcodesINI.Write(packet.OpcodeHEX, "Opcode", packet.OpcodeHEX);
                opcodesINI.Write(packet.OpcodeHEX, "Name", "Unknow");
                opcodesINI.Write(packet.OpcodeHEX, "Type", type);
                opcodesINI.Write(packet.OpcodeHEX, "Modifier", "None");
            }

            //Add to PacketList
            switch (type)
            {
                case "S->C":
                    if (checkServerPackets.Checked)
                    {
                        var lvPacket = new ListViewItem(packet.OpcodeHEX);
                        lvPacket.SubItems.Add(type);
                        if (opcode == null)
                        {
                            lvPacket.SubItems.Add("Unknow"); //Name
                            lvPacket.SubItems.Add(packet.ToString());
                            lvPacketList.Items.Insert(0, lvPacket);
                        }
                        else
                        {
                            //Get Name
                            lvPacket.SubItems.Add(opcode.SubItems[2].Text); //Name
                            lvPacket.SubItems.Add(packet.ToString());

                            //Check for modifier
                            var listenOpcodes = lvOpcodeList.Items.Find("Listen", true);
                            if (listenOpcodes.Length == 0)
                            {
                                if (opcode.SubItems[3].Text == "Ignore")
                                {
                                    lvPacket = null;
                                    packet.Dispose();
                                    packet = null;
                                }
                                else //Dont ignore
                                {
                                    lvPacketList.Items.Insert(0, lvPacket);
                                }
                            }
                            else //Listen mode is active
                            {
                                if (opcode.SubItems[3].Text == "Listen")
                                {
                                    lvPacketList.Items.Insert(0, lvPacket);
                                }
                            }
                        }

                    }
                    break;
                case "C->S":
                    if (checkClientPackets.Checked)
                    {
                        var lvPacket = new ListViewItem(packet.OpcodeHEX);
                        lvPacket.SubItems.Add(type);
                        if (opcode == null)
                        {
                            lvPacket.SubItems.Add("Unknow"); //Name
                            lvPacket.SubItems.Add(packet.ToString());
                            lvPacketList.Items.Insert(0, lvPacket);
                        }
                        else
                        {
                            //Get Name
                            lvPacket.SubItems.Add(opcode.SubItems[2].Text); //Name
                            lvPacket.SubItems.Add(packet.ToString());

                            //Check for modifier
                            var listenOpcodes = lvOpcodeList.Items.Find("Listen", true);
                            if (listenOpcodes.Length == 0)
                            {
                                if (opcode.SubItems[3].Text == "Ignore")
                                {
                                    lvPacket = null;
                                    packet.Dispose();
                                    packet = null;
                                }
                                else //Dont ignore
                                {
                                    lvPacketList.Items.Insert(0, lvPacket);
                                }
                            }
                            else //Listen mode is active
                            {
                                if (opcode.SubItems[3].Text == "Listen")
                                {
                                    lvPacketList.Items.Insert(0, lvPacket);
                                }
                            }
                        }
                    }
                    break;
                case "None":
                    Console.Beep();
                    break;
            }
        }

        public void AddUnknowPacket(phPacket packet)
        {

        }

        public void AddFaildPacket(phPacket packet, Exception ex)
        {

        }

        public void UpdateConnectionStatus(string status)
        {
            groupBoxConnection.Text = "Connection - " + status;
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (checkClientless.Checked == false)
            {
                if (Network.ProxyConnection.Connect(Convert.ToUInt16(txtProxyPort.Text)))
                {
                    groupBoxConnection.Text = "Connection - Connected";
                }
                else
                {
                    groupBoxConnection.Text = "Connection - Faild";
                }
            }
            else //Clientless
            {

            }
        }

        System.Diagnostics.Process phConnector;
        private void btnStartphConnector_Click(object sender, EventArgs e)
        {
            if (phConnector != null)
            {
                phConnector.Kill();
                phConnector = null;
                btnStartphConnector.Text = "Start phConnector";
            }
            else
            {
                phConnector = new System.Diagnostics.Process();
                phConnector.StartInfo.FileName = Application.StartupPath + "\\phConnector.exe";
                phConnector.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                phConnector.Start();
                btnStartphConnector.Text = "Kill phConnector";
            }
        }

    }
}
