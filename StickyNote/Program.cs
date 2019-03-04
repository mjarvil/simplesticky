using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;


namespace StickyNote
{
    static class Program
    {
        static Mutex mutex = new Mutex(false, "3132");
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            if (!mutex.WaitOne(TimeSpan.FromSeconds(2), false))
            {
                //MessageBox.Show("Application already started!", "", MessageBoxButtons.OK);
                return;
            }

            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new frmSticky());
            }
            finally { mutex.ReleaseMutex(); } // I find this more explicit
            
        }
    }
}
