using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace EduTrackPro.StudentPortal.Controls
{
    public class NotificationsPanel : UserControl
    {
        private System.ComponentModel.IContainer components = null;
        private FlowLayoutPanel flow;

        public NotificationsPanel()
        {
            InitializeComponent();

            if (!DesignMode && LicenseManager.UsageMode != LicenseUsageMode.Designtime)
            {
                LoadSampleAlerts();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.flow = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();

            this.flow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flow.AutoScroll = true;
            this.flow.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flow.WrapContents = false;
            this.flow.BackColor = System.Drawing.Color.FromArgb(250, 250, 250);
            this.flow.Padding = new System.Windows.Forms.Padding(24);
            this.flow.Name = "flow";

            this.Controls.Add(this.flow);

            this.Name = "NotificationsPanel";
            this.Size = new System.Drawing.Size(800, 600);

            this.ResumeLayout(false);
        }

        public void LoadAlerts(IEnumerable<Tuple<string, string, string>> alerts)
        {
            if (flow == null) return;

            flow.SuspendLayout();
            flow.Controls.Clear();

            foreach (var a in alerts)
            {
                AlertCard card = new AlertCard();
                card.Width = Math.Max(600, Math.Max(0, flow.ClientSize.Width - 48));
                card.Margin = new Padding(0, 0, 0, 12);
                card.SetData(a.Item1, a.Item2, a.Item3);
                flow.Controls.Add(card);
            }
            flow.ResumeLayout();
        }

        public void LoadSampleAlerts()
        {
            var sample = new List<Tuple<string, string, string>>
            {
                Tuple.Create(DateTime.Now.AddDays(-1).ToString("g"), "Attendance", "You missed 2 classes in Mathematics."),
                Tuple.Create(DateTime.Now.AddDays(-3).ToString("g"), "Course", "New assignment posted for Chemistry."),
                Tuple.Create(DateTime.Now.AddDays(-7).ToString("g"), "Attendance", "Your overall attendance dropped below 75% in Physics."),
                Tuple.Create(DateTime.Now.AddDays(-10).ToString("g"), "System", "Profile updated successfully.")
            };

            LoadAlerts(sample);
        }
    }
}
