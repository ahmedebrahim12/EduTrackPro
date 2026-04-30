using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using EduTrackPro.Components;
using EduTrackPro.Data;
using EduTrackPro.Models;
using System.Linq;

namespace EduTrackPro
{
    public class SessionsView : BaseView
    {
        private DataGridView grid;
        private ComboBox comboCourse;
        private DateTimePicker dtp;
        private TextBox txtStartTime;
        private TextBox txtEndTime;
        private TextBox txtSearch;

        public SessionsView() : base("Manage Sessions", "Schedule and manage your upcoming course lectures and lab sessions.")
        {
            SetupLayout();
            LoadCourses();
            LoadSessions(); // Initial load
        }

        private void SetupLayout()
        {
            var card = new CustomCard { Location = new Point(0, 110), Size = new Size(1000, 500), Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom };
            
            // Add Session Section
            var pnlAdd = new Panel { Location = new Point(20, 20), Size = new Size(960, 80) };
            int x = 0;
            comboCourse = CreateInputGroup(pnlAdd, "Course", "Select Course", ref x, 0, 200);
            dtp = CreateDatePickerGroup(pnlAdd, "Date", ref x, 0, 150);
            txtStartTime = CreateTextInputGroup(pnlAdd, "Start Time", "09:00 AM", ref x, 0, 100);
            txtEndTime = CreateTextInputGroup(pnlAdd, "End Time", "10:30 AM", ref x, 0, 100);

            var btnAdd = new CustomButton { Text = "Add Session", Location = new Point(x, 20), Size = new Size(150, 40), BackColor = Theme.PrimaryBlue, ForeColor = Color.White };
            btnAdd.Click += (s, e) => AddSession();
            pnlAdd.Controls.Add(btnAdd);
            card.Controls.Add(pnlAdd);

            var lblHistory = new Label { Text = "View Sessions History", Font = Theme.SubHeaderFont, Location = new Point(20, 120), AutoSize = true };
            card.Controls.Add(lblHistory);

            txtSearch = new TextBox { 
                Text = "Search sessions...", 
                ForeColor = Color.Gray, 
                Location = new Point(500, 120), 
                Size = new Size(250, 30), 
                Font = Theme.BodyFont,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            txtSearch.Enter += (s, e) => { if (txtSearch.Text == "Search sessions...") { txtSearch.Text = ""; txtSearch.ForeColor = Color.Black; } };
            txtSearch.Leave += (s, e) => { if (string.IsNullOrWhiteSpace(txtSearch.Text)) { txtSearch.Text = "Search sessions..."; txtSearch.ForeColor = Color.Gray; } };
            txtSearch.TextChanged += (s, e) => FilterGrid();

            grid = new DataGridView {
                Location = new Point(20, 160),
                Size = new Size(960, 300),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                ReadOnly = true,
                RowTemplate = { Height = 60 },
                EnableHeadersVisualStyles = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                GridColor = Color.FromArgb(240, 240, 240)
            };

            grid.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle {
                BackColor = Color.FromArgb(248, 249, 250),
                ForeColor = Color.FromArgb(50, 50, 50),
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };

            grid.Columns.Add("Course", "COURSE NAME");
            grid.Columns.Add("Date", "DATE");
            grid.Columns.Add("Slot", "TIME SLOT");
            grid.Columns.Add("Room", "ROOM/LINK");
            grid.Columns.Add("Status", "STATUS");
            grid.Columns.Add(new DataGridViewImageColumn { Name = "Actions", HeaderText = "ACTIONS" });

            grid.Columns[0].Width = 250;
            grid.Columns[1].Width = 150;
            grid.Columns[2].Width = 180;
            grid.Columns[3].Width = 200;
            grid.Columns[4].Width = 100;
            grid.Columns[5].Width = 80;

            grid.CellPainting += Grid_CellPainting;
            
            comboCourse.SelectedIndexChanged += (s, e) => LoadSessions();

            card.Controls.AddRange(new Control[] { lblHistory, txtSearch, grid });
            this.Controls.Add(card);
        }

        private void LoadSessions()
        {
            if (comboCourse.SelectedItem is Course course)
            {
                grid.Rows.Clear();
                var sessions = DataService.Instance.GetSessionsByProcedureConnected(course.CourseID);
                foreach (var s in sessions)
                {
                    bool taken = DataService.Instance.IsAttendanceTaken(s.SessionID);
                    string status = (s.SessionDate < DateTime.Today || taken) ? "COMPLETED" : "SCHEDULED";
                    grid.Rows.Add(course.CourseName, s.SessionDate.ToString("MMM dd, yyyy"), $"{s.StartTime} - {s.EndTime}", "Room 302", status);
                }
            }
        }

        private void Grid_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (e.ColumnIndex == 4 && e.Value != null)
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.ContentForeground);
                string status = e.Value.ToString();
                Color bgColor = status == "COMPLETED" ? Color.FromArgb(230, 230, 230) : Color.FromArgb(220, 240, 255);
                Color textColor = status == "COMPLETED" ? Color.Gray : Theme.PrimaryBlue;
                var rect = new Rectangle(e.CellBounds.X + 5, e.CellBounds.Y + 15, 90, 30);
                using (var brush = new SolidBrush(bgColor)) e.Graphics.FillPath(brush, Theme.GetRoundedPath(rect, 15));
                TextRenderer.DrawText(e.Graphics, status, new Font("Segoe UI", 7, FontStyle.Bold), rect, textColor, TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter);
                e.Handled = true;
            }
        }

        private ComboBox CreateInputGroup(Control parent, string label, string def, ref int x, int y, int width)
        {
            parent.Controls.Add(new Label { Text = label, Font = Theme.SmallFont, ForeColor = Color.Gray, Location = new Point(x, y), AutoSize = true });
            var combo = new ComboBox { Location = new Point(x, y + 20), Width = width, Font = Theme.BodyFont, DropDownStyle = ComboBoxStyle.DropDownList };
            parent.Controls.Add(combo);
            x += width + 20;
            return combo;
        }

        private DateTimePicker CreateDatePickerGroup(Control parent, string label, ref int x, int y, int width)
        {
            parent.Controls.Add(new Label { Text = label, Font = Theme.SmallFont, ForeColor = Color.Gray, Location = new Point(x, y), AutoSize = true });
            var d = new DateTimePicker { Location = new Point(x, y + 20), Width = width, Font = Theme.BodyFont, Format = DateTimePickerFormat.Short };
            parent.Controls.Add(d);
            x += width + 20;
            return d;
        }

        private TextBox CreateTextInputGroup(Control parent, string label, string placeholder, ref int x, int y, int width)
        {
            parent.Controls.Add(new Label { Text = label, Font = Theme.SmallFont, ForeColor = Color.Gray, Location = new Point(x, y), AutoSize = true });
            var txt = new TextBox { Text = placeholder, Location = new Point(x, y + 20), Width = width, Font = Theme.BodyFont };
            parent.Controls.Add(txt);
            x += width + 20;
            return txt;
        }

        private void LoadCourses()
        {
            var courses = DataService.Instance.GetCoursesConnected();
            comboCourse.DataSource = courses;
            comboCourse.DisplayMember = "CourseName";
        }

        private void AddSession()
        {
            if (comboCourse.SelectedItem is Course course)
            {
                var session = new Session {
                    CourseID = course.CourseID,
                    SessionDate = dtp.Value,
                    StartTime = txtStartTime.Text,
                    EndTime = txtEndTime.Text
                };
                DataService.Instance.SaveSessionConnected(session);
                DataService.Instance.AddActivity($"Scheduled new session for {course.CourseName}");
                LoadSessions();
                MessageBox.Show("Session added successfully!");
            }
        }

        private void FilterGrid()
        {
            string query = txtSearch.Text.ToLower();
            if (query == "search sessions...") return;
            foreach (DataGridViewRow row in grid.Rows)
                row.Visible = row.Cells[0].Value.ToString().ToLower().Contains(query);
        }
    }
}
