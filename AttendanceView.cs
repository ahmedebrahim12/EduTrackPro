using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Data;
using EduTrackPro.Components;
using EduTrackPro.Data;
using EduTrackPro.Models;

namespace EduTrackPro
{
    public class AttendanceView : BaseView
    {
        private FlowLayoutPanel flowPanel;
        private ComboBox comboCourse;
        private ComboBox comboSession;
        private TextBox txtSearch;
        private DataTable studentTable;
        private Dictionary<int, string> attendanceStatus = new Dictionary<int, string>();

        public AttendanceView() : base("Take Attendance", "Record student attendance for specific course sessions.")
        {
            SetupFilters();
            SetupStudentList();
        }

        private void SetupFilters()
        {
            var card = new CustomCard { Location = new Point(0, 100), Size = new Size(1000, 120), Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right };
            
            int x = 20;
            comboCourse = CreateFilterCombo(card, "COURSE", ref x, 250);
            comboSession = CreateFilterCombo(card, "SESSION", ref x, 200);

            var courses = DataService.Instance.GetCoursesConnected();
            comboCourse.DataSource = courses;
            comboCourse.DisplayMember = "CourseName";

            if (courses.Count > 0)
            {
                LoadSessions(courses[0].CourseID);
                LoadStudents();
            }

            comboCourse.SelectedIndexChanged += (s, e) => {
                if (comboCourse.SelectedItem is Course course)
                {
                    LoadSessions(course.CourseID);
                }
            };
            comboSession.SelectedIndexChanged += (s, e) => LoadStudents();

            var btnMarkAll = new CustomButton { Text = "Mark All Present", Location = new Point(x, 40), Size = new Size(150, 40), BackColor = Color.FromArgb(240, 240, 240), ForeColor = Color.Black };
            btnMarkAll.Click += (s, e) => {
                foreach (Control studentCard in flowPanel.Controls) {
                    if (studentCard is Panel p) {
                        // Find the FlowLayoutPanel containing the buttons
                        foreach (Control c in p.Controls) {
                            if (c is FlowLayoutPanel actions) {
                                // Find the 'P' button and click it
                                foreach (Control btn in actions.Controls) {
                                    if (btn is Button b && b.Text == "P") {
                                        b.PerformClick();
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            };

            x += 160;
            var btnSave = new CustomButton { 
                Text = "Save Attendance", 
                Location = new Point(x, 40), 
                Size = new Size(160, 40), 
                BackColor = Theme.PrimaryBlue, 
                ForeColor = Color.White 
            };
            btnSave.Click += (s, e) => SaveAttendance();

            card.Controls.AddRange(new Control[] { btnMarkAll, btnSave });
            this.Controls.Add(card);
        }

        private void SetupStudentList()
        {
            var container = new CustomCard { Location = new Point(0, 240), Size = new Size(1000, 500), Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom };
            
            txtSearch = new TextBox { 
                Text = "Search students...", 
                Location = new Point(20, 20), 
                Width = 300, 
                Font = Theme.BodyFont, 
                ForeColor = Color.Gray 
            };
            txtSearch.Enter += (s, e) => { if (txtSearch.Text == "Search students...") { txtSearch.Text = ""; txtSearch.ForeColor = Color.Black; } };
            txtSearch.Leave += (s, e) => { if (string.IsNullOrWhiteSpace(txtSearch.Text)) { txtSearch.Text = "Search students..."; txtSearch.ForeColor = Color.Gray; } };
            txtSearch.TextChanged += (s, e) => FilterStudents();

            flowPanel = new FlowLayoutPanel { 
                Location = new Point(20, 65), 
                Size = new Size(960, 420), 
                AutoScroll = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom
            };
            
            container.Controls.AddRange(new Control[] { txtSearch, flowPanel });
            this.Controls.Add(container);
        }


        private void LoadStudents()
        {
            if (comboCourse.SelectedItem is Course course)
            {
                int sessionId = (comboSession.SelectedItem as Session)?.SessionID ?? 0;
                var existing = sessionId > 0 ? DataService.Instance.GetExistingAttendance(sessionId) : new Dictionary<int, string>();

                flowPanel.Controls.Clear();
                studentTable = DataService.Instance.GetStudentsForAttendance(course.CourseID);
                foreach (DataRow row in studentTable.Rows)
                {
                    int sid = Convert.ToInt32(row["StudentID"]);
                    string status = existing.ContainsKey(sid) ? existing[sid] : "Undefined";
                    AddStudentCard(sid, row["Name"].ToString()!, status);
                }
            }
        }

        private void AddStudentCard(int id, string name, string initialStatus = "Present")
        {
            var card = new Panel { Size = new Size(250, 130), Margin = new Padding(0, 0, 15, 15), BackColor = Color.FromArgb(250, 250, 250) };
            card.Tag = name.ToLower();
            
            var lblName = new Label { Text = name, Font = new Font("Segoe UI", 10, FontStyle.Bold), Location = new Point(15, 15), AutoSize = true };
            var lblId = new Label { Text = "ID: " + id, Font = Theme.SmallFont, ForeColor = Color.Gray, Location = new Point(15, 38), AutoSize = true };

            // Status Buttons Container
            var pnlActions = new FlowLayoutPanel { Location = new Point(15, 70), Size = new Size(220, 45), WrapContents = false };
            
            var btnP = CreateStatusBtn("P", Color.FromArgb(40, 167, 69), id, card, "Present");
            var btnA = CreateStatusBtn("A", Color.FromArgb(220, 53, 69), id, card, "Absent");
            var btnL = CreateStatusBtn("L", Color.FromArgb(255, 193, 7), id, card, "Late");

            pnlActions.Controls.AddRange(new Control[] { btnP, btnA, btnL });

            // Set initial state
            attendanceStatus[id] = initialStatus;
            
            // If undefined, don't highlight any button and keep card neutral
            if (initialStatus != "Undefined")
            {
                Button activeBtn = initialStatus == "Present" ? btnP : (initialStatus == "Absent" ? btnA : btnL);
                foreach (Control c in pnlActions.Controls) { c.BackColor = Color.White; c.ForeColor = ((Button)c).FlatAppearance.BorderColor; }
                activeBtn.BackColor = activeBtn.FlatAppearance.BorderColor;
                activeBtn.ForeColor = Color.White;

                if (initialStatus == "Present") card.BackColor = Color.FromArgb(240, 255, 240);
                else if (initialStatus == "Absent") card.BackColor = Color.FromArgb(255, 240, 240);
                else if (initialStatus == "Late") card.BackColor = Color.FromArgb(255, 253, 240);
            }
            else
            {
                // Neutral state for Undefined
                card.BackColor = Color.White;
                foreach (Control c in pnlActions.Controls) { c.BackColor = Color.White; c.ForeColor = ((Button)c).FlatAppearance.BorderColor; }
            }

            card.Controls.AddRange(new Control[] { lblName, lblId, pnlActions });
            flowPanel.Controls.Add(card);
        }

        private Button CreateStatusBtn(string text, Color color, int studentId, Panel card, string status)
        {
            var btn = new Button {
                Text = text,
                Size = new Size(60, 35),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.White,
                ForeColor = color,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderColor = color;
            btn.FlatAppearance.BorderSize = 1;

            btn.Click += (s, e) => {
                attendanceStatus[studentId] = status;
                
                // Reset all buttons in this parent
                foreach (Control c in btn.Parent.Controls) {
                    c.BackColor = Color.White;
                    c.ForeColor = ((Button)c).FlatAppearance.BorderColor;
                }

                // Active state
                btn.BackColor = color;
                btn.ForeColor = Color.White;

                // Subtle card feedback
                if (status == "Present") card.BackColor = Color.FromArgb(240, 255, 240);
                else if (status == "Absent") card.BackColor = Color.FromArgb(255, 240, 240);
                else card.BackColor = Color.FromArgb(255, 253, 240);
            };

            // Remove automatic activation of 'P'
            // if (text == "P") { btn.BackColor = color; btn.ForeColor = Color.White; }

            return btn;
        }
    
        private void FilterStudents()
        {
            string query = txtSearch.Text.ToLower();
            if (query == "search students...") query = "";

            foreach (Control ctrl in flowPanel.Controls)
            {
                if (ctrl is Panel p)
                {
                    p.Visible = string.IsNullOrEmpty(query) || p.Tag.ToString().Contains(query);
                }
            }
        }

        private void SaveAttendance()
        {
            if (studentTable == null) return;
            if (!studentTable.Columns.Contains("Status")) studentTable.Columns.Add("Status");

            foreach (DataRow row in studentTable.Rows)
            {
                int sid = Convert.ToInt32(row["StudentID"]);
                row["Status"] = attendanceStatus.ContainsKey(sid) ? attendanceStatus[sid] : "Undefined";
            }

            int sessionId = (comboSession.SelectedItem as Session)?.SessionID ?? 0;
            int courseId = (comboCourse.SelectedItem as Course)?.CourseID ?? 0;
            
            if (sessionId == 0) {
                MessageBox.Show("Please select a session first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataService.Instance.SaveAttendanceDisconnected(sessionId, courseId, studentTable);
            DataService.Instance.AddActivity($"Recorded attendance for session ID {sessionId}");
            MessageBox.Show("Attendance saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void LoadSessions(int courseId)
        {
            var sessions = DataService.Instance.GetSessionsByProcedureConnected(courseId);
            comboSession.DataSource = null;
            comboSession.DataSource = sessions;
            comboSession.DisplayMember = "FormattedSession";
        }

        private ComboBox CreateFilterCombo(Control parent, string label, ref int x, int width)
        {
            parent.Controls.Add(new Label { Text = label, Font = Theme.SmallFont, ForeColor = Color.Gray, Location = new Point(x, 20), AutoSize = true });
            var combo = new ComboBox { Location = new Point(x, 42), Width = width, Font = Theme.BodyFont, DropDownStyle = ComboBoxStyle.DropDownList };
            parent.Controls.Add(combo);
            x += width + 20;
            return combo;
        }
    }
}
