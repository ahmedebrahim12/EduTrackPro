using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace EduTrackPro
{
    partial class LoginForm
    {
        private System.ComponentModel.IContainer components = null;

        // Left panel (branding)
        private Panel panelLeft;
        private Label lblBrand;
        private Label lblBrandSub;
        private Label lblFeature1;
        private Label lblFeature2;
        private Label lblFeature3;

        // Right panel (login card)
        private Panel panelRight;
        private Panel panelCard;
        private Label lblWelcome;
        private Label lblSubtitle;
        private Label label1;       // Email label
        private Panel panelEmail;
        private System.Windows.Forms.TextBox textBox1;
        private Label label2;       // Password label
        private Panel panelPass;
        private System.Windows.Forms.TextBox textBoxPass;
        private Label lblShowPass;
        private Button button1;     // Login
        private Button button2;     // Exit
        private Label label3;       // "Don't have an account?"
        private Label label4;       // Sign Up link

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Form configuration

            this.ClientSize        = new Size(900, 540);
            this.FormBorderStyle   = FormBorderStyle.None;
            this.StartPosition     = FormStartPosition.CenterScreen;
            this.Name              = "LoginForm";
            this.Text              = "EduTrack Pro – Sign In";
            this.BackColor         = Color.FromArgb(15, 23, 42);
            this.DoubleBuffered    = true;
            this.Load             += new EventHandler(this.LoginForm_Load);

            // Left side design

            panelLeft = new Panel
            {
                Size      = new Size(420, 540),
                Location  = new Point(0, 0),
                BackColor = Color.Transparent
            };
            panelLeft.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using var brush = new LinearGradientBrush(
                    panelLeft.ClientRectangle,
                    Color.FromArgb(37, 99, 235),
                    Color.FromArgb(99, 48, 200),
                    LinearGradientMode.ForwardDiagonal);
                g.FillRectangle(brush, panelLeft.ClientRectangle);

                // Decorative circles
                using var circleBrush = new SolidBrush(Color.FromArgb(30, 255, 255, 255));
                g.FillEllipse(circleBrush, -60, -60, 260, 260);
                g.FillEllipse(circleBrush, 200, 320, 300, 300);
                g.FillEllipse(circleBrush, 100, 180, 120, 120);
            };

            lblBrand = new Label
            {
                Text      = "EduTrack Pro",
                Font      = new Font("Segoe UI", 28, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                AutoSize  = true,
                Location  = new Point(40, 80)
            };

            lblBrandSub = new Label
            {
                Text      = "Smart Attendance Management",
                Font      = new Font("Segoe UI", 12),
                ForeColor = Color.FromArgb(200, 255, 255, 255),
                BackColor = Color.Transparent,
                AutoSize  = true,
                Location  = new Point(40, 125)
            };

            // Feature bullets
            lblFeature1 = MakeFeatureLbl("✓  Real-time attendance tracking", 220);
            lblFeature2 = MakeFeatureLbl("✓  Multi-role portal (Instructor / Student)", 258);
            lblFeature3 = MakeFeatureLbl("✓  Automated absence notifications", 296);

            panelLeft.Controls.AddRange(new Control[]
                { lblBrand, lblBrandSub, lblFeature1, lblFeature2, lblFeature3 });

            // Login controls

            panelRight = new Panel
            {
                Size      = new Size(480, 540),
                Location  = new Point(420, 0),
                BackColor = Color.FromArgb(15, 23, 42)
            };

            panelCard = new Panel
            {
                Size      = new Size(380, 430),
                Location  = new Point(50, 55),
                BackColor = Color.FromArgb(30, 41, 59)
            };
            // Rounded corners via Paint
            panelCard.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                int r = 18;
                var rect = new Rectangle(0, 0, panelCard.Width - 1, panelCard.Height - 1);
                using var path = RoundedRect(rect, r);
                using var bg = new SolidBrush(Color.FromArgb(30, 41, 59));
                g.FillPath(bg, path);
                using var pen = new Pen(Color.FromArgb(55, 71, 100), 1.5f);
                g.DrawPath(pen, path);
            };

            lblWelcome = new Label
            {
                Text      = "Welcome Back 👋",
                Font      = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                AutoSize  = true,
                Location  = new Point(30, 30)
            };

            lblSubtitle = new Label
            {
                Text      = "Sign in to your account",
                Font      = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(148, 163, 184),
                BackColor = Color.Transparent,
                AutoSize  = true,
                Location  = new Point(30, 68)
            };

            // Email
            label1 = new Label
            {
                Text      = "Email Address",
                Font      = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.FromArgb(148, 163, 184),
                BackColor = Color.Transparent,
                AutoSize  = true,
                Location  = new Point(30, 110)
            };
            panelEmail = MakeInputPanel(30, 132);
            textBox1 = new System.Windows.Forms.TextBox
            {
                BorderStyle = BorderStyle.None,
                BackColor   = Color.FromArgb(51, 65, 85),
                ForeColor   = Color.White,
                Font        = new Font("Segoe UI", 11),
                Size        = new Size(290, 22),
                Location    = new Point(12, 9),
                Name        = "textBox1"
            };
            textBox1.TextChanged += new EventHandler(this.textBox1_TextChanged);
            panelEmail.Controls.Add(textBox1);

            // Password
            label2 = new Label
            {
                Text      = "Password",
                Font      = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.FromArgb(148, 163, 184),
                BackColor = Color.Transparent,
                AutoSize  = true,
                Location  = new Point(30, 192)
            };
            panelPass = MakeInputPanel(30, 214);
            textBoxPass = new System.Windows.Forms.TextBox
            {
                BorderStyle  = BorderStyle.None,
                BackColor    = Color.FromArgb(51, 65, 85),
                ForeColor    = Color.White,
                Font         = new Font("Segoe UI", 11),
                Size         = new Size(250, 22),
                Location     = new Point(12, 9),
                PasswordChar = '*',
                Name         = "textBoxPass"
            };
            textBoxPass.TextChanged += new EventHandler(this.textBoxPass_TextChanged);

            lblShowPass = new Label
            {
                Text      = "👁",
                Font      = new Font("Segoe UI", 12),
                ForeColor = Color.FromArgb(148, 163, 184),
                BackColor = Color.Transparent,
                Size      = new Size(28, 24),
                Location  = new Point(270, 7),
                Cursor    = Cursors.Hand,
                TextAlign = ContentAlignment.MiddleCenter
            };
            lblShowPass.Click += (s, e) =>
            {
                textBoxPass.PasswordChar = textBoxPass.PasswordChar == '*' ? '\0' : '*';
                lblShowPass.ForeColor    = textBoxPass.PasswordChar == '\0'
                    ? Color.FromArgb(99, 102, 241) : Color.FromArgb(148, 163, 184);
            };
            panelPass.Controls.AddRange(new Control[] { textBoxPass, lblShowPass });

            // Login button
            button1 = new Button
            {
                Text      = "Sign In  →",
                Font      = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 },
                Size      = new Size(320, 46),
                Location  = new Point(30, 275),
                Cursor    = Cursors.Hand,
                Name      = "button1"
            };
            button1.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                var rect = new Rectangle(0, 0, button1.Width - 1, button1.Height - 1);
                using var path = RoundedRect(rect, 10);
                using var brush = new LinearGradientBrush(rect,
                    Color.FromArgb(99, 102, 241),
                    Color.FromArgb(37, 99, 235),
                    LinearGradientMode.Horizontal);
                g.FillPath(brush, path);
                TextRenderer.DrawText(g, button1.Text, button1.Font,
                    rect, Color.White,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            };
            button1.Click += new EventHandler(this.button1_Click);

            // Sign Up row
            label3 = new Label
            {
                Text      = "Don't have an account?",
                Font      = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(148, 163, 184),
                BackColor = Color.Transparent,
                AutoSize  = true,
                Location  = new Point(30, 338)
            };

            label4 = new Label
            {
                Text      = "Sign Up",
                Font      = new Font("Segoe UI", 9, FontStyle.Bold | FontStyle.Underline),
                ForeColor = Color.FromArgb(99, 102, 241),
                BackColor = Color.Transparent,
                AutoSize  = true,
                Location  = new Point(190, 338),
                Cursor    = Cursors.Hand,
                Name      = "label4"
            };
            label4.Click += new EventHandler(this.label4_Click);

            // Exit link
            button2 = new Button
            {
                Text      = "✕",
                Font      = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(148, 163, 184),
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0, MouseOverBackColor = Color.FromArgb(220, 53, 69) },
                BackColor = Color.Transparent,
                Size      = new Size(36, 36),
                Location  = new Point(838, 10),
                Cursor    = Cursors.Hand,
                Name      = "button2"
            };
            button2.Click += new EventHandler(this.button2_Click);

            // Assemble card
            panelCard.Controls.AddRange(new Control[]
            {
                lblWelcome, lblSubtitle,
                label1, panelEmail,
                label2, panelPass,
                button1, label3, label4
            });

            panelRight.Controls.AddRange(new Control[] { panelCard });
            this.Controls.AddRange(new Control[] { panelLeft, panelRight, button2 });

            this.ResumeLayout(false);
        }

        // Helper functions

        private Label MakeFeatureLbl(string text, int y) => new Label
        {
            Text      = text,
            Font      = new Font("Segoe UI", 10),
            ForeColor = Color.FromArgb(219, 234, 254),
            BackColor = Color.Transparent,
            AutoSize  = true,
            Location  = new Point(40, y)
        };

        private Panel MakeInputPanel(int x, int y) => new Panel
        {
            Size      = new Size(320, 40),
            Location  = new Point(x, y),
            BackColor = Color.FromArgb(51, 65, 85)
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
