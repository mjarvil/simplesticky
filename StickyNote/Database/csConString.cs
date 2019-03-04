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

        public static string appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        private string appPath;

        public string AppPath
        {
            get { return appPath; }
            set { appPath = value; }
        }
        
        

        

        public static SQLiteConnection getConn()
        {
            return new SQLiteConnection("Data Source = "+ csConString.appData +"\\Database\\sticky.sqlite;Version=3");
        }
        public void createDatabase()
        {

            string folderName = csConString.appData + @"\\Database";
            System.IO.Directory.CreateDirectory(folderName);
            SQLiteConnection.CreateFile(csConString.appData + @"\\Database\\sticky.sqlite");

        }

       

       


    }
}
