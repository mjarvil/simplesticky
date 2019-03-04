using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
using System.Windows.Forms;
using System.Data;

namespace StickyNote.Database
{
    class csConString
    {
        private static csConString instance = null;
        private static readonly object padlock = new object();

        csConString()
        {
        }

        public static csConString Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new csConString();
                    }
                    return instance;
                }
            }
        }

        private string appPath;

        public string AppPath
        {
            get { return appPath; }
            set { appPath = value; }
        }
        
        

        

        public static SQLiteConnection getConn()
        {
            return new SQLiteConnection("Data Source = "+ Application.StartupPath+"\\Database\\sticky.sqlite;Version=3");
        }
        public void createDatabase()
        {
            
            string folderName = Application.StartupPath + @"\\Database";
            System.IO.Directory.CreateDirectory(folderName);
            SQLiteConnection.CreateFile(Application.StartupPath + @"\\Database\\sticky.sqlite");

        }

       

       


    }
}
