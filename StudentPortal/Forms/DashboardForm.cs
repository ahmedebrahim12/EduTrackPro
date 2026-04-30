using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using EduTrackPro.StudentPortal.Controls;

namespace EduTrackPro.StudentPortal.Forms
{
    public partial class DashboardForm : Form
    {
        private System.ComponentModel.IContainer components = null;
        private Panel leftNav;
        private TableLayoutPanel mainLayout;
        private Button btnNotifications;
        private Label lblBrand;
        private readonly bool _showLeftNav;

        public DashboardForm()
        {
            _showLeftNav = true;
            InitializeComponent();
            ApplyRuntimeSettings();
        }

 
        public DashboardForm(bool showLeftNav)
        {
            _showLeftNav = showLeftNav;
            InitializeComponent();
            ApplyRuntimeSettings();
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
            this.leftNav = new System.Windows.Forms.Panel();
            this.lblBrand = new System.Windows.Forms.Label();
            this.btnNotifications = new System.Windows.Forms.Button();
            this.mainLayout = new System.Windows.Forms.TableLayoutPanel();

            this.leftNav.SuspendLayout();
            this.SuspendLayout();

            this.Text = "Student Dashboard";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Maximized;
            this.BackColor = Color.FromArgb(250, 250, 250);

            this.leftNav.Dock = DockStyle.Left;
            this.leftNav.Width = 220;
            this.leftNav.BackColor = Color.White;
            this.leftNav.Padding = new Padding(12);
            this.leftNav.Name = "leftNav";
            this.leftNav.Visible = true; 

            // Brand Label
            this.lblBrand.Text = "AttendanceTracker";
            this.lblBrand.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            this.lblBrand.ForeColor = Color.FromArgb(47, 85, 151);
            this.lblBrand.Height = 36;
            this.lblBrand.Dock = DockStyle.Top;
            this.lblBrand.TextAlign = ContentAlignment.MiddleLeft;
            this.lblBrand.Name = "lblBrand";

            // Notifications Button
            this.btnNotifications.Text = "Notifications";
            this.btnNotifications.Font = new Font("Segoe UI", 10F);
            this.btnNotifications.Width = 180;
            this.btnNotifications.Height = 38;
            this.btnNotifications.Location = new Point(12, 80);
            this.btnNotifications.BackColor = Color.White;
            this.btnNotifications.ForeColor = Color.FromArgb(45, 45, 45);
            this.btnNotifications.FlatStyle = FlatStyle.Flat;
            this.btnNotifications.FlatAppearance.BorderSize = 0;
            this.btnNotifications.Cursor = Cursors.Hand;
            this.btnNotifications.Name = "btnNotifications";
            this.btnNotifications.Click += new EventHandler(this.BtnNotifications_Click);
            this.btnNotifications.MouseEnter += new EventHandler(this.SidebarButton_MouseEnter);
            this.btnNotifications.MouseLeave += new EventHandler(this.SidebarButton_MouseLeave);

            this.mainLayout.Dock = DockStyle.Fill;
            this.mainLayout.Padding = new Padding(24);
            this.mainLayout.AutoScroll = true;
            this.mainLayout.ColumnCount = 1;
            this.mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            this.mainLayout.Name = "mainLayout";

            // Add controls
            this.leftNav.Controls.Add(this.btnNotifications);
            this.leftNav.Controls.Add(this.lblBrand);
            this.Controls.Add(this.mainLayout);
            this.Controls.Add(this.leftNav);

            this.leftNav.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private void ApplyRuntimeSettings()
        {
            if (leftNav != null)
            {
                leftNav.Visible = _showLeftNav;
            }

            if (mainLayout != null)
            {
                mainLayout.Padding = _showLeftNav ? new Padding(24) : new Padding(40);
            }

            if (!DesignMode && LicenseManager.UsageMode != LicenseUsageMode.Designtime)
            {
                this.Load += DashboardForm_Load;
            }
        }

        private void DashboardForm_Load(object sender, EventArgs e)
        {
            if (mainLayout != null && mainLayout.Controls.Count == 0)
            {
                PopulateCards();
            }
        }

        private void PopulateCards()
        {
            if (mainLayout == null) return;
            mainLayout.Controls.Clear();

            // Card 1
            var overall = new HorizontalCard
            {
                Title = "Overall Attendance",
                Height = 110,
                Dock = DockStyle.Top
            };
            overall.UpdateAttendance(87);
            AddCardToLayout(overall);

            // Card 2
            var courses = new HorizontalCard
            {
                Title = "Enrolled Courses",
                Height = 110,
                Dock = DockStyle.Top
            };
            courses.UpdateAttendance(83);
            AddCardToLayout(courses);

            // Card 3
            var analytics = new HorizontalCard
            {
                Title = "Latest Module Completion",
                Height = 110,
                Dock = DockStyle.Top
            };
            analytics.UpdateAttendance(72);
            AddCardToLayout(analytics);
        }

        private void AddCardToLayout(Control card)
        {
            mainLayout.RowCount += 1;
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 12F));
            Panel spacer = new Panel { Height = 12, Dock = DockStyle.Fill, BackColor = Color.Transparent };
            mainLayout.Controls.Add(spacer, 0, mainLayout.RowCount - 1);

            mainLayout.RowCount += 1;
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, card.Height));
            mainLayout.Controls.Add(card, 0, mainLayout.RowCount - 1);
        }

        private void BtnNotifications_Click(object sender, EventArgs e)
        {
            using (var nf = new NotificationsForm())
            {
                nf.ShowDialog(this);
            }
        }

        private void SidebarButton_MouseEnter(object sender, EventArgs e)
        {
            if (sender is Button b)
            {
                b.BackColor = Color.FromArgb(47, 85, 151);
                b.ForeColor = Color.White;
            }
        }

        private void SidebarButton_MouseLeave(object sender, EventArgs e)
        {
            if (sender is Button b)
            {
                b.BackColor = Color.White;
                b.ForeColor = Color.FromArgb(45, 45, 45);
            }
        }
    }
}
