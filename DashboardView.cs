using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using EduTrackPro.Components;
using EduTrackPro.Data;

namespace EduTrackPro
{
    public class DashboardView : BaseView
    {
        private FlowLayoutPanel statsPanel;
        private FlowLayoutPanel listPanel;
        private FlowLayoutPanel alertPanel;

        public DashboardView() : base("Instructor Dashboard", "Welcome back! Here is your academic overview for today.")
        {
            SetupLayout();
            LoadData();
        }

        private void SetupLayout()
        {
            var container = new Panel {
                Location = new Point(0, 110),
                Size = new Size(this.Width, this.Height - 110),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom,
                Padding = new Padding(0, 0, 40, 40)
            };
            this.Controls.Add(container);

            var mainLayout = new TableLayoutPanel { 
                Dock = DockStyle.Fill, 
                ColumnCount = 2, 
                RowCount = 1
            };
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70));
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));
            container.Controls.Add(mainLayout);

            var leftPanel = new Panel { Dock = DockStyle.Fill };
            
            statsPanel = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 160, WrapContents = false };
            listPanel = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoScroll = true, FlowDirection = FlowDirection.TopDown, WrapContents = false };
            
            var lblRecent = new Label { Text = "Recent Activities", Font = Theme.SubHeaderFont, Dock = DockStyle.Top, Height = 60, TextAlign = ContentAlignment.BottomLeft };
            
            leftPanel.Controls.Add(listPanel);
            leftPanel.Controls.Add(lblRecent);
            leftPanel.Controls.Add(statsPanel);
            mainLayout.Controls.Add(leftPanel, 0, 0);

            var rightCard = new CustomCard { Dock = DockStyle.Fill, Margin = new Padding(20, 0, 0, 0) };
            var lblAlertsTitle = new Label { Text = "Important Alerts", Font = Theme.SubHeaderFont, Location = new Point(20, 20), AutoSize = true };
            
            alertPanel = new FlowLayoutPanel { 
                Location = new Point(15, 60), 
                Size = new Size(250, 500), 
                AutoScroll = true, 
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom
            };

            rightCard.Controls.Add(lblAlertsTitle);
            rightCard.Controls.Add(alertPanel);
            mainLayout.Controls.Add(rightCard, 1, 0);
        }

        private void LoadData()
        {
            statsPanel.Controls.Clear();
            statsPanel.Controls.Add(new StatCard("Total Courses", DataService.Instance.GetTotalCourses().ToString("D2"), Theme.PrimaryBlue, "📚"));
            statsPanel.Controls.Add(new StatCard("Active Students", DataService.Instance.GetActiveStudentsCount().ToString("D2"), Theme.SecondaryGreen, "👥"));
            statsPanel.Controls.Add(new StatCard("Attendance Rate", DataService.Instance.GetAttendanceRate(), Theme.AccentOrange, "📊"));
            statsPanel.Controls.Add(new StatCard("Upcoming Sessions", DataService.Instance.GetUpcomingSessions().ToString("D2"), Theme.StatusPurple, "📅"));

            listPanel.Controls.Clear();
            var activities = DataService.Instance.GetRecentActivities();
            foreach (var activity in activities)
            {
                listPanel.Controls.Add(new ActivityItem(activity));
            }

            alertPanel.Controls.Clear();
            var notifications = DataService.Instance.GetNotificationHistory();
            foreach (var note in notifications)
            {
                AddAlertItem(note);
            }
        }

        private void AddAlertItem(string text)
        {
            var pnl = new Panel { Size = new Size(220, 90), Margin = new Padding(0, 0, 0, 15), BackColor = Color.FromArgb(250, 250, 250) };
            
            string type = "INFO";
            Color color = Color.SkyBlue;
            if (text.Contains("[Warning]")) { type = "WARNING"; color = Color.OrangeRed; }
            else if (text.Contains("[Success]")) { type = "SUCCESS"; color = Color.Green; }

            var lblTag = new Label { Text = type, Font = new Font("Segoe UI", 7, FontStyle.Bold), ForeColor = color, Location = new Point(10, 10), AutoSize = true };
            var lblText = new Label { 
                Text = text.Replace("[Info] ", "").Replace("[Warning] ", "").Replace("[Success] ", ""), 
                Font = new Font("Segoe UI", 9), 
                Location = new Point(10, 30), 
                Size = new Size(200, 50),
                AutoEllipsis = true
            };

            pnl.Controls.AddRange(new Control[] { lblTag, lblText });
            alertPanel.Controls.Add(pnl);
        }
    }

    public class StatCard : CustomCard
    {
        public StatCard(string title, string val, Color accent, string icon)
        {
            this.Size = new Size(185, 130); // Increased height
            this.Margin = new Padding(0, 0, 15, 0);

            var pnlAccent = new Panel { BackColor = accent, Width = 4, Height = 40, Location = new Point(0, 45) };
            var lblIcon = new Label { Text = icon, Font = new Font("Segoe UI", 18), Location = new Point(15, 15), AutoSize = true };
            var lblTitle = new Label { Text = title, Font = Theme.SmallFont, ForeColor = Color.Gray, Location = new Point(15, 55), AutoSize = true };
            var lblVal = new Label { Text = val, Font = new Font("Segoe UI", 24, FontStyle.Bold), Location = new Point(15, 75), AutoSize = true };

            this.Controls.AddRange(new Control[] { pnlAccent, lblIcon, lblTitle, lblVal });
        }
    }

    public class ActivityItem : Panel
    {
        public ActivityItem(string activity)
        {
            this.Size = new Size(650, 65);
            this.Margin = new Padding(0, 0, 0, 12);
            this.BackColor = Color.White;

            var icon = new Panel { Size = new Size(40, 40), Location = new Point(15, 12), BackColor = Color.FromArgb(240, 245, 255) };
            var lblIcon = new Label { Text = "⚡", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter };
            icon.Controls.Add(lblIcon);

            var lblText = new Label { Text = activity.Contains("-") ? activity.Split('-')[1].Trim() : activity, Font = new Font("Segoe UI", 10, FontStyle.Bold), Location = new Point(70, 12), AutoSize = true };
            var lblTime = new Label { Text = activity.Contains("-") ? "at " + activity.Split('-')[0].Trim() : "", Font = Theme.SmallFont, ForeColor = Color.Gray, Location = new Point(70, 36), AutoSize = true };

            this.Controls.AddRange(new Control[] { icon, lblText, lblTime });
        }
    }
}
