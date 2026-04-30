using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace EduTrackPro.StudentPortal.Controls
{
    public enum NotificationType { Warning, Info, Success }

    public class NotificationCard : UserControl
    {
        private Panel container;
        private Panel leftStrip;
        private Label lblTitle;
        private Label lblTime;
        private Label lblCourseBadge;
        private Label lblDescription;
        private Label lblAction;
        private PictureBox picIcon;

        private NotificationType currentType = NotificationType.Info;

        public NotificationCard()
        {
            InitializeComponent();
            SetStyle(ControlStyles.ResizeRedraw, true);
        }

        private void InitializeComponent()
        {
            this.container = new System.Windows.Forms.Panel();
            this.picIcon = new System.Windows.Forms.PictureBox();
            this.leftStrip = new System.Windows.Forms.Panel();
            this.lblTime = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblCourseBadge = new System.Windows.Forms.Label();
            this.lblDescription = new System.Windows.Forms.Label();
            this.lblAction = new System.Windows.Forms.Label();
            this.container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // container
            // 
            this.container.BackColor = System.Drawing.Color.White;
            this.container.Controls.Add(this.picIcon);
            this.container.Controls.Add(this.leftStrip);
            this.container.Controls.Add(this.lblTime);
            this.container.Controls.Add(this.lblTitle);
            this.container.Controls.Add(this.lblCourseBadge);
            this.container.Controls.Add(this.lblDescription);
            this.container.Controls.Add(this.lblAction);
            this.container.Dock = System.Windows.Forms.DockStyle.Fill;
            this.container.Location = new System.Drawing.Point(0, 0);
            this.container.Name = "container";
            this.container.Size = new System.Drawing.Size(600, 160);
            this.container.TabIndex = 0;
            this.container.Paint += new System.Windows.Forms.PaintEventHandler(this.Container_Paint);
            // 
            // picIcon
            // 
            this.picIcon.BackColor = System.Drawing.Color.Transparent;
            this.picIcon.Location = new System.Drawing.Point(24, 24);
            this.picIcon.Name = "picIcon";
            this.picIcon.Size = new System.Drawing.Size(40, 40);
            this.picIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picIcon.TabIndex = 1;
            this.picIcon.TabStop = false;
            this.picIcon.Paint += new System.Windows.Forms.PaintEventHandler(this.PicIcon_Paint);
            // 
            // leftStrip
            // 
            this.leftStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(85)))), ((int)(((byte)(151)))));
            this.leftStrip.Dock = System.Windows.Forms.DockStyle.Left;
            this.leftStrip.Location = new System.Drawing.Point(0, 0);
            this.leftStrip.Name = "leftStrip";
            this.leftStrip.Size = new System.Drawing.Size(6, 160);
            this.leftStrip.TabIndex = 0;
            // 
            // lblTime
            // 
            this.lblTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTime.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblTime.ForeColor = System.Drawing.Color.Gray;
            this.lblTime.Location = new System.Drawing.Point(400, 24);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(180, 20);
            this.lblTime.TabIndex = 2;
            this.lblTime.Text = "Just now";
            this.lblTime.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.Black;
            this.lblTitle.Location = new System.Drawing.Point(80, 22);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(141, 21);
            this.lblTitle.TabIndex = 3;
            this.lblTitle.Text = "Notification Title";
            // 
            // lblCourseBadge
            // 
            this.lblCourseBadge.AutoSize = true;
            this.lblCourseBadge.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(235)))), ((int)(((byte)(250)))));
            this.lblCourseBadge.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.lblCourseBadge.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(85)))), ((int)(((byte)(151)))));
            this.lblCourseBadge.Location = new System.Drawing.Point(80, 48);
            this.lblCourseBadge.Name = "lblCourseBadge";
            this.lblCourseBadge.Padding = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.lblCourseBadge.Size = new System.Drawing.Size(81, 17);
            this.lblCourseBadge.TabIndex = 4;
            this.lblCourseBadge.Text = "Course Code";
            // 
            // lblDescription
            // 
            this.lblDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDescription.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblDescription.Location = new System.Drawing.Point(80, 75);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(500, 45);
            this.lblDescription.TabIndex = 5;
            this.lblDescription.Text = "Description goes here...";
            // 
            // lblAction
            // 
            this.lblAction.AutoSize = true;
            this.lblAction.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblAction.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblAction.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(85)))), ((int)(((byte)(151)))));
            this.lblAction.Location = new System.Drawing.Point(80, 125);
            this.lblAction.Name = "lblAction";
            this.lblAction.Size = new System.Drawing.Size(129, 19);
            this.lblAction.TabIndex = 6;
            this.lblAction.Text = "Action Required...";
            // 
            // NotificationCard
            // 
            this.Controls.Add(this.container);
            this.Margin = new System.Windows.Forms.Padding(0, 0, 0, 16);
            this.Name = "NotificationCard";
            this.Size = new System.Drawing.Size(600, 160);
            this.container.ResumeLayout(false);
            this.container.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picIcon)).EndInit();
            this.ResumeLayout(false);

        }

        public void SetNotification(string title, string message, string course, NotificationType type, string timeStr = "Just now", string action = "")
        {
            this.lblTitle.Text = title;
            this.lblDescription.Text = message;
            this.lblCourseBadge.Text = course;
            this.lblTime.Text = timeStr;
            this.lblAction.Text = action;
            this.lblAction.Visible = !string.IsNullOrEmpty(action);
            
            this.lblCourseBadge.Visible = !string.IsNullOrEmpty(course);
            if (!this.lblCourseBadge.Visible)
            {
                this.lblDescription.Location = new Point(80, 50);
            }
            else
            {
                this.lblDescription.Location = new Point(80, 75);
            }

            this.currentType = type;

            Color themeColor;
            Color bgColor;

            switch (type)
            {
                case NotificationType.Warning:
                    themeColor = Color.FromArgb(243, 156, 18); 
                    bgColor = Color.Transparent;
                    break;
                case NotificationType.Success:
                    themeColor = Color.FromArgb(39, 174, 96);
                    bgColor = Color.Transparent;
                    break;
                case NotificationType.Info:
                default:
                    themeColor = Color.FromArgb(47, 85, 151);
                    bgColor = Color.Transparent;
                    break;
            }

            this.leftStrip.BackColor = themeColor;
            
            
            this.picIcon.BackColor = bgColor;
            
            
            this.lblCourseBadge.BackColor = bgColor;
            this.lblCourseBadge.ForeColor = themeColor;
            
            this.picIcon.Invalidate();
            this.container.Invalidate();
        }

        private void Container_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            var r = container.ClientRectangle;
            r.Width -= 1;
            r.Height -= 1;
            
            Color borderColor;
            switch (currentType)
            {
                case NotificationType.Warning: borderColor = Color.FromArgb(243, 156, 18); break;
                case NotificationType.Success: borderColor = Color.FromArgb(39, 174, 96); break;
                default: borderColor = Color.FromArgb(47, 85, 151); break;
            }
            
            
            using (var pen = new Pen(Color.FromArgb(100, borderColor), 1))
            {
                g.DrawRectangle(pen, r);
            }
        }

        private void PicIcon_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            
            Color iconColor;
            switch (currentType)
            {
                case NotificationType.Warning: iconColor = Color.FromArgb(243, 156, 18); break;
                case NotificationType.Success: iconColor = Color.FromArgb(39, 174, 96); break;
                default: iconColor = Color.FromArgb(47, 85, 151); break;
            }

            using (var pen = new Pen(iconColor, 3))
            {
                if (currentType == NotificationType.Info)
                {
                    // draw 'i'
                    g.DrawLine(pen, 20, 12, 20, 14); // dot
                    g.DrawLine(pen, 20, 18, 20, 28); // line
                }
                else if (currentType == NotificationType.Warning)
                {
                    // draw '!'
                    g.DrawLine(pen, 20, 10, 20, 24); // line
                    g.DrawLine(pen, 20, 28, 20, 30); // dot
                }
                else if (currentType == NotificationType.Success)
                {
                    // draw checkmark
                    g.DrawLine(pen, 12, 20, 18, 26);
                    g.DrawLine(pen, 18, 26, 28, 14);
                }
                
                // Draw circle around
                g.DrawEllipse(pen, 2, 2, 36, 36);
            }
        }
    }
}

