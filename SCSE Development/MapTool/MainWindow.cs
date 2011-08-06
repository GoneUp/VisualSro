using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MapTool
{
    public partial class MainWindow : Form
    {
        ChildForms.StartWindow startWindow;

        public MainWindow()
        {
            InitializeComponent();
            startWindow = new ChildForms.StartWindow();
        }

        private void MainWindow_Shown(object sender, EventArgs e)
        {
            picMap.Hide();
            startWindow.MdiParent = this;            
            startWindow.Show();
        }

    }
}
