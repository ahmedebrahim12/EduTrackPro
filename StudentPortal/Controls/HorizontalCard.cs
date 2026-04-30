using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace EduTrackPro.StudentPortal.Controls
{
    public enum CardIconType { Trend, Info, Trophy }

    public class HorizontalCard : UserControl
    {
        private Panel container;
        private Label lblTitle;
        private Label lblPercentStatus;
        private Panel progressBackground;
        private Panel progressFill;
        private PictureBox picTrend;
        private Label lblProgressLeft;
        private Label lblSubText;

        private int attendanceValue = 0;
        private Color? customProgressColor = null;
        private CardIconType iconType = CardIconType.Trend;
        private bool showProgress = true;
        private Image cardIcon = null;

        public HorizontalCard()
        {
            InitializeComponent();
            SetStyle(ControlStyles.ResizeRedraw, true);
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            
            UpdateLayoutPositions();
        }

        private void InitializeComponent()
        {
            this.container = new System.Windows.Forms.Panel();
            this.picTrend = new System.Windows.Forms.PictureBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblPercentStatus = new System.Windows.Forms.Label();
            this.progressBackground = new System.Windows.Forms.Panel();
            this.progressFill = new System.Windows.Forms.Panel();
            this.lblProgressLeft = new System.Windows.Forms.Label();
            this.lblSubText = new System.Windows.Forms.Label();
            this.container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picTrend)).BeginInit();
            this.progressBackground.SuspendLayout();
            this.SuspendLayout();
            // 
            // container
            // 
            this.container.BackColor = System.Drawing.Color.White;
            this.container.Controls.Add(this.picTrend);
            this.container.Controls.Add(this.lblTitle);
            this.container.Controls.Add(this.lblPercentStatus);
            this.container.Controls.Add(this.progressBackground);
            this.container.Controls.Add(this.lblProgressLeft);
            this.container.Controls.Add(this.lblSubText);
            this.container.Dock = System.Windows.Forms.DockStyle.Fill;
            this.container.Location = new System.Drawing.Point(0, 0);
            this.container.Name = "container";
            this.container.Padding = new System.Windows.Forms.Padding(14);
            this.container.Size = new System.Drawing.Size(150, 150);
            this.container.TabIndex = 0;
            // 
            // picTrend
            // 
            this.picTrend.BackColor = System.Drawing.Color.Transparent;
            this.picTrend.Location = new System.Drawing.Point(14, 35);
            this.picTrend.Name = "picTrend";
            this.picTrend.Padding = new System.Windows.Forms.Padding(10);
            this.picTrend.Size = new System.Drawing.Size(48, 48);
            this.picTrend.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picTrend.TabIndex = 0;
            this.picTrend.TabStop = false;
            // 
            // lblTitle
            // 
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.lblTitle.Location = new System.Drawing.Point(72, 35);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(400, 22);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblPercentStatus
            // 
            this.lblPercentStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPercentStatus.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblPercentStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this.lblPercentStatus.Location = new System.Drawing.Point(536, 35);
            this.lblPercentStatus.Name = "lblPercentStatus";
            this.lblPercentStatus.Size = new System.Drawing.Size(200, 22);
            this.lblPercentStatus.TabIndex = 2;
            this.lblPercentStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // progressBackground
            // 
            this.progressBackground.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBackground.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.progressBackground.Controls.Add(this.progressFill);
            this.progressBackground.Location = new System.Drawing.Point(72, 69);
            this.progressBackground.Name = "progressBackground";
            this.progressBackground.Size = new System.Drawing.Size(510, 12);
            this.progressBackground.TabIndex = 3;
            // 
            // progressFill
            // 
            this.progressFill.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(174)))), ((int)(((byte)(96)))));
            this.progressFill.Location = new System.Drawing.Point(0, 0);
            this.progressFill.Name = "progressFill";
            this.progressFill.Size = new System.Drawing.Size(0, 12);
            this.progressFill.TabIndex = 0;
            // 
            // lblProgressLeft
            // 
            this.lblProgressLeft.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblProgressLeft.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.lblProgressLeft.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.lblProgressLeft.Location = new System.Drawing.Point(512, 85);
            this.lblProgressLeft.Name = "lblProgressLeft";
            this.lblProgressLeft.Size = new System.Drawing.Size(60, 18);
            this.lblProgressLeft.TabIndex = 4;
            this.lblProgressLeft.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblSubText
            // 
            this.lblSubText.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblSubText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(120)))), ((int)(((byte)(120)))));
            this.lblSubText.Location = new System.Drawing.Point(72, 87);
            this.lblSubText.Name = "lblSubText";
            this.lblSubText.Size = new System.Drawing.Size(400, 18);
            this.lblSubText.TabIndex = 5;
            this.lblSubText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // HorizontalCard
            // 
            this.Controls.Add(this.container);
            this.Name = "HorizontalCard";
            this.picTrend.Paint += new System.Windows.Forms.PaintEventHandler(this.PicTrend_Paint);
            this.container.Paint += new System.Windows.Forms.PaintEventHandler(this.Container_Paint);
            this.container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picTrend)).EndInit();
            this.progressBackground.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private void Container_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            var r = container.ClientRectangle;
            r.Width -= 1;
            r.Height -= 1;
            using (var pen = new Pen(Color.FromArgb(230, 230, 230), 1))
            {
                g.DrawRectangle(pen, r);
            }
        }

        private void PicTrend_Paint(object sender, PaintEventArgs e)
        {
            if (picTrend.Image != null) return;

            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            
            if (iconType == CardIconType.Info)
            {
                using (var pen = new Pen(Color.FromArgb(47, 85, 151), 2))
                {
                    g.DrawEllipse(pen, 10, 10, 20, 20);
                    g.DrawLine(pen, 20, 16, 20, 16.5f); // dot
                    g.DrawLine(pen, 20, 20, 20, 26);    // line
                }
                return;
            }
            else if (iconType == CardIconType.Trophy)
            {
                using (var pen = new Pen(Color.FromArgb(241, 196, 15), 2))
                {
                    PointF[] star = {
                        new PointF(20, 8), new PointF(23, 15),
                        new PointF(30, 15), new PointF(24, 20),
                        new PointF(26, 28), new PointF(20, 23),
                        new PointF(14, 28), new PointF(16, 20),
                        new PointF(10, 15), new PointF(17, 15)
                    };
                    g.DrawPolygon(pen, star);
                }
                return;
            }

            Point[] up = { new Point(8, 28), new Point(20, 8), new Point(32, 28) };
            Point[] down = { new Point(8, 12), new Point(20, 32), new Point(32, 12) };
            using (var pen = new Pen(attendanceValue >= 50 ? Color.FromArgb(39, 174, 96) : Color.FromArgb(192, 57, 43), 3))
            {
                if (attendanceValue >= 50) g.DrawLines(pen, up);
                else g.DrawLines(pen, down);
            }
        }

        private void HorizontalCard_Resize(object sender, EventArgs e)
        {
            UpdateLayoutPositions();
        }

        private void UpdateLayoutPositions()
        {
            if (container == null) return;
            

            int contentHeight = 85; 
            int topOffset = (Height - contentHeight) / 2;
            if (topOffset < 10) topOffset = 10;

            int width = Width - container.Padding.Left - container.Padding.Right;
            
            picTrend.Location = new Point(16, topOffset);
            lblTitle.Location = new Point(72, topOffset);
            lblTitle.Width = Math.Max(120, width - 300);
            
            lblPercentStatus.Location = new Point(Width - container.Padding.Right - 200, topOffset);
            
            progressBackground.Width = Math.Max(120, width - 130);
            progressBackground.Location = new Point(72, topOffset + 34);
            
            lblProgressLeft.Location = new Point(progressBackground.Right - 60, progressBackground.Bottom + 4);
            lblSubText.Location = new Point(72, progressBackground.Bottom + 6);
            
            UpdateProgressFill();
        }

        private void UpdateProgressFill()
        {
            if (progressBackground.Width <= 0 || !showProgress) return;
            int newWidth = (int)Math.Round(progressBackground.Width * attendanceValue / 100.0);
            progressFill.Width = Math.Max(0, newWidth);
            progressFill.Height = progressBackground.Height;
            progressFill.BackColor = customProgressColor ?? GetColorForValue(attendanceValue);
        }

        private Color GetColorForValue(int value)
        {
            if (value > 75) return Color.FromArgb(39, 174, 96); // Green
            if (value >= 50) return Color.FromArgb(243, 156, 18); // Orange
            return Color.FromArgb(192, 57, 43); // Red
        }

        [Category("Custom"), Description("Optional icon image to display")]
        public Image CardIcon
        {
            get { return cardIcon; }
            set
            {
                cardIcon = value;
                picTrend.Image = value;
                picTrend.SizeMode = PictureBoxSizeMode.Zoom;
                picTrend.Invalidate();
            }
        }

        public string Title
        {
            get { return lblTitle.Text; }
            set { lblTitle.Text = value; }
        }

        [Category("Custom"), Description("The main value displayed (e.g. 87%)")]
        public string MainValue
        {
            get { return lblPercentStatus.Text; }
            set { lblPercentStatus.Text = value; }
        }

        [Category("Custom"), Description("The sub-text displayed below the title")]
        public string SubText
        {
            get { return lblSubText.Text; }
            set 
            { 
                lblSubText.Text = value;
                lblSubText.Visible = !string.IsNullOrEmpty(value);
            }
        }

        [Category("Custom"), Description("Attendance percentage shown on the card")]
        public int Percentage
        {
            get { return attendanceValue; }
            set { UpdateAttendance(value); }
        }

        public void SetupCard(string title, string mainValue, string subText, int progressValue, Color? color = null, CardIconType icon = CardIconType.Trend, Image iconImage = null)
        {
            this.Title = title;
            this.CardIcon = iconImage;
            this.lblPercentStatus.Text = mainValue;
            this.lblSubText.Text = subText;
            this.lblSubText.Visible = !string.IsNullOrEmpty(subText);
            
            this.attendanceValue = Math.Max(0, Math.Min(100, progressValue));
            this.customProgressColor = color;
            this.iconType = icon;
            
            this.showProgress = progressValue >= 0;
            this.progressBackground.Visible = showProgress;
            this.lblProgressLeft.Visible = showProgress;
            
            if (showProgress)
            {
                lblProgressLeft.Text = $"{attendanceValue}%";
                UpdateProgressFill();
            }
            
            picTrend.Invalidate();
            UpdateLayoutPositions();
        }

        public void UpdateAttendance(int value)
        {
            if (value < 0) value = 0;
            if (value > 100) value = 100;
            attendanceValue = value;
            
            this.lblPercentStatus.Text = $"{attendanceValue}%";
            this.lblSubText.Text = (attendanceValue >= 75 ? "Great job!" : attendanceValue >= 50 ? "Monitor" : "Needs attention");
            this.lblSubText.Visible = true;
            this.lblProgressLeft.Text = $"{attendanceValue}%";
            
            this.showProgress = true;
            this.progressBackground.Visible = true;
            this.lblProgressLeft.Visible = true;
            this.iconType = CardIconType.Trend;
            this.customProgressColor = null;

            UpdateProgressFill();
            picTrend.Invalidate();
            UpdateLayoutPositions();
        }
    }
}
