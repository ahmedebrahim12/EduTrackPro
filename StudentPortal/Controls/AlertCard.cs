using System;
using System.Drawing;
using System.Windows.Forms;

namespace EduTrackPro.StudentPortal.Controls
{
    public class AlertCard : UserControl
    {
        private Panel container;
        private Label lblDate;
        private Label lblType;
        private Label lblMessage;

        public AlertCard()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.container = new System.Windows.Forms.Panel();
            this.lblDate = new System.Windows.Forms.Label();
            this.lblType = new System.Windows.Forms.Label();
            this.lblMessage = new System.Windows.Forms.Label();
            this.container.SuspendLayout();
            this.SuspendLayout();
            // 
            // container
            // 
            this.container.BackColor = System.Drawing.Color.White;
            this.container.Controls.Add(this.lblDate);
            this.container.Controls.Add(this.lblType);
            this.container.Controls.Add(this.lblMessage);
            this.container.Dock = System.Windows.Forms.DockStyle.Fill;
            this.container.Location = new System.Drawing.Point(0, 0);
            this.container.Name = "container";
            this.container.Padding = new System.Windows.Forms.Padding(12);
            this.container.Size = new System.Drawing.Size(150, 88);
            this.container.TabIndex = 0;
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblDate.ForeColor = System.Drawing.Color.Gray;
            this.lblDate.Location = new System.Drawing.Point(12, 10);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(0, 13);
            this.lblDate.TabIndex = 0;
            // 
            // lblType
            // 
            this.lblType.AutoSize = true;
            this.lblType.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(85)))), ((int)(((byte)(151)))));
            this.lblType.Location = new System.Drawing.Point(12, 28);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(0, 15);
            this.lblType.TabIndex = 1;
            // 
            // lblMessage
            // 
            this.lblMessage.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblMessage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.lblMessage.Location = new System.Drawing.Point(12, 44);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(420, 36);
            this.lblMessage.TabIndex = 2;
            // 
            // AlertCard
            // 
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.container);
            this.Name = "AlertCard";
            this.Size = new System.Drawing.Size(150, 88);
            this.container.ResumeLayout(false);
            this.container.PerformLayout();
            this.ResumeLayout(false);

        }

        private void Container_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            var r = container.ClientRectangle;
            r.Width -= 1;
            r.Height -= 1;
            using (var pen = new Pen(Color.FromArgb(220, 220, 220), 1))
            {
                g.DrawRectangle(pen, r);
            }
        }

        public void SetData(string date, string type, string message)
        {
            lblDate.Text = date;
            lblType.Text = type;
            lblMessage.Text = message;
        }
    }
}
