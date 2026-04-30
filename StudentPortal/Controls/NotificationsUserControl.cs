using EduTrackPro.Data;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using EduTrackPro.StudentPortal.Controls;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;

namespace EduTrackPro.StudentPortal.Controls
{
    public class NotificationsUserControl : UserControl
    {
        private System.ComponentModel.IContainer components = null;
        private FlowLayoutPanel flowLayout;
        private Panel header;
        private Label lblTitle;

        private int _studentId;

        public NotificationsUserControl(int studentId = 0)
        {
            _studentId = studentId;
            InitializeComponent();
            if (!DesignMode && LicenseManager.UsageMode != LicenseUsageMode.Designtime)
            {
                this.Load += NotificationsUserControl_Load;
            }
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
            this.flowLayout = new System.Windows.Forms.FlowLayoutPanel();
            this.header.SuspendLayout();
            this.SuspendLayout();
            
            // header
            this.header.BackColor = System.Drawing.Color.White;
            this.header.Dock = System.Windows.Forms.DockStyle.Top;
            this.header.Height = 72;
            this.header.Padding = new System.Windows.Forms.Padding(20);
            this.header.Name = "header";
            
            // lblTitle
            this.lblTitle.Text = "Notifications";
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(34, 34, 34);
            this.lblTitle.Width = 420;
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblTitle.Name = "lblTitle";
            
            // flowLayout
            this.flowLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayout.AutoScroll = true;
            this.flowLayout.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayout.WrapContents = false;
            this.flowLayout.BackColor = System.Drawing.Color.FromArgb(250, 250, 250);
            this.flowLayout.Padding = new System.Windows.Forms.Padding(24);
            this.flowLayout.Name = "flowLayout";
            
            // Controls
            this.header.Controls.Add(this.lblTitle);
            this.Controls.Add(this.flowLayout);
            this.Controls.Add(this.header);
            
            this.Name = "NotificationsUserControl";
            this.Size = new System.Drawing.Size(800, 600);
            this.BackColor = System.Drawing.Color.White;
            
            this.header.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private void NotificationsUserControl_Load(object sender, EventArgs e)
        {
            LoadNotificationsData();
        }

        private void LoadNotificationsData()
        {
            this.flowLayout.SuspendLayout();
            this.flowLayout.Controls.Clear();

            int cardWidth = Math.Max(600, Math.Max(0, this.flowLayout.ClientSize.Width - 48));

            int currentId = _studentId > 0 ? _studentId : Main_student.currentStudentId;
            var notifications = DataService.Instance.GetStudentNotifications(currentId);

            foreach (var rawNotif in notifications)
            {
                string[] parts = rawNotif.Split('|');
                string timeDisplay = "Just now";
                string notifStr = rawNotif;

                if (parts.Length > 1) {
                    if (DateTime.TryParse(parts[0], out DateTime dt)) {
                        var span = DateTime.Now - dt;
                        if (span.TotalMinutes < 1) timeDisplay = "Just now";
                        else if (span.TotalHours < 1) timeDisplay = $"{(int)span.TotalMinutes} mins ago";
                        else if (span.TotalDays < 1) timeDisplay = $"{(int)span.TotalHours} hours ago";
                        else timeDisplay = dt.ToString("MMM dd");
                    }
                    notifStr = parts[1];
                }

                string title = "Notification";
                string description = notifStr;
                string courseName = "";
                NotificationType notifType = NotificationType.Info;

                if (notifStr.Contains("[Success]")) notifType = NotificationType.Success;
                else if (notifStr.Contains("[Warning]")) notifType = NotificationType.Warning;

                NotificationCard card = new NotificationCard();
                card.Width = cardWidth;
                card.SetNotification(title, description, courseName, notifType, timeDisplay, "");
                
                this.flowLayout.Controls.Add(card);
            }

            this.flowLayout.ResumeLayout();
        }
    }
}

