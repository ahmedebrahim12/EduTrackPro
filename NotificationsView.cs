using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using EduTrackPro.Components;
using EduTrackPro.Data;
using EduTrackPro.Models;

namespace EduTrackPro
{
    public class NotificationsView : BaseView
    {
        private ComboBox comboTarget;
        private ComboBox comboType;
        private TextBox txtTitle;
        private TextBox txtBody;
        private ListBox listHistory;

        public NotificationsView() : base("Notification Center", "Manage announcements and broadcast messages to your enrolled students.")
        {
            SetupLayout();
        }

        private void SetupLayout()
        {
            var container = new Panel {
                Location = new Point(0, 120),
                Size = new Size(1000, 600),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom
            };
            this.Controls.Add(container);

            var mainLayout = new TableLayoutPanel { 
                Dock = DockStyle.Fill, 
                ColumnCount = 2,
                RowCount = 1
            };
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45));
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55));
            container.Controls.Add(mainLayout);

            // Left Side: Compose
            var composeCard = new CustomCard { Dock = DockStyle.Fill, Margin = new Padding(0, 0, 10, 0) };
            
            var lblGroup = new Label { Text = "SELECT RECIPIENT GROUP (COURSE)", Font = Theme.SmallFont, ForeColor = Color.Gray, Location = new Point(20, 20), AutoSize = true };
            comboTarget = new ComboBox { Location = new Point(20, 45), Width = 350, Font = Theme.BodyFont, DropDownStyle = ComboBoxStyle.DropDownList };
            
            var lblType = new Label { Text = "NOTIFICATION TYPE", Font = Theme.SmallFont, ForeColor = Color.Gray, Location = new Point(20, 95), AutoSize = true };
            comboType = new ComboBox { Location = new Point(20, 120), Width = 350, Font = Theme.BodyFont, DropDownStyle = ComboBoxStyle.DropDownList };
            comboType.Items.AddRange(new string[] { "Info", "Warning", "Success" });
            comboType.SelectedIndex = 0;

            var lblTitle = new Label { Text = "SUBJECT / TITLE", Font = Theme.SmallFont, ForeColor = Color.Gray, Location = new Point(20, 170), AutoSize = true };
            txtTitle = new TextBox { Location = new Point(20, 195), Width = 350, Font = Theme.BodyFont };
            
            var lblBody = new Label { Text = "MESSAGE BODY", Font = Theme.SmallFont, ForeColor = Color.Gray, Location = new Point(20, 245), AutoSize = true };
            txtBody = new TextBox { Location = new Point(20, 270), Width = 350, Height = 100, Multiline = true, Font = Theme.BodyFont };

            var btnPublish = new CustomButton { 
                Text = "Publish Notification", 
                Location = new Point(20, 390), 
                Size = new Size(350, 45), 
                BackColor = Theme.PrimaryBlue, 
                ForeColor = Color.White 
            };
            btnPublish.Click += (s, e) => SendNotification();

            composeCard.Controls.AddRange(new Control[] { lblGroup, comboTarget, lblType, comboType, lblTitle, txtTitle, lblBody, txtBody, btnPublish });
            mainLayout.Controls.Add(composeCard, 0, 0);

            // Right Side: History
            var historyCard = new CustomCard { Dock = DockStyle.Fill, Margin = new Padding(10, 0, 0, 0) };
            var lblHistoryLabel = new Label { Text = "Sent Notifications History", Font = Theme.SubHeaderFont, Location = new Point(20, 20), AutoSize = true };
            listHistory = new ListBox { 
                Location = new Point(20, 60), 
                Size = new Size(450, 350), 
                BorderStyle = BorderStyle.None, 
                Font = Theme.BodyFont,
                ItemHeight = 40
            };
            listHistory.Items.AddRange(new object[] {
                "Midterm Exam Room Change - CS101",
                "Reading Assignment Ch. 4-5 - PHY301",
                "Grades for Quiz 2 Posted - MATH202"
            });

            historyCard.Controls.AddRange(new Control[] { lblHistoryLabel, listHistory });
            mainLayout.Controls.Add(historyCard, 1, 0);

            LoadHistory();
            var courses = DataService.Instance.GetCoursesConnected();
            comboTarget.DataSource = courses;
            comboTarget.DisplayMember = "CourseName";
        }

        private void LoadHistory()
        {
            listHistory.Items.Clear();
            var items = DataService.Instance.GetNotificationHistory();
            foreach (var item in items) listHistory.Items.Add(item);
        }

        private void SendNotification()
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text)) return;

            int courseId = (comboTarget.SelectedItem as Course)?.CourseID ?? 0;
            string type = comboType.SelectedItem?.ToString() ?? "Info";
            
            // Add student selection if needed, for now we can default or add a combo
            DataService.Instance.PublishNotificationConnected(null, txtTitle.Text, txtBody.Text, type);
            
            MessageBox.Show("Notification sent to all students in the course!", "Sent", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadHistory();
            
            txtTitle.Clear();
            txtBody.Clear();
        }
    }
}
