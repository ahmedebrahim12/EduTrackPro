using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using EduTrackPro.Components;
using EduTrackPro.Data;

namespace EduTrackPro
{
    public partial class MainForm : Form
    {
        private TableLayoutPanel mainLayout;
        private Panel sidebar;
        private Panel contentPanel;
        private Panel activeIndicator;
        private Button? currentActiveButton;

        public static int currentInstructorId = 0;
        public static string currentInstructorName = "Instructor";

        public MainForm(int instructorId = 0)
        {
            currentInstructorId = instructorId;
            // Load instructor name from DB via DataService
            if (instructorId > 0)
                currentInstructorName = EduTrackPro.Data.DataService.Instance.GetInstructorName(instructorId);
            InitializeComponent();
            SetupLayout();
            
            this.FormClosing += (s, e) => {
                DataService.Instance.AddActivity("System shutdown cleanly");
            };

            ShowView(new DashboardView());
        }

        private void InitializeComponent()
        {
            this.Text = "EduTrack Pro - Instructor Portal";
            this.Size = new Size(1280, 800);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Theme.BackgroundGray;
            this.Font = Theme.BodyFont;
            this.DoubleBuffered = true;
        }

        private void SetupLayout()
        {
            mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = Theme.BackgroundGray
            };
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 260)); 
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100)); 
            this.Controls.Add(mainLayout);

            sidebar = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Theme.SidebarBg,
                Padding = new Padding(0, 20, 0, 0)
            };
            mainLayout.Controls.Add(sidebar, 0, 0);

            var contentContainer = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 2, BackColor = Theme.BackgroundGray };
            contentContainer.RowStyles.Add(new RowStyle(SizeType.Absolute, 70)); 
            contentContainer.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); 
            mainLayout.Controls.Add(contentContainer, 1, 0);

            var topBar = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(20, 15, 20, 15) };
            contentContainer.Controls.Add(topBar, 0, 0);

            var logoLabel = new Label { Text = "EduTrack Pro", Font = new Font("Segoe UI", 18, FontStyle.Bold), ForeColor = Theme.PrimaryBlue, Location = new Point(25, 20), AutoSize = true };
            sidebar.Controls.Add(logoLabel);

            // Top bar: welcome message + instructor name
            var lblWelcome = new Label {
                Text = $"Welcome, {currentInstructorName}",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Theme.PrimaryBlue,
                Dock = DockStyle.Left,
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleLeft
            };
            var lblDate = new Label {
                Text = DateTime.Now.ToString("dddd, MMMM dd yyyy"),
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.Gray,
                Dock = DockStyle.Right,
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleRight
            };
            topBar.Controls.Add(lblWelcome);
            topBar.Controls.Add(lblDate);

            activeIndicator = new Panel { BackColor = Theme.PrimaryBlue, Width = 5, Height = 35, Location = new Point(0, 100), Visible = false };
            sidebar.Controls.Add(activeIndicator);

            int y = 100;
            AddNavItem("Dashboard", y, () => new DashboardView());
            AddNavItem("My Courses", y += 55, () => new CoursesView());
            AddNavItem("Manage Sessions", y += 55, () => new SessionsView());
            AddNavItem("Take Attendance", y += 55, () => new AttendanceView());
            AddNavItem("Notifications", y += 55, () => new NotificationsView());
            AddNavItem("Reports", y += 55, () => new ReportsView());

            // Logout button pinned to bottom of sidebar
            var btnLogout = new Button
            {
                Text = "Logout",
                TextAlign = ContentAlignment.MiddleCenter,
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0, MouseOverBackColor = Color.FromArgb(200, 35, 50) },
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(220, 53, 69),
                Size = new Size(200, 45),
                Location = new Point(30, 640),
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left
            };
            
            // Add rounded corners to the button
            btnLogout.Paint += (s, e) => {
                var g = e.Graphics; g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                using (var path = new System.Drawing.Drawing2D.GraphicsPath()) {
                    int r = 10;
                    path.AddArc(0, 0, r, r, 180, 90);
                    path.AddArc(btnLogout.Width - r, 0, r, r, 270, 90);
                    path.AddArc(btnLogout.Width - r, btnLogout.Height - r, r, r, 0, 90);
                    path.AddArc(0, btnLogout.Height - r, r, r, 90, 90);
                    path.CloseFigure();
                    btnLogout.Region = new Region(path);
                }
            };
            btnLogout.Click += (s, e) =>
            {
                var result = MessageBox.Show(
                    "Are you sure you want to logout?",
                    "Logout",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    currentInstructorId = 0;
                    currentInstructorName = "Instructor";
                    this.Close();
                }
            };
            sidebar.Controls.Add(btnLogout);
            btnLogout.BringToFront();

            contentPanel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(40), BackColor = Theme.BackgroundGray, AutoScroll = true };
            contentContainer.Controls.Add(contentPanel, 0, 1);
        }

        private void AddNavItem(string text, int y, Func<UserControl> viewFactory)
        {
            var btn = new Button
            {
                Text = "      " + text,
                TextAlign = ContentAlignment.MiddleLeft,
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0, MouseOverBackColor = Theme.SecondaryBlue },
                Font = Theme.BodyFont,
                ForeColor = Theme.TextDark,
                Size = new Size(240, 45),
                Location = new Point(0, y),
                Cursor = Cursors.Hand
            };

            btn.Click += (s, e) => {
                UpdateActiveIndicator(btn);
                ShowView(viewFactory());
            };
            
            sidebar.Controls.Add(btn);
            btn.BringToFront();
        }

        private void UpdateActiveIndicator(Button btn)
        {
            activeIndicator.Visible = true;
            activeIndicator.Location = new Point(0, btn.Location.Y + 5);
            activeIndicator.BringToFront();

            if (currentActiveButton != null)
            {
                currentActiveButton.BackColor = Color.Transparent;
                currentActiveButton.ForeColor = Theme.TextDark;
            }
            btn.BackColor = Theme.SecondaryBlue;
            btn.ForeColor = Theme.PrimaryBlue;
            currentActiveButton = btn;
        }

        public void ShowView(UserControl view)
        {
            contentPanel.Controls.Clear();
            view.Dock = DockStyle.Fill;
            contentPanel.Controls.Add(view);
        }
    }

    public class BaseView : UserControl
    {
        public BaseView(string title, string subtitle)
        {
            this.BackColor = Theme.BackgroundGray;
            this.Padding = new Padding(0, 0, 40, 40);
            
            var lblTitle = new Label { Text = title, Font = new Font("Segoe UI", 24, FontStyle.Bold), ForeColor = Theme.TextDark, Location = new Point(0, 0), AutoSize = true };
            var lblSubtitle = new Label { Text = subtitle, Font = Theme.BodyFont, ForeColor = Theme.TextLight, Location = new Point(0, 45), AutoSize = true };
            
            this.Controls.Add(lblTitle);
            this.Controls.Add(lblSubtitle);
        }
    }
}
