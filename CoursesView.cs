using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using EduTrackPro.Components;
using EduTrackPro.Data;
using EduTrackPro.Models;

namespace EduTrackPro
{
    public class CoursesView : BaseView
    {
        private FlowLayoutPanel flowPanel;

        public CoursesView() : base("My Courses", "View and manage your assigned academic courses and student enrollments.")
        {
            SetupHeader();
            SetupListArea();
        }

        private void SetupHeader()
        {
            var btnNew = new CustomButton { 
                Text = "+ New Course", 
                Location = new Point(0, 100), 
                Size = new Size(160, 45),
                BackColor = Theme.PrimaryBlue,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnNew.Click += (s, e) => ShowAddCourseDialog();
            this.Controls.Add(btnNew);
        }

        private void SetupListArea()
        {
            flowPanel = new FlowLayoutPanel {
                Location = new Point(0, 160),
                Size = new Size(this.Width - 20, this.Height - 180),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom,
                AutoScroll = true,
                WrapContents = false,
                FlowDirection = FlowDirection.TopDown,
                Padding = new Padding(20, 10, 20, 10)
            };
            this.Controls.Add(flowPanel);
            LoadCourses();
        }

        private void LoadCourses()
        {
            flowPanel.Controls.Clear();
            var courses = DataService.Instance.GetCoursesConnected();
            foreach (var course in courses)
            {
                AddCourseCard(course);
            }
        }

        private void AddCourseCard(Course course)
        {
            var card = new CustomCard { Size = new Size(960, 150), Margin = new Padding(0, 0, 0, 15) };
            
            var imgBox = new Panel { Size = new Size(100, 110), Location = new Point(20, 20), BackColor = Color.FromArgb(245, 246, 250) };
            imgBox.Paint += (s, e) => {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                TextRenderer.DrawText(e.Graphics, "📚", new Font("Segoe UI", 24), imgBox.ClientRectangle, Color.Gray, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            };

            var lblName = new Label { Text = course.CourseName, Font = new Font("Segoe UI", 14, FontStyle.Bold), Location = new Point(140, 25), AutoSize = true };
            var lblCode = new Label { Text = "Course ID: " + course.CourseID, Font = Theme.BodyFont, ForeColor = Color.Gray, Location = new Point(140, 55), AutoSize = true };
            
            var btnView = new CustomButton { 
                Text = "Analytics", 
                Location = new Point(140, 95), 
                Size = new Size(120, 35), 
                BackColor = Theme.PrimaryBlue,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            btnView.Click += (s, e) => ShowCourseDetailsCard(course);

            var btnEnroll = new CustomButton { 
                Text = "Enroll Students", 
                Location = new Point(270, 95), 
                Size = new Size(150, 35), 
                BackColor = Theme.PrimaryBlue,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            btnEnroll.Click += (s, e) => ShowEnrollDialog(course);

            card.Controls.AddRange(new Control[] { imgBox, lblName, lblCode, btnView, btnEnroll });
            flowPanel.Controls.Add(card);
        }

        private void ShowEnrollDialog(Course course)
        {
            var allStudents = DataService.Instance.GetAllStudents();
            var students = new List<Student>();
            foreach (var s in allStudents) {
                if (!DataService.Instance.IsStudentEnrolled(s.StudentID, course.CourseID)) {
                    students.Add(s);
                }
            }

            if (students.Count == 0) { MessageBox.Show("All students are already enrolled in this course.", "Information"); return; }

            Form prompt = new Form() { 
                Width = 450, Height = 500, 
                Text = "Enroll Students in " + course.CourseName, 
                StartPosition = FormStartPosition.CenterParent, 
                FormBorderStyle = FormBorderStyle.FixedDialog,
                BackColor = Color.White
            };

            Label textLabel = new Label() { Left = 20, Top = 15, Text = "Select one or more students:", AutoSize = true, Font = Theme.SmallFont };
            
            CheckedListBox clb = new CheckedListBox() { 
                Left = 20, Top = 45, Width = 390, Height = 300, 
                BorderStyle = BorderStyle.FixedSingle,
                CheckOnClick = true,
                Font = Theme.BodyFont
            };
            foreach (var student in students) clb.Items.Add(student);
            clb.DisplayMember = "Name";

            Button btnAll = new Button() { Text = "Select All", Left = 20, Top = 355, Width = 100 };
            btnAll.Click += (s, e) => { for (int i = 0; i < clb.Items.Count; i++) clb.SetItemChecked(i, true); };

            Button btnNone = new Button() { Text = "Deselect All", Left = 130, Top = 355, Width = 100 };
            btnNone.Click += (s, e) => { for (int i = 0; i < clb.Items.Count; i++) clb.SetItemChecked(i, false); };

            Button confirmation = new CustomButton() { 
                Text = "Enroll Selected", 
                Left = 260, Top = 400, Width = 150, Height = 40,
                BackColor = Theme.PrimaryBlue, ForeColor = Color.White,
                DialogResult = DialogResult.OK 
            };
            
            prompt.Controls.AddRange(new Control[] { textLabel, clb, btnAll, btnNone, confirmation });
            prompt.AcceptButton = confirmation;

            if (prompt.ShowDialog() == DialogResult.OK)
            {
                int count = 0;
                foreach (Student student in clb.CheckedItems)
                {
                    DataService.Instance.EnrollStudent(student.StudentID, course.CourseID);
                    DataService.Instance.PublishNotificationConnected(student.StudentID, "Course Enrollment", "You have been enrolled in " + course.CourseName, "Success");
                    count++;
                }
                
                if (count > 0)
                {
                    DataService.Instance.AddActivity($"Enrolled {count} students in {course.CourseName}");
                    MessageBox.Show($"{count} students have been enrolled successfully!", "Success");
                }
            }
        }

        private void ShowCourseDetailsCard(Course course)
        {
            int studentCount = DataService.Instance.GetStudentCountByCourseConnected(course.CourseID);
            
            Form detailForm = new Form { 
                Size = new Size(450, 400), 
                Text = "Course Analytics", 
                StartPosition = FormStartPosition.CenterParent, 
                FormBorderStyle = FormBorderStyle.FixedDialog,
                BackColor = Color.White,
                MaximizeBox = false, MinimizeBox = false
            };

            var mainCard = new CustomCard { Dock = DockStyle.Fill, Margin = new Padding(20), Padding = new Padding(20) };
            
            var lblTitle = new Label { Text = course.CourseName, Font = new Font("Segoe UI", 16, FontStyle.Bold), Location = new Point(20, 20), AutoSize = true };
            var lblCode = new Label { Text = "ID: " + course.CourseID, Font = Theme.SmallFont, ForeColor = Color.Gray, Location = new Point(20, 55), AutoSize = true };

            var pnlStat = new Panel { Size = new Size(390, 80), Location = new Point(20, 90), BackColor = Color.FromArgb(240, 245, 255) };
            var lblStatVal = new Label { Text = studentCount.ToString(), Font = new Font("Segoe UI", 20, FontStyle.Bold), ForeColor = Theme.PrimaryBlue, Location = new Point(15, 15), AutoSize = true };
            var lblStatLabel = new Label { Text = "Enrolled Students", Font = Theme.SmallFont, Location = new Point(15, 48), AutoSize = true };
            pnlStat.Controls.AddRange(new Control[] { lblStatVal, lblStatLabel });

            var lblDesc = new Label { 
                Text = "This course is currently active. Enrollment data is being synchronized via the Oracle system.", 
                Font = Theme.BodyFont, Location = new Point(20, 190), Size = new Size(390, 60), ForeColor = Color.FromArgb(100, 100, 100) 
            };

            var btnClose = new CustomButton { Text = "Close", Location = new Point(20, 280), Size = new Size(390, 40), BackColor = Color.FromArgb(240, 240, 240), ForeColor = Color.Black };
            btnClose.Click += (s, e) => detailForm.Close();

            mainCard.Controls.AddRange(new Control[] { lblTitle, lblCode, pnlStat, lblDesc, btnClose });
            detailForm.Controls.Add(mainCard);
            detailForm.ShowDialog();
        }

        private void ShowAddCourseDialog()
        {
            Form prompt = new Form() { Width = 400, Height = 250, Text = "Create New Course", StartPosition = FormStartPosition.CenterParent, FormBorderStyle = FormBorderStyle.FixedDialog };
            Label textLabel = new Label() { Left = 20, Top = 20, Text = "Course Name:", AutoSize = true };
            TextBox textBox = new TextBox() { Left = 20, Top = 50, Width = 340, Font = Theme.BodyFont };
            Button confirmation = new Button() { Text = "Add Course", Left = 260, Width = 100, Top = 100, DialogResult = DialogResult.OK };
            
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;

            if (prompt.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(textBox.Text))
            {
                DataService.Instance.AddCourseConnected(textBox.Text);
                LoadCourses();
            }
        }
    }
}
