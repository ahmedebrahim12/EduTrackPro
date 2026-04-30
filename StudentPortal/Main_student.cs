using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using EduTrackPro.StudentPortal.Controls;

namespace EduTrackPro.StudentPortal
{
    public partial class Main_student : Form
    {
        private DashboardUserControl     dashboardView;
        private CoursesUserControl       coursesView;
        private ProfileUserControl       profileView;
        private NotificationsUserControl notificationsView;
        private CrystalReportControl     reportsView;

        public static int currentStudentId = 0;

        private Button[] _navButtons;

        public Main_student(int studentId)
        {
            currentStudentId = studentId;
            InitializeComponent();
            this.Load += Main_student_Load;

            // Wire nav buttons
            this.btnDashboard.Click     += BtnDashboard_Click;
            this.btnCourses.Click       += BtnCourses_Click;
            this.btnProfile.Click       += BtnProfile_Click;
            this.btnNotifications.Click += BtnNotifications_Click;
            this.btnReports.Click       += BtnReports_Click;

            _navButtons = new[] { btnDashboard, btnCourses, btnProfile, btnNotifications, btnReports };
            foreach (var b in _navButtons) StyleNormal(b);

            // Instantiate views once
            dashboardView     = new DashboardUserControl(studentId)     { Dock = DockStyle.Fill };
            coursesView       = new CoursesUserControl(studentId)       { Dock = DockStyle.Fill };
            profileView       = new ProfileUserControl(studentId)       { Dock = DockStyle.Fill };
            notificationsView = new NotificationsUserControl(studentId) { Dock = DockStyle.Fill };
            reportsView       = new CrystalReportControl()              { Dock = DockStyle.Fill };
        }

        private void Main_student_Load(object sender, EventArgs e)
        {
            if (!DesignMode && LicenseManager.UsageMode != LicenseUsageMode.Designtime)
                ShowView(dashboardView, btnDashboard);
        }

        // ── Navigation ───────────────────────────────────────────────────────
        private void ShowView(UserControl view, Button active)
        {
            this.contentPanel.Controls.Clear();
            this.contentPanel.Controls.Add(view);
            HighlightButton(active);
        }

        private void BtnDashboard_Click(object sender, EventArgs e)     => ShowView(dashboardView,     btnDashboard);
        private void BtnCourses_Click(object sender, EventArgs e)       { coursesView.LoadCourses(); ShowView(coursesView, btnCourses); }
        private void BtnProfile_Click(object sender, EventArgs e)       { profileView.BuildProfile(); ShowView(profileView, btnProfile); }
        private void BtnNotifications_Click(object sender, EventArgs e) => ShowView(notificationsView, btnNotifications);
        private void BtnReports_Click(object sender, EventArgs e)       { reportsView.LoadStudentAttendanceReport(currentStudentId); ShowView(reportsView, btnReports); }

        // ── Sidebar Styling ──────────────────────────────────────────────────
        private void StyleNormal(Button b)
        {
            b.BackColor = Color.White;
            b.ForeColor = Color.FromArgb(45, 45, 45);
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.Cursor = Cursors.Hand;
        }

        private void StyleActive(Button b)
        {
            b.BackColor = Color.FromArgb(47, 85, 151);
            b.ForeColor = Color.White;
        }

        private void HighlightButton(Button active)
        {
            foreach (var b in _navButtons) StyleNormal(b);
            StyleActive(active);
        }

        // Mouse hover effects
        private void HoverEnter(Button b, Button activeCheck, UserControl activeView)
        {
            if (this.contentPanel.Controls.Contains(activeView)) return;
            b.BackColor = Color.FromArgb(240, 242, 245);
            b.ForeColor = Color.FromArgb(47, 85, 151);
        }
        private void HoverLeave(Button b, UserControl activeView)
        {
            if (this.contentPanel.Controls.Contains(activeView)) return;
            StyleNormal(b);
        }

        private void btnDashboard_MouseEnter(object sender, EventArgs e)     => HoverEnter(btnDashboard, btnDashboard, dashboardView);
        private void btnDashboard_MouseLeave(object sender, EventArgs e)     => HoverLeave(btnDashboard, dashboardView);
        private void BtnCourses_MouseEnter(object sender, EventArgs e)       => HoverEnter(btnCourses, btnCourses, coursesView);
        private void BtnCourses_MouseLeave(object sender, EventArgs e)       => HoverLeave(btnCourses, coursesView);
        private void BtnProfile_MouseEnter(object sender, EventArgs e)       => HoverEnter(btnProfile, btnProfile, profileView);
        private void BtnProfile_MouseLeave(object sender, EventArgs e)       => HoverLeave(btnProfile, profileView);
        private void btnNotifications_MouseEnter(object sender, EventArgs e) => HoverEnter(btnNotifications, btnNotifications, notificationsView);
        private void btnNotifications_MouseLeave(object sender, EventArgs e) => HoverLeave(btnNotifications, notificationsView);
        private void btnReports_MouseEnter(object sender, EventArgs e)       => HoverEnter(btnReports, btnReports, reportsView);
        private void btnReports_MouseLeave(object sender, EventArgs e)       => HoverLeave(btnReports, reportsView);

        // ── Logout ───────────────────────────────────────────────────────────
        private void BtnLogout_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "Are you sure you want to logout?", "Logout",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                currentStudentId = 0;
                this.Close();
            }
        }

        // Unused stubs kept for designer compatibility
        private void Title_panal_Paint(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
        private void btnDashboard_Click_1(object sender, EventArgs e) { }
    }
}
