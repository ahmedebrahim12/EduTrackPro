using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using EduTrackPro.Components;
using EduTrackPro.Data;
using EduTrackPro.Models;
using EduTrackPro.InstructorReport;

namespace EduTrackPro
{
    public class ReportsView : BaseView
    {
        private ComboBox Category_cmb;
        private ComboBox Session_cmb;
        private DateTimePicker StartDate_txt;
        private DateTimePicker EndDate_txt;
        private CustomButton GenerateReport_btn;
        private Inst_CrystalReport instReportControl;

        public ReportsView() : base("Crystal Reports", "Instructor attendance report by course.")
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
            Category_cmb = new ComboBox { Location = new Point(20, 42), Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
            Category_cmb.SelectedIndexChanged += (s, e) => LoadSessions();

            var lblSession = new Label { Text = "Select Session", Location = new Point(230, 20), AutoSize = true, Font = Theme.SmallFont, ForeColor = Theme.PrimaryBlue };
            Session_cmb = new ComboBox { Location = new Point(230, 42), Width = 220, DropDownStyle = ComboBoxStyle.DropDownList };

            var lblStart = new Label { Text = "Start Date", Location = new Point(460, 20), AutoSize = true, Font = Theme.SmallFont };
            StartDate_txt = new DateTimePicker { Location = new Point(460, 42), Width = 150, Format = DateTimePickerFormat.Short };

            var lblEnd = new Label { Text = "End Date", Location = new Point(620, 20), AutoSize = true, Font = Theme.SmallFont };
            EndDate_txt = new DateTimePicker { Location = new Point(620, 42), Width = 150, Format = DateTimePickerFormat.Short };

            GenerateReport_btn = new CustomButton {
                Text = "Generate Report",
                Location = new Point(790, 35),
                Size = new Size(180, 45),
                BackColor = Theme.PrimaryBlue,
                ForeColor = Color.White
            };
            GenerateReport_btn.Click += GenerateReport_btn_Click;

            card.Controls.AddRange(new Control[] {
                lblCategory, Category_cmb,
                lblSession, Session_cmb,
                lblStart, StartDate_txt,
                lblEnd, EndDate_txt,
                GenerateReport_btn
            });
            this.Controls.Add(card);

            instReportControl = new Inst_CrystalReport
            {
                Location = new Point(0, 230),
                Size = new Size(1150, 500),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom
            };
            this.Controls.Add(instReportControl);

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
            if (Category_cmb.SelectedItem is Course course)
            {
                instReportControl.LoadInstructorAttendanceReport(course.CourseID, StartDate_txt.Value, EndDate_txt.Value);
            }
            else
            {
                MessageBox.Show("Please select a course first.", "No Course Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
