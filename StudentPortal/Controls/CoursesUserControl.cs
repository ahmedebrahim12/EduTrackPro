using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using EduTrackPro.Data;
using EduTrackPro.Models;
using Oracle.ManagedDataAccess.Client;

namespace EduTrackPro.StudentPortal.Controls
{
    public class CoursesUserControl : UserControl
    {
        private int _studentId;
        private Panel header;
        private Label lblTitle;
        private Label lblSubtitle;
        private FlowLayoutPanel flowCourses;

        public CoursesUserControl(int studentId = 0)
        {
            _studentId = studentId;
            InitLayout();
            if (!DesignMode) this.Load += (s, e) => LoadCourses();
        }

        private new void InitLayout()
        {
            this.BackColor = Color.FromArgb(245, 247, 250);
            this.Dock      = DockStyle.Fill;

            // Header
            header = new Panel
            {
                Dock      = DockStyle.Top,
                Height    = 90,
                BackColor = Color.White,
                Padding   = new Padding(30, 20, 20, 10)
            };
            lblTitle = new Label
            {
                Text      = "📚  My Courses",
                Font      = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                Dock      = DockStyle.Top,
                AutoSize  = false,
                Height    = 40
            };
            lblSubtitle = new Label
            {
                Text      = "All courses you are enrolled in",
                Font      = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(100, 116, 139),
                Dock      = DockStyle.Top,
                AutoSize  = false,
                Height    = 24
            };
            header.Controls.Add(lblSubtitle);
            header.Controls.Add(lblTitle);

            // Course cards flow panel
            flowCourses = new FlowLayoutPanel
            {
                Dock          = DockStyle.Fill,
                AutoScroll    = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents  = false,
                Padding       = new Padding(30, 20, 20, 20),
                BackColor     = Color.FromArgb(245, 247, 250)
            };

            this.Controls.Add(flowCourses);
            this.Controls.Add(header);
        }

        public void LoadCourses()
        {
            flowCourses.Controls.Clear();
            int id = _studentId > 0 ? _studentId : Main_student.currentStudentId;

            // ── Oracle query (from marawn) ───────────────────────────────────
            List<(int courseId, string name, int present, int total)> courses
                = new List<(int, string, int, int)>();
            try
            {
                using var conn = new OracleConnection(DataService.ordb);
                conn.Open();

                string sql = @"
                    SELECT c.CourseID, c.CourseName,
                           SUM(CASE WHEN a.Status = 'Present' THEN 1 ELSE 0 END) AS PresentCount,
                           COUNT(a.AttendanceID) AS TotalSessions
                    FROM Course c
                    JOIN Enrollment e     ON c.CourseID  = e.CourseID
                    LEFT JOIN courseSession s ON c.CourseID  = s.CourseID
                    LEFT JOIN Attendance a   ON s.SessionID = a.SessionID AND a.StudentID = :sid
                    WHERE e.StudentID = :sid2
                    GROUP BY c.CourseID, c.CourseName
                    ORDER BY c.CourseName";

                using var cmd = new OracleCommand(sql, conn);
                cmd.Parameters.Add("sid",  id);
                cmd.Parameters.Add("sid2", id);

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    courses.Add((
                        reader.GetInt32(0),
                        reader.GetString(1),
                        reader.IsDBNull(2) ? 0 : Convert.ToInt32(reader.GetDecimal(2)),
                        reader.IsDBNull(3) ? 0 : Convert.ToInt32(reader.GetDecimal(3))
                    ));
                }
            }
            catch (Exception ex)
            {
                // No mock fallback anymore.
                Console.WriteLine("Error loading courses: " + ex.Message);
            }

            if (courses.Count == 0)
            {
                flowCourses.Controls.Add(new Label
                {
                    Text      = "No enrolled courses found.",
                    Font      = new Font("Segoe UI", 12),
                    ForeColor = Color.Gray,
                    AutoSize  = true,
                    Margin    = new Padding(10, 20, 0, 0)
                });
                return;
            }

            Color[] palette = {
                Color.FromArgb(37, 99, 235),
                Color.FromArgb(99, 48, 200),
                Color.FromArgb(5, 150, 105),
                Color.FromArgb(217, 119, 6),
                Color.FromArgb(220, 38, 38)
            };

            int colorIdx = 0;
            foreach (var (courseId, name, present, total) in courses)
            {
                int pct = total > 0 ? (int)Math.Round((double)present / total * 100) : 0;
                Color accent = palette[colorIdx % palette.Length];
                colorIdx++;

                var card = BuildCourseCard(name, present, total, pct, accent);
                flowCourses.Controls.Add(card);
            }
        }

        private Panel BuildCourseCard(string name, int present, int total, int pct, Color accent)
        {
            int cardW = Math.Max(600, flowCourses.ClientSize.Width - 60);

            var card = new Panel
            {
                Size        = new Size(cardW, 120),
                BackColor   = Color.White,
                Margin      = new Padding(0, 0, 0, 14),
                Cursor      = Cursors.Default
            };

            // Left accent strip
            var strip = new Panel
            {
                Size      = new Size(6, 120),
                Location  = new Point(0, 0),
                BackColor = accent
            };
            card.Controls.Add(strip);

            // Course name
            var lblName = new Label
            {
                Text      = name,
                Font      = new Font("Segoe UI", 13, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                Location  = new Point(24, 16),
                Size      = new Size(cardW - 200, 26),
                AutoEllipsis = true
            };
            card.Controls.Add(lblName);

            // Attendance text
            string attendTxt = total > 0
                ? $"{present} / {total} sessions attended"
                : "No sessions recorded yet";
            var lblAttend = new Label
            {
                Text      = attendTxt,
                Font      = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(100, 116, 139),
                Location  = new Point(24, 44),
                AutoSize  = true
            };
            card.Controls.Add(lblAttend);

            // Progress bar background
            var progressBg = new Panel
            {
                Size      = new Size(cardW - 160, 8),
                Location  = new Point(24, 76),
                BackColor = Color.FromArgb(226, 232, 240)
            };
            card.Controls.Add(progressBg);

            // Progress bar fill
            int fillW = total > 0 ? Math.Max(0, (int)((cardW - 160) * pct / 100.0)) : 0;
            var progressFill = new Panel
            {
                Size      = new Size(fillW, 8),
                Location  = new Point(0, 0),
                BackColor = pct >= 75 ? Color.FromArgb(5, 150, 105) : Color.FromArgb(220, 38, 38)
            };
            progressBg.Controls.Add(progressFill);

            // Percentage badge
            var lblPct = new Label
            {
                Text      = $"{pct}%",
                Font      = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = accent,
                Location  = new Point(cardW - 130, 20),
                Size      = new Size(80, 30),
                TextAlign = ContentAlignment.MiddleRight
            };
            card.Controls.Add(lblPct);

            // Status label
            string statusTxt = total == 0 ? "No data" : pct >= 75 ? "✓ Good Standing" : "⚠ Low Attendance";
            Color  statusClr = total == 0 ? Color.Gray : pct >= 75 ? Color.FromArgb(5, 150, 105) : Color.FromArgb(220, 38, 38);
            var lblStatus = new Label
            {
                Text      = statusTxt,
                Font      = new Font("Segoe UI", 8, FontStyle.Bold),
                ForeColor = statusClr,
                Location  = new Point(cardW - 130, 52),
                Size      = new Size(110, 18),
                TextAlign = ContentAlignment.MiddleRight
            };
            card.Controls.Add(lblStatus);

            return card;
        }
    }
}
