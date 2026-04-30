using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using EduTrackPro.Data;
using EduTrackPro.StudentPortal;

namespace EduTrackPro.StudentPortal.Forms
{
    public class NotificationsForm : Form
    {
        private System.ComponentModel.IContainer components = null;
        private DataGridView dgvAlerts;
        private Panel header;
        private Label lblTitle;

        public NotificationsForm()
        {
            InitializeComponent();
            LoadNotifications();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.header = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.dgvAlerts = new System.Windows.Forms.DataGridView();

            ((System.ComponentModel.ISupportInitialize)(this.dgvAlerts)).BeginInit();
            this.header.SuspendLayout();
            this.SuspendLayout();

            // Header Panel
            this.header.BackColor = System.Drawing.Color.White;
            this.header.Dock = System.Windows.Forms.DockStyle.Top;
            this.header.Height = 72;
            this.header.Padding = new System.Windows.Forms.Padding(20);
            this.header.Name = "header";

            // Title Label
            this.lblTitle.Text = "Notifications";
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(34, 34, 34);
            this.lblTitle.Width = 420;
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblTitle.Name = "lblTitle";

            // DataGridView
            this.dgvAlerts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvAlerts.ReadOnly = true;
            this.dgvAlerts.AllowUserToAddRows = false;
            this.dgvAlerts.BackgroundColor = System.Drawing.Color.White;
            this.dgvAlerts.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvAlerts.RowHeadersVisible = false;
            this.dgvAlerts.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvAlerts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAlerts.Name = "dgvAlerts";

            
            this.header.Controls.Add(this.lblTitle);
            this.Controls.Add(this.dgvAlerts);
            this.Controls.Add(this.header);

           
            this.Text = "Notifications";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.BackColor = System.Drawing.Color.White;

            ((System.ComponentModel.ISupportInitialize)(this.dgvAlerts)).EndInit();
            this.header.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private void LoadNotifications()
        {
            // ── Determine current student (0 = show all / instructor context) ──
            int studentId = EduTrackPro.StudentPortal.Main_student.currentStudentId;

            DataTable dt = new DataTable();
            dt.Columns.Add("Date",    typeof(string));
            dt.Columns.Add("Type",    typeof(string));
            dt.Columns.Add("Message", typeof(string));

            var notifications = EduTrackPro.Data.DataService.Instance
                                    .GetStudentNotifications(studentId);

            foreach (var n in notifications)
            {
                // Format: "[Type] Title: Description"  or  "[Type] Target - Title: Desc"
                string type    = "Info";
                string message = n;
                string date    = DateTime.Now.ToString("g");

                if (n.Contains("[Warning]"))      { type = "Warning"; }
                else if (n.Contains("[Success]")) { type = "Success"; }
                else if (n.Contains("[Info]"))    { type = "Info"; }

                // Strip the type tag from the displayed message
                message = message
                    .Replace("[Warning] ", "")
                    .Replace("[Success] ", "")
                    .Replace("[Info] ",    "");

                dt.Rows.Add(date, type, message);
            }

            if (dt.Rows.Count == 0)
            {
                dt.Rows.Add(
                    DateTime.Now.ToString("g"),
                    "System",
                    "No notifications found.");
            }

            dgvAlerts.DataSource = dt;

            dgvAlerts.RowsDefaultCellStyle.Font           = new Font("Segoe UI", 9F);
            dgvAlerts.ColumnHeadersDefaultCellStyle.Font  = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgvAlerts.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            dgvAlerts.EnableHeadersVisualStyles = false;
        }
    }
}
