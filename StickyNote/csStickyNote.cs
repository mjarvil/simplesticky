using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using System.Data;
using System.Data.SQLite;

namespace StickyNote
{
    class csStickyNote
    {
        SQLiteConnection conn = Database.csConString.getConn();

        private static csStickyNote instance = null;
        private static readonly object padlock = new object();

        csStickyNote()
        {
        }

        public static csStickyNote Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new csStickyNote();
                    }
                    return instance;
                }
            }
        }

        private string notePath = Application.StartupPath + @"\\Note.txt";

        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        

        public string NotePath
        {
            get { return notePath; }
            set { notePath = value; }
        }
        
        private string stringValue;

        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
        

        public void createRecord(int id, string value)
        {
            try
            {
                conn.Open();
                string query = "insert into records(id, note)values(@id,@value);";
                SQLiteCommand cmd = new SQLiteCommand(query,conn);
                cmd.Parameters.Add(new SQLiteParameter("@id", id));
                cmd.Parameters.Add(new SQLiteParameter("@value", value));
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception e)
            {
                conn.Close();
                MessageBox.Show(e.ToString());
            }
        }
        
        public void loadFileData(ListView lv)
        {
            try
            {
                conn.Open();
                string query = "select * from records";
                lv.Items.Clear();
                DataTable dataTable = new DataTable();
                DataSet dataSet = new DataSet();
                dataSet.Tables.Add(dataTable);
                SQLiteDataAdapter mySqlDataAdapter = new SQLiteDataAdapter(query, conn);
                mySqlDataAdapter.Fill(dataTable);
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    lv.Items.Add(dataRow[0].ToString());
                    lv.Items[lv.Items.Count - 1].SubItems.Add(dataRow[1].ToString());

                }
                conn.Close();
            }
            catch (Exception e)
            {
                conn.Close();
                MessageBox.Show(e.ToString());
            }
       
        }
        public void updateData(string value)
        {
            try
            {
                conn.Open();
                string query = "update records set note = @value where id = @id";
                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                cmd.Parameters.Add(new SQLiteParameter("@id", id));
                cmd.Parameters.Add(new SQLiteParameter("@value", value));
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception e)
            {
                conn.Close();
                MessageBox.Show(e.ToString());
            }
        }

        public Point GetPositionInForm(Control ctrl)
        {
            Point p = ctrl.Location;
            Control parent = ctrl.Parent;
            while (!(parent is Form))
            {
                p.Offset(parent.Location.X, parent.Location.Y);
                parent = parent.Parent;
            }
            return p;
        }

        public void deleteRecord()
        {
            try
            {
                conn.Open();
                string query = "delete from records where id = @id";
                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                cmd.Parameters.Add(new SQLiteParameter("@id", id));                
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception e)
            {
                conn.Close();
                MessageBox.Show(e.ToString());
            }
        }

         public void createTable()
        {
            try
            {
                string query = "create table records(id int(5),note text)";
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch(Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        public int countRecords()
         {
             int num=0;
             conn.Open();
             string query = "select count(id) as Max from records";
             SQLiteCommand cmd = new SQLiteCommand(query,conn);
             SQLiteDataReader dr = cmd.ExecuteReader();
             
                while(dr.Read())
                {
                    num = Convert.ToInt32(dr["Max"].ToString());
                }
                
             conn.Close();
             return num; 
         }
    }
}
