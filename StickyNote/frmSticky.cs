using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace StickyNote
{
    public partial class frmSticky : Form
    {
        Database.csConString db = Database.csConString.Instance;
        csStickyNote stickyNote = csStickyNote.Instance;      

        private const int WM_NCHITTEST = 0x84;
        private const int HT_CLIENT = 0x1;
        private const int HT_CAPTION = 0x2;

        
        public frmSticky()
        {
            InitializeComponent();
        }

        private void frmSticky_MouseDown(object sender, MouseEventArgs e)
        {

        }
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == WM_NCHITTEST)
                m.Result = (IntPtr)(HT_CAPTION);
        }

        private void txtForm_TextChanged(object sender, EventArgs e)
        {
            

           
        }

        private void lblDelete_Click(object sender, EventArgs e)
        {
            
        }

        private void frmSticky_Load(object sender, EventArgs e)
        {
            checkIfDatabaseExists();
            reloadPreviousLocation();

            this.BackColor = Properties.Settings.Default.myColor;
            lblStatus.Text = "Application Start...";
        }

       
        private void checkIfDatabaseExists()
        {
            db.AppPath = Database.csConString.appData + @"\\Database\\sticky.sqlite";
            if (!File.Exists(db.AppPath))
            {
                db.createDatabase();
                stickyNote.createTable();
            }
            else
            {
                stickyNote.loadFileData(lvList);
            }
        }
        private void reloadPreviousLocation()
        {

            if ((ModifierKeys & Keys.Shift) == 0)
            {
                string initLocation = Properties.Settings.Default.InitialLocation;
                Point il = new Point(0, 0);
                Size sz = Size;
                if (!string.IsNullOrWhiteSpace(initLocation))
                {
                    string[] parts = initLocation.Split(',');
                    if (parts.Length >= 2)
                    {
                        il = new Point(int.Parse(parts[0]), int.Parse(parts[1]));
                    }
                    if (parts.Length >= 4)
                    {
                        sz = new Size(int.Parse(parts[2]), int.Parse(parts[3]));
                    }
                }
                Size = sz;
                Location = il;
            }
        }
        private void label5_Click(object sender, EventArgs e)
        {
            stickyNote.GetPositionInForm(this);
        }

        private void frmSticky_FormClosing(object sender, FormClosingEventArgs e)
        {
            if ((ModifierKeys & Keys.Shift) == 0)
            {
                Point location = Location;
                Size size = Size;
                if (WindowState != FormWindowState.Normal)
                {
                    location = RestoreBounds.Location;
                    size = RestoreBounds.Size;
                }
                string initLocation = string.Join(",", location.X, location.Y, size.Width, size.Height);
                Properties.Settings.Default.InitialLocation = initLocation;
                Properties.Settings.Default.Save();
            }
        }


        private void pbClose_Click(object sender, EventArgs e)
        {
            DialogResult rs = MessageBox.Show("Close Application?", "System Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (rs.Equals(DialogResult.Yes))
            {
                Application.Exit();
            }
        }

        private void pbAdd_Click(object sender, EventArgs e)
        {
            lvList.Visible = false;
            rtForm.Visible = true;
            pbBack.Visible = true;
            pbAdd.Visible = false;

            lblStatus.Text = "Add Note";
            rtForm.Text = "";
        }

        private void pbBack_Click(object sender, EventArgs e)
        {
            lvList.Visible = true;
            rtForm.Visible = false;
            pbBack.Visible = false;
            pbAdd.Visible = true;

            int x = stickyNote.countRecords();
            int id = x+1;

            if (rtForm.Text == "")
            {
                lblStatus.Text = "No data saved";
            }
            else
            {
                stickyNote.createRecord(id, rtForm.Text);
                rtForm.Text = "";
                stickyNote.loadFileData(lvList);
                lblStatus.Text = "Record Saved";
            }
        }

        private void lvList_MouseClick(object sender, MouseEventArgs e)
        {
            stickyNote.Id = Convert.ToInt32(lvList.FocusedItem.SubItems[0].Text);
        }

        private void lvList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            lvList.Visible = false;
            rtForm.Visible = true;
            pbBack.Visible = false;
            pbAdd.Visible = false;
            pbUpdate.Visible = true;

            lblStatus.Text = "Update Note id: " + stickyNote.Id.ToString();
            stickyNote.StringValue = lvList.FocusedItem.SubItems[1].Text;
            rtForm.Text = lvList.FocusedItem.SubItems[1].Text;
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lvList.SelectedItems.Count > 0)
            {
                DialogResult rs = MessageBox.Show("Delete record?", "System Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (rs.Equals(DialogResult.Yes))
                {
                    stickyNote.deleteRecord();
                    stickyNote.loadFileData(lvList);
                    lblStatus.Text = "Deleted Successfully.";
                }
            }
            else
            {
                lblStatus.Text = "No record is selected.";
            }
            
        }

        private void pbUpdate_Click(object sender, EventArgs e)
        {

            lvList.Visible = true;
            rtForm.Visible = false;
            pbUpdate.Visible = false;
            pbAdd.Visible = true;
                        
            if (rtForm.Text == stickyNote.StringValue)
            {
                lblStatus.Text = "No data saved";
            }
            else if(rtForm.Text == "")
            {
                lblStatus.Text = "No new value supplied.";
            }
            else
            {
                stickyNote.updateData(rtForm.Text);
                rtForm.Text = "";
                stickyNote.loadFileData(lvList);
                lblStatus.Text = "Record Updated";
            }
        }

        private void pbMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void pbColorChooser_Click(object sender, EventArgs e)
        {
            if (cDialog.ShowDialog() == DialogResult.OK)
            {
                this.BackColor = cDialog.Color;
                Properties.Settings.Default.myColor = cDialog.Color;
                Properties.Settings.Default.Save();
                lblStatus.Text = "Color settings updated.";
            }
        }

        
    }
}
