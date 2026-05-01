using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace EduTrackPro
{
    partial class RegisterForm
    {
        private System.ComponentModel.IContainer components = null;

        // Left panel
        private Panel panelLeft;
        private Label lblBrand;
        private Label lblBrandSub;
        private Label lblF1, lblF2, lblF3;

        // Right panel / card
        private Panel panelRight;
        private Panel panelCard;
        private Label lblTitle;
        private Label lblSubtitle;

        // Role selector
        private Label lblRoleHint;
        private Panel panelRoleRow;
        private Button btnRoleStudent;
        private Button btnRoleInstructor;
        private string selectedRole = "Student";

        // Name
        private Label lblName;
        private Panel panelName;
        internal System.Windows.Forms.TextBox textBoxName;

        // Email
        private Label lblEmailLbl;
        private Panel panelEmailBox;
        internal System.Windows.Forms.TextBox textBoxEmail;

        // Password
        private Label lblPassLbl;
        private Panel panelPassBox;
        internal System.Windows.Forms.TextBox textBoxPass;
        private Label lblShowPass;

        // Confirm Password
        private Label lblConPassLbl;
        private Panel panelConPassBox;
        internal System.Windows.Forms.TextBox textBoxConPass;
        private Label lblShowConPass;

        // Buttons
        internal Button SignUpButton;
        internal Button btnBackToLogin;
        internal new Button CancelButton;
        private Button btnClose;

        // Already have account
        private Label lblHave;
        private Label lblSignIn;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Main Form Settings

            this.ClientSize      = new Size(960, 600);
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition   = FormStartPosition.CenterScreen;
            this.Name            = "RegisterForm";
            this.Text            = "EduTrack Pro – Sign Up";
            this.BackColor       = Color.FromArgb(15, 23, 42);
            this.DoubleBuffered  = true;
            this.Load           += new EventHandler(this.RegisterForm_Load);

            // Sidebar / Branding

            panelLeft = new Panel { Size = new Size(380, 600), Location = new Point(0, 0), BackColor = Color.Transparent };
            panelLeft.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using var br = new LinearGradientBrush(panelLeft.ClientRectangle,
                    Color.FromArgb(99, 48, 200), Color.FromArgb(37, 99, 235),
                    LinearGradientMode.ForwardDiagonal);
                g.FillRectangle(br, panelLeft.ClientRectangle);
                using var cb = new SolidBrush(Color.FromArgb(25, 255, 255, 255));
                g.FillEllipse(cb, -80, -80, 280, 280);
                g.FillEllipse(cb, 180, 360, 320, 320);
                g.FillEllipse(cb, 80, 200, 100, 100);
            };

            lblBrand    = MakeLbl("EduTrack Pro", new Font("Segoe UI", 26, FontStyle.Bold), Color.White, new Point(36, 70), true);
            lblBrandSub = MakeLbl("Smart Attendance Management", new Font("Segoe UI", 11), Color.FromArgb(200, 255, 255, 255), new Point(36, 115), true);
            lblF1 = MakeFeatureLbl("✓  Register as Student or Instructor", 230);
            lblF2 = MakeFeatureLbl("✓  Track attendance in real-time", 268);
            lblF3 = MakeFeatureLbl("✓  Get instant notifications", 306);
            panelLeft.Controls.AddRange(new Control[] { lblBrand, lblBrandSub, lblF1, lblF2, lblF3 });

            // Registration Form Controls

            panelRight = new Panel { Size = new Size(580, 600), Location = new Point(380, 0), BackColor = Color.FromArgb(15, 23, 42) };

            panelCard = new Panel { Size = new Size(480, 560), Location = new Point(50, 20), BackColor = Color.FromArgb(30, 41, 59) };
            panelCard.Paint += (s, e) =>
            {
                var g = e.Graphics; g.SmoothingMode = SmoothingMode.AntiAlias;
                using var path = RoundedRect(new Rectangle(0, 0, panelCard.Width - 1, panelCard.Height - 1), 18);
                using var bg   = new SolidBrush(Color.FromArgb(30, 41, 59));
                g.FillPath(bg, path);
                using var pen  = new Pen(Color.FromArgb(55, 71, 100), 1.5f);
                g.DrawPath(pen, path);
            };

            lblTitle    = MakeLbl("Create Account 🎓", new Font("Segoe UI", 19, FontStyle.Bold), Color.White, new Point(28, 24), true);
            lblSubtitle = MakeLbl("Join EduTrack Pro today", new Font("Segoe UI", 10), Color.FromArgb(148, 163, 184), new Point(28, 62), true);

            // Role selector
            lblRoleHint = MakeLbl("I am a:", new Font("Segoe UI", 9, FontStyle.Bold), Color.FromArgb(148, 163, 184), new Point(28, 100), true);
            panelRoleRow = new Panel { Size = new Size(424, 40), Location = new Point(28, 120), BackColor = Color.Transparent };

            btnRoleStudent = MakeRoleBtn("Student", new Point(0, 0), true);
            btnRoleInstructor = MakeRoleBtn("Instructor", new Point(218, 0), false);
            btnRoleStudent.Click += (s, e) => SelectRole("Student");
            btnRoleInstructor.Click += (s, e) => SelectRole("Instructor");
            panelRoleRow.Controls.AddRange(new Control[] { btnRoleStudent, btnRoleInstructor });

            // Full Name
            lblName   = MakeLbl("Full Name", new Font("Segoe UI", 9, FontStyle.Bold), Color.FromArgb(148, 163, 184), new Point(28, 172), true);
            panelName = MakeInputPanel(28, 194, 424);
            textBoxName = MakeTextBox("textBoxName");
            textBoxName.TextChanged += new EventHandler(this.textBoxName_TextChanged);
            panelName.Controls.Add(textBoxName);

            // Email
            lblEmailLbl   = MakeLbl("Email Address", new Font("Segoe UI", 9, FontStyle.Bold), Color.FromArgb(148, 163, 184), new Point(28, 248), true);
            panelEmailBox = MakeInputPanel(28, 270, 424);
            textBoxEmail  = MakeTextBox("textBoxEmail");
            textBoxEmail.TextChanged += new EventHandler(this.textBoxEmail_TextChanged);
            panelEmailBox.Controls.Add(textBoxEmail);

            // Password
            lblPassLbl  = MakeLbl("Password", new Font("Segoe UI", 9, FontStyle.Bold), Color.FromArgb(148, 163, 184), new Point(28, 324), true);
            panelPassBox = MakeInputPanel(28, 346, 424);
            textBoxPass  = MakeTextBox("textBoxPass"); textBoxPass.PasswordChar = '*';
            textBoxPass.Size = new Size(360, 22);
            textBoxPass.TextChanged += new EventHandler(this.textBoxPass_TextChanged);
            lblShowPass = MakeEyeBtn(new Point(376, 7));
            lblShowPass.Click += (s, e) => TogglePass(textBoxPass, lblShowPass);
            panelPassBox.Controls.AddRange(new Control[] { textBoxPass, lblShowPass });

            // Confirm Password
            lblConPassLbl  = MakeLbl("Confirm Password", new Font("Segoe UI", 9, FontStyle.Bold), Color.FromArgb(148, 163, 184), new Point(28, 400), true);
            panelConPassBox = MakeInputPanel(28, 422, 424);
            textBoxConPass  = MakeTextBox("textBoxConPass"); textBoxConPass.PasswordChar = '*';
            textBoxConPass.Size = new Size(360, 22);
            textBoxConPass.TextChanged += new EventHandler(this.textBoxConPass_TextChanged);
            lblShowConPass = MakeEyeBtn(new Point(376, 7));
            lblShowConPass.Click += (s, e) => TogglePass(textBoxConPass, lblShowConPass);
            panelConPassBox.Controls.AddRange(new Control[] { textBoxConPass, lblShowConPass });

            // Sign Up button
            SignUpButton = new Button
            {
                Text = "Create Account  →", Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.White, FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 },
                Size = new Size(424, 46), Location = new Point(28, 466),
                Cursor = Cursors.Hand, Name = "SignUpButton", UseVisualStyleBackColor = false
            };
            SignUpButton.Paint += (s, e) =>
            {
                var g = e.Graphics; g.SmoothingMode = SmoothingMode.AntiAlias;
                var rect = new Rectangle(0, 0, SignUpButton.Width - 1, SignUpButton.Height - 1);
                using var path = RoundedRect(rect, 10);
                using var brush = new LinearGradientBrush(rect,
                    Color.FromArgb(99, 48, 200), Color.FromArgb(37, 99, 235),
                    LinearGradientMode.Horizontal);
                g.FillPath(brush, path);
                TextRenderer.DrawText(g, SignUpButton.Text, SignUpButton.Font,
                    rect, Color.White, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            };
            SignUpButton.Click += new EventHandler(this.SignUpButton_Click);

            // Back to Login Button
            btnBackToLogin = new Button
            {
                Text = "← Back to Login",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(148, 163, 184),
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 1, BorderColor = Color.FromArgb(71, 85, 105) },
                Size = new Size(424, 40),
                Location = new Point(28, 510),
                Cursor = Cursors.Hand,
                Name = "btnBackToLogin",
                UseVisualStyleBackColor = false
            };
            btnBackToLogin.Click += new EventHandler(this.CancelButton_Click);

            // Hidden CancelButton (kept for code compatibility)
            CancelButton = new Button { Visible = false, Name = "CancelButton" };
            CancelButton.Click += new EventHandler(this.CancelButton_Click);

            // Close X
            btnClose = new Button
            {
                Text = "✕", Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(148, 163, 184), FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0, MouseOverBackColor = Color.FromArgb(220, 53, 69) },
                BackColor = Color.Transparent, Size = new Size(36, 36),
                Location = new Point(918, 10), Cursor = Cursors.Hand
            };
            btnClose.Click += (s, e) => { LoginForm lf = new LoginForm(); lf.Show(); this.Close(); };

            // Assemble
            panelCard.Controls.AddRange(new Control[]
            {
                lblTitle, lblSubtitle,
                lblRoleHint, panelRoleRow,
                lblName, panelName,
                lblEmailLbl, panelEmailBox,
                lblPassLbl, panelPassBox,
                lblConPassLbl, panelConPassBox,
                SignUpButton, btnBackToLogin,
                CancelButton
            });
            panelRight.Controls.Add(panelCard);
            this.Controls.AddRange(new Control[] { panelLeft, panelRight, btnClose });
            this.ResumeLayout(false);
        }

        // Form UI Helpers

        private void SelectRole(string role)
        {
            selectedRole = role;
            // Student
            btnRoleStudent.BackColor   = role == "Student" ? Color.FromArgb(99, 102, 241) : Color.FromArgb(51, 65, 85);
            btnRoleStudent.ForeColor   = Color.White;
            // Instructor
            btnRoleInstructor.BackColor = role == "Instructor" ? Color.FromArgb(99, 102, 241) : Color.FromArgb(51, 65, 85);
            btnRoleInstructor.ForeColor = Color.White;
            btnRoleStudent.Invalidate(); btnRoleInstructor.Invalidate();
        }

        private void TogglePass(TextBox tb, Label eye)
        {
            tb.PasswordChar = tb.PasswordChar == '*' ? '\0' : '*';
            eye.ForeColor   = tb.PasswordChar == '\0' ? Color.FromArgb(99, 102, 241) : Color.FromArgb(148, 163, 184);
        }

        private Label MakeLbl(string text, Font font, Color fore, Point loc, bool autoSize) =>
            new Label { Text = text, Font = font, ForeColor = fore, BackColor = Color.Transparent, Location = loc, AutoSize = autoSize };

        private Label MakeFeatureLbl(string text, int y) =>
            new Label { Text = text, Font = new Font("Segoe UI", 10), ForeColor = Color.FromArgb(219, 234, 254), BackColor = Color.Transparent, AutoSize = true, Location = new Point(36, y) };

        private Panel MakeInputPanel(int x, int y, int width) =>
            new Panel { Size = new Size(width, 40), Location = new Point(x, y), BackColor = Color.FromArgb(51, 65, 85) };

        private System.Windows.Forms.TextBox MakeTextBox(string name) =>
            new System.Windows.Forms.TextBox
            {
                BorderStyle = BorderStyle.None, BackColor = Color.FromArgb(51, 65, 85),
                ForeColor = Color.White, Font = new Font("Segoe UI", 11),
                Size = new Size(400, 22), Location = new Point(12, 9), Name = name
            };

        private Label MakeEyeBtn(Point loc) =>
            new Label { Text = "👁", Font = new Font("Segoe UI", 12), ForeColor = Color.FromArgb(148, 163, 184),
                BackColor = Color.Transparent, Size = new Size(28, 24), Location = loc, Cursor = Cursors.Hand, TextAlign = ContentAlignment.MiddleCenter };

        private Button MakeRoleBtn(string text, Point loc, bool selected) =>
            new Button
            {
                Text = text, Size = new Size(208, 36), Location = loc, Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 10, FontStyle.Bold), ForeColor = Color.White,
                BackColor = selected ? Color.FromArgb(99, 102, 241) : Color.FromArgb(51, 65, 85),
                FlatStyle = FlatStyle.Flat, FlatAppearance = { BorderSize = 0 }
            };

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