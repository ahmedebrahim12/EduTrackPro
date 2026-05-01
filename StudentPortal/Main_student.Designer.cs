using System.Drawing;
using System.Windows.Forms;

namespace EduTrackPro.StudentPortal
{
    partial class Main_student
    {
        private System.ComponentModel.IContainer components;
        private Panel panelSidebar;
        private Label lblAppTitle;
        private Button btnDashboard;
        private Button btnCourses;
        private Button btnProfile;
        private Button btnNotifications;
        private Panel contentPanel;
        private Button btnLogout;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.panelSidebar     = new System.Windows.Forms.Panel();
            this.btnReports       = new System.Windows.Forms.Button();
            this.btnDashboard     = new System.Windows.Forms.Button();
            this.btnCourses       = new System.Windows.Forms.Button();
            this.btnProfile       = new System.Windows.Forms.Button();
            this.btnNotifications = new System.Windows.Forms.Button();
            this.lblAppTitle      = new System.Windows.Forms.Label();
            this.contentPanel     = new System.Windows.Forms.Panel();
            this.btnLogout        = new System.Windows.Forms.Button();
            this.panelSidebar.SuspendLayout();
            this.SuspendLayout();

            // panelSidebar
            this.panelSidebar.BackColor = System.Drawing.Color.White;
            this.panelSidebar.Controls.Add(this.btnReports);
            this.panelSidebar.Controls.Add(this.btnLogout);
            this.panelSidebar.Controls.Add(this.btnNotifications);
            this.panelSidebar.Controls.Add(this.btnProfile);
            this.panelSidebar.Controls.Add(this.btnCourses);
            this.panelSidebar.Controls.Add(this.btnDashboard);
            this.panelSidebar.Controls.Add(this.lblAppTitle);
            this.panelSidebar.Dock     = System.Windows.Forms.DockStyle.Left;
            this.panelSidebar.Location = new System.Drawing.Point(0, 0);
            this.panelSidebar.Name     = "panelSidebar";
            this.panelSidebar.Size     = new System.Drawing.Size(220, 650);
            this.panelSidebar.TabIndex = 0;

            // lblAppTitle
            this.lblAppTitle.Dock      = System.Windows.Forms.DockStyle.Top;
            this.lblAppTitle.Font      = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblAppTitle.ForeColor = System.Drawing.Color.FromArgb(47, 85, 151);
            this.lblAppTitle.Name      = "lblAppTitle";
            this.lblAppTitle.Size      = new System.Drawing.Size(220, 96);
            this.lblAppTitle.TabIndex  = 0;
            this.lblAppTitle.Text      = "EduTrack Pro";
            this.lblAppTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // btnDashboard
            this.btnDashboard.BackColor              = System.Drawing.Color.White;
            this.btnDashboard.Cursor                 = System.Windows.Forms.Cursors.Hand;
            this.btnDashboard.FlatAppearance.BorderSize = 0;
            this.btnDashboard.FlatStyle              = System.Windows.Forms.FlatStyle.Flat;
            this.btnDashboard.ForeColor              = System.Drawing.Color.FromArgb(45, 45, 45);
            this.btnDashboard.Location               = new System.Drawing.Point(16, 110);
            this.btnDashboard.Name                   = "btnDashboard";
            this.btnDashboard.Size                   = new System.Drawing.Size(188, 50);
            this.btnDashboard.TabIndex               = 1;
            this.btnDashboard.Text                   = "🏠  Dashboard";
            this.btnDashboard.TextAlign              = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDashboard.UseVisualStyleBackColor = false;
            this.btnDashboard.Click      += new System.EventHandler(this.BtnDashboard_Click);
            this.btnDashboard.MouseEnter += new System.EventHandler(this.btnDashboard_MouseEnter);
            this.btnDashboard.MouseLeave += new System.EventHandler(this.btnDashboard_MouseLeave);

            // btnCourses
            this.btnCourses.BackColor              = System.Drawing.Color.White;
            this.btnCourses.Cursor                 = System.Windows.Forms.Cursors.Hand;
            this.btnCourses.FlatAppearance.BorderSize = 0;
            this.btnCourses.FlatStyle              = System.Windows.Forms.FlatStyle.Flat;
            this.btnCourses.ForeColor              = System.Drawing.Color.FromArgb(45, 45, 45);
            this.btnCourses.Location               = new System.Drawing.Point(16, 168);
            this.btnCourses.Name                   = "btnCourses";
            this.btnCourses.Size                   = new System.Drawing.Size(188, 50);
            this.btnCourses.TabIndex               = 2;
            this.btnCourses.Text                   = "📚  My Courses";
            this.btnCourses.TextAlign              = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCourses.UseVisualStyleBackColor = false;
            this.btnCourses.Click      += new System.EventHandler(this.BtnCourses_Click);
            this.btnCourses.MouseEnter += new System.EventHandler(this.BtnCourses_MouseEnter);
            this.btnCourses.MouseLeave += new System.EventHandler(this.BtnCourses_MouseLeave);

            // btnProfile
            this.btnProfile.BackColor              = System.Drawing.Color.White;
            this.btnProfile.Cursor                 = System.Windows.Forms.Cursors.Hand;
            this.btnProfile.FlatAppearance.BorderSize = 0;
            this.btnProfile.FlatStyle              = System.Windows.Forms.FlatStyle.Flat;
            this.btnProfile.ForeColor              = System.Drawing.Color.FromArgb(45, 45, 45);
            this.btnProfile.Location               = new System.Drawing.Point(16, 226);
            this.btnProfile.Name                   = "btnProfile";
            this.btnProfile.Size                   = new System.Drawing.Size(188, 50);
            this.btnProfile.TabIndex               = 3;
            this.btnProfile.Text                   = "👤  Profile";
            this.btnProfile.TextAlign              = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnProfile.UseVisualStyleBackColor = false;
            this.btnProfile.Click      += new System.EventHandler(this.BtnProfile_Click);
            this.btnProfile.MouseEnter += new System.EventHandler(this.BtnProfile_MouseEnter);
            this.btnProfile.MouseLeave += new System.EventHandler(this.BtnProfile_MouseLeave);

            // btnNotifications
            this.btnNotifications.BackColor              = System.Drawing.Color.White;
            this.btnNotifications.Cursor                 = System.Windows.Forms.Cursors.Hand;
            this.btnNotifications.FlatAppearance.BorderSize = 0;
            this.btnNotifications.FlatStyle              = System.Windows.Forms.FlatStyle.Flat;
            this.btnNotifications.ForeColor              = System.Drawing.Color.FromArgb(45, 45, 45);
            this.btnNotifications.Location               = new System.Drawing.Point(16, 284);
            this.btnNotifications.Name                   = "btnNotifications";
            this.btnNotifications.Size                   = new System.Drawing.Size(188, 50);
            this.btnNotifications.TabIndex               = 4;
            this.btnNotifications.Text                   = "🔔  Notifications";
            this.btnNotifications.TextAlign              = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNotifications.UseVisualStyleBackColor = false;
            this.btnNotifications.Click      += new System.EventHandler(this.BtnNotifications_Click);
            this.btnNotifications.MouseEnter += new System.EventHandler(this.btnNotifications_MouseEnter);
            this.btnNotifications.MouseLeave += new System.EventHandler(this.btnNotifications_MouseLeave);

            // btnReports
            this.btnReports.BackColor              = System.Drawing.Color.White;
            this.btnReports.Cursor                 = System.Windows.Forms.Cursors.Hand;
            this.btnReports.FlatAppearance.BorderSize = 0;
            this.btnReports.FlatStyle              = System.Windows.Forms.FlatStyle.Flat;
            this.btnReports.ForeColor              = System.Drawing.Color.FromArgb(45, 45, 45);
            this.btnReports.Location               = new System.Drawing.Point(16, 342);
            this.btnReports.Name                   = "btnReports";
            this.btnReports.Size                   = new System.Drawing.Size(188, 50);
            this.btnReports.TabIndex               = 6;
            this.btnReports.Text                   = "📊  Reports";
            this.btnReports.TextAlign              = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnReports.UseVisualStyleBackColor = false;
            this.btnReports.Click      += new System.EventHandler(this.BtnReports_Click);
            this.btnReports.MouseEnter += new System.EventHandler(this.btnReports_MouseEnter);
            this.btnReports.MouseLeave += new System.EventHandler(this.btnReports_MouseLeave);

            // btnLogout
            this.btnLogout.BackColor              = System.Drawing.Color.FromArgb(220, 53, 69);
            this.btnLogout.Cursor                 = System.Windows.Forms.Cursors.Hand;
            this.btnLogout.FlatAppearance.BorderSize = 0;
            this.btnLogout.FlatStyle              = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogout.ForeColor              = System.Drawing.Color.White;
            this.btnLogout.Font                   = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnLogout.Location               = new System.Drawing.Point(16, 520);
            this.btnLogout.Name                   = "btnLogout";
            this.btnLogout.Size                   = new System.Drawing.Size(188, 45);
            this.btnLogout.TabIndex               = 5;
            this.btnLogout.Text                   = "Logout";
            this.btnLogout.UseVisualStyleBackColor = false;
            this.btnLogout.Click += new System.EventHandler(this.BtnLogout_Click);

            // contentPanel
            this.contentPanel.Dock     = System.Windows.Forms.DockStyle.Fill;
            this.contentPanel.Location = new System.Drawing.Point(220, 0);
            this.contentPanel.Name     = "contentPanel";
            this.contentPanel.Size     = new System.Drawing.Size(792, 650);
            this.contentPanel.TabIndex = 3;

            // Main_student
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize          = new System.Drawing.Size(1012, 650);
            this.ControlBox          = false;
            this.Controls.Add(this.contentPanel);
            this.Controls.Add(this.panelSidebar);
            this.FormBorderStyle     = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name                = "Main_student";
            this.Text                = "EduTrack Pro - Student Portal";
            this.panelSidebar.ResumeLayout(false);
            this.ResumeLayout(false);
        }
        private Button btnReports;
    }
}
