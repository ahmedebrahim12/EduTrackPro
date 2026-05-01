using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using EduTrackPro.Data;
using Oracle.ManagedDataAccess.Client;

namespace EduTrackPro.StudentPortal.Controls
{
    public class ProfileUserControl : UserControl
    {
        private int _studentId;

        public ProfileUserControl(int studentId = 0)
        {
            _studentId = studentId;
            this.BackColor = Color.FromArgb(245, 247, 250);
            this.Dock      = DockStyle.Fill;
            if (!DesignMode) this.Load += (s, e) => BuildProfile();
        }

        public void BuildProfile()
        {
            this.Controls.Clear();
            int id = _studentId > 0 ? _studentId : Main_student.currentStudentId;

            // Fetch student data from database

            string name   = "Unknown Student";
            string email  = "N/A";
            int    total  = 0;
            int    present= 0;
            int    courses= 0;

            try
            {
                using var conn = new OracleConnection(DataService.ordb);
                conn.Open();

                using (var cmd = new OracleCommand(
                    "SELECT NAME, EMAIL FROM Student WHERE StudentID = :id", conn))
                {
                    cmd.Parameters.Add("id", id);
                    using var r = cmd.ExecuteReader();
                    if (r.Read()) { name = r.GetString(0); email = r.GetString(1); }
                }
                using (var cmd = new OracleCommand(
                    "SELECT COUNT(*) FROM Attendance WHERE StudentID = :id", conn))
                {
                    cmd.Parameters.Add("id", id);
                    total = Convert.ToInt32(cmd.ExecuteScalar());
                }
                using (var cmd = new OracleCommand(
                    "SELECT COUNT(*) FROM Attendance WHERE StudentID = :id AND Status='Present'", conn))
                {
                    cmd.Parameters.Add("id", id);
                    present = Convert.ToInt32(cmd.ExecuteScalar());
                }
                using (var cmd = new OracleCommand(
                    "SELECT COUNT(*) FROM Enrollment WHERE StudentID = :id", conn))
                {
                    cmd.Parameters.Add("id", id);
                    courses = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                // No mock fallback anymore.
                name = "Error loading profile: " + ex.Message;
            }

            int pct = total > 0 ? (int)Math.Round((double)present / total * 100) : 0;
            string secondInitial = name.Split(' ').Length > 1 ? name.Split(' ')[1][0].ToString() : "";
            string initials = name.Length >= 2
                ? $"{name[0]}{secondInitial}"
                : name.Substring(0, 1).ToUpper();

            // Setup UI Components

            // Scrollable content
            var scroll = new Panel
            {
                Dock      = DockStyle.Fill,
                AutoScroll = true,
                BackColor  = Color.FromArgb(245, 247, 250)
            };

            // Profile card
            var card = new Panel
            {
                Size      = new Size(720, 520),
                Location  = new Point(40, 30),
                BackColor = Color.White
            };
            card.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using var path = RoundedRect(new Rectangle(0, 0, card.Width - 1, card.Height - 1), 14);
                e.Graphics.FillPath(new SolidBrush(Color.White), path);
                e.Graphics.DrawPath(new Pen(Color.FromArgb(226, 232, 240), 1), path);
            };

            // Avatar circle
            var avatar = new Panel
            {
                Size     = new Size(90, 90),
                Location = new Point(40, 40),
                BackColor = Color.Transparent
            };
            avatar.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using var br = new LinearGradientBrush(avatar.ClientRectangle,
                    Color.FromArgb(37, 99, 235), Color.FromArgb(99, 48, 200),
                    LinearGradientMode.ForwardDiagonal);
                e.Graphics.FillEllipse(br, 0, 0, 89, 89);
                TextRenderer.DrawText(e.Graphics, initials.ToUpper(),
                    new Font("Segoe UI", 28, FontStyle.Bold),
                    new Rectangle(0, 0, 90, 90), Color.White,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            };
            card.Controls.Add(avatar);

            // Name & email
            card.Controls.Add(MakeLbl(name, new Font("Segoe UI", 18, FontStyle.Bold),
                Color.FromArgb(30, 41, 59), new Point(148, 44)));
            card.Controls.Add(MakeLbl($"✉  {email}", new Font("Segoe UI", 10),
                Color.FromArgb(100, 116, 139), new Point(150, 82)));
            card.Controls.Add(MakeLbl($"🎓  Student ID: {id}", new Font("Segoe UI", 10),
                Color.FromArgb(100, 116, 139), new Point(150, 106)));

            // Divider
            var div = new Panel { Size = new Size(640, 1), Location = new Point(40, 155), BackColor = Color.FromArgb(226, 232, 240) };
            card.Controls.Add(div);

            // Stats row
            AddStatBox(card, "Overall Attendance", $"{pct}%",
                pct >= 75 ? Color.FromArgb(5, 150, 105) : Color.FromArgb(220, 38, 38), 40, 175);
            AddStatBox(card, "Sessions Attended", $"{present}/{total}",
                Color.FromArgb(37, 99, 235), 220, 175);
            AddStatBox(card, "Enrolled Courses", $"{courses}",
                Color.FromArgb(99, 48, 200), 400, 175);

            // Progress bar section
            card.Controls.Add(MakeLbl("Attendance Progress", new Font("Segoe UI", 11, FontStyle.Bold),
                Color.FromArgb(30, 41, 59), new Point(40, 320)));

            var progBg = new Panel
            {
                Size      = new Size(640, 12),
                Location  = new Point(40, 350),
                BackColor = Color.FromArgb(226, 232, 240)
            };
            int fillW = (int)(640 * pct / 100.0);
            var progFill = new Panel
            {
                Size      = new Size(fillW, 12),
                Location  = new Point(0, 0),
                BackColor = pct >= 75 ? Color.FromArgb(5, 150, 105) : Color.FromArgb(220, 38, 38)
            };
            progBg.Controls.Add(progFill);
            card.Controls.Add(progBg);

            string standingTxt = pct >= 75 ? "Good Standing ✓" : "Attendance Below 75% ⚠";
            Color  standingClr = pct >= 75 ? Color.FromArgb(5, 150, 105) : Color.FromArgb(220, 38, 38);
            card.Controls.Add(MakeLbl($"{pct}% — {standingTxt}",
                new Font("Segoe UI", 10), standingClr,
                new Point(40, 372)));

            // Info section
            card.Controls.Add(MakeLbl("Account Information", new Font("Segoe UI", 11, FontStyle.Bold),
                Color.FromArgb(30, 41, 59), new Point(40, 420)));
            card.Controls.Add(MakeLbl($"Full Name:   {name}", new Font("Segoe UI", 10),
                Color.FromArgb(71, 85, 105), new Point(40, 450)));
            card.Controls.Add(MakeLbl($"Email:          {email}", new Font("Segoe UI", 10),
                Color.FromArgb(71, 85, 105), new Point(40, 474)));

            scroll.Controls.Add(card);
            this.Controls.Add(scroll);
        }

        private void AddStatBox(Control parent, string label, string value, Color accent, int x, int y)
        {
            var box = new Panel { Size = new Size(160, 120), Location = new Point(x, y), BackColor = Color.FromArgb(248, 250, 252) };
            box.Controls.Add(new Label
            {
                Text = value, Font = new Font("Segoe UI", 22, FontStyle.Bold),
                ForeColor = accent, Location = new Point(10, 20), AutoSize = true
            });
            box.Controls.Add(new Label
            {
                Text = label, Font = new Font("Segoe UI", 8),
                ForeColor = Color.FromArgb(100, 116, 139), Location = new Point(10, 70), AutoSize = true
            });
            var accentStrip = new Panel { Size = new Size(4, 120), Location = new Point(0, 0), BackColor = accent };
            box.Controls.Add(accentStrip);
            parent.Controls.Add(box);
        }

        private Label MakeLbl(string text, Font font, Color fore, Point loc) =>
            new Label { Text = text, Font = font, ForeColor = fore, BackColor = Color.Transparent, Location = loc, AutoSize = true };

        private GraphicsPath RoundedRect(Rectangle bounds, int radius)
        {
            int d = radius * 2;
            var path = new GraphicsPath();
            path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
            path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
            path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}
