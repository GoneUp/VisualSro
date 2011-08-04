using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;

namespace MapTool
{
    public class Program
    {
        public static MainWindow mainWindow;

        public static void Main(string[] args)
        {
            mainWindow = new MainWindow();
            //Application.EnableVisualStyles();
            Application.Run(mainWindow);

        }
    }
}
