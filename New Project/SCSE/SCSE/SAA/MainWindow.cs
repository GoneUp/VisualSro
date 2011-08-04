using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace SAA
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;

            m_alarm1 = new System.Media.SoundPlayer(global::SAA.Properties.Resources.alarm);
            m_alarm2 = new System.Media.SoundPlayer(global::SAA.Properties.Resources.alarm2);
        }

        private System.Media.SoundPlayer m_alarm1;
        private System.Media.SoundPlayer m_alarm2;

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "Start")
            {
                button1.Text = "Stop";
                m_alarm1.PlayLooping();
                //m_alarm2.PlayLooping();
                currentFreq = startFreq;
                workerBeep.RunWorkerAsync();
            }
            else
            {
                button1.Text = "Start";
                m_alarm1.Stop();
                m_alarm2.Stop();
                if (thFinal != null)
                {
                    if (thFinal.IsAlive)
                    {
                        thFinal.Abort();
                    }
                }
                workerBeep.CancelAsync();
            }
        }

        int startFreq = 1000;
        int incrementFreq = 50;
        int currentFreq;

        Thread thFinal;

        private void workerBeep_DoWork(object sender, DoWorkEventArgs e)
        {
            for (progressBar1.Value = 0; progressBar1.Value < 100; progressBar1.Value += 5)
            {
                if (workerBeep.CancellationPending == false)
                {
                    Console.Beep(currentFreq += incrementFreq, 250);
                    System.Threading.Thread.Sleep(1000);
                    e.Result = true;
                }
                else
                {
                    e.Cancel = true;
                    return;
                }
            }

        }

        private void workerBeep_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressBar1.Value = 0;
            if (e.Cancelled == false)
            {
                m_alarm1.Stop();
                m_alarm2.PlayLooping();

                thFinal = new Thread(playMelodie);
                thFinal.IsBackground = true;
                thFinal.Start();
            }
        }

        public void playMelodie()
        {
            for (int i = 0; i < 9999; i++)
            {
                Console.Beep(2500, 1000);
                System.Threading.Thread.Sleep(250);
                Console.Beep(1500, 1000);
                System.Threading.Thread.Sleep(250);

            }
        }

    }
}
