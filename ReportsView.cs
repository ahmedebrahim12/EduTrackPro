using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System.Data;
using EduTrackPro.Components;
using EduTrackPro.Data;
using EduTrackPro.Models;

namespace EduTrackPro
{
    public class ReportsView : BaseView
    {
        private ComboBox Category_cmb; // Course Name
        private ComboBox Session_cmb;   // Dynamic Sessions
        private DateTimePicker StartDate_txt;
        private DateTimePicker EndDate_txt;
        private CustomButton GenerateReport_btn;
        
        private MockCrystalReportViewer crystalReportViewer1;
        private ReportDocument CR;

        public ReportsView() : base("Crystal Reports", "Dynamic reports with session selection.")
        {
            SetupLabUI();
        }

        private void SetupLabUI()
        {
            var card = new CustomCard { 
                Location = new Point(0, 100), 
                Size = new Size(1150, 120), 
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right 
            };

            var lblCategory = new Label { Text = "Course", Location = new Point(20, 20), AutoSize = true, Font = Theme.SmallFont };
            Category_cmb = new ComboBox { Location = new Point(20, 42), Width = 180, DropDownStyle = ComboBoxStyle.DropDownList };
            Category_cmb.SelectedIndexChanged += (s, e) => LoadSessions();
            
            var lblSession = new Label { Text = "Select Session (Dynamic)", Location = new Point(210, 20), AutoSize = true, Font = Theme.SmallFont, ForeColor = Theme.PrimaryBlue };
            Session_cmb = new ComboBox { Location = new Point(210, 42), Width = 220, DropDownStyle = ComboBoxStyle.DropDownList };

            var lblStart = new Label { Text = "Start Date", Location = new Point(440, 20), AutoSize = true, Font = Theme.SmallFont };
            StartDate_txt = new DateTimePicker { Location = new Point(440, 42), Width = 150, Format = DateTimePickerFormat.Short };

            var lblEnd = new Label { Text = "End Date", Location = new Point(600, 20), AutoSize = true, Font = Theme.SmallFont };
            EndDate_txt = new DateTimePicker { Location = new Point(600, 42), Width = 150, Format = DateTimePickerFormat.Short };

            GenerateReport_btn = new CustomButton { 
                Text = "Generate Report", 
                Location = new Point(770, 35), 
                Size = new Size(180, 45), 
                BackColor = Theme.PrimaryBlue, 
                ForeColor = Color.White 
            };
            GenerateReport_btn.Click += GenerateReport_btn_Click;

            card.Controls.AddRange(new Control[] { lblCategory, Category_cmb, lblSession, Session_cmb, lblStart, StartDate_txt, lblEnd, EndDate_txt, GenerateReport_btn });
            this.Controls.Add(card);

            crystalReportViewer1 = new MockCrystalReportViewer {
                Location = new Point(0, 230),
                Size = new Size(1000, 480),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom
            };
            this.Controls.Add(crystalReportViewer1);

            // Populate Courses initially
            var courses = DataService.Instance.GetCoursesConnected();
            Category_cmb.DataSource = courses;
            Category_cmb.DisplayMember = "CourseName";
        }

        private void LoadSessions()
        {
            if (Category_cmb.SelectedItem is Course course)
            {
                var sessions = DataService.Instance.GetSessionsByProcedureConnected(course.CourseID);
                Session_cmb.DataSource = null;
                Session_cmb.DataSource = sessions;
                Session_cmb.DisplayMember = "FormattedSession";
            }
        }

        private void GenerateReport_btn_Click(object sender, EventArgs e)
        {
            CR = new ReportDocument();
            
            // Lab 8 Parameters
            CR.SetParameterValue(0, Category_cmb.Text);
            CR.SetParameterValue(1, StartDate_txt.Value.ToString("dd-MMM-yy").ToUpper());
            CR.SetParameterValue(2, EndDate_txt.Value.ToString("dd-MMM-yy").ToUpper());

            // Sending data to Viewer
            crystalReportViewer1.SetParameterValue("Course", Category_cmb.Text);
            
            // Pass the selected session info to the mock viewer for filtering
            if (Session_cmb.SelectedItem is Session session)
            {
                crystalReportViewer1.SessionID = session.SessionID;
                crystalReportViewer1.SetParameterValue("Session", session.FormattedSession);
            }

            crystalReportViewer1.ReportSource = CR;
        }
    }
}
