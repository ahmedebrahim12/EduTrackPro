using EduTrackPro.Data;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.IO;

namespace EduTrackPro.StudentPortal.Controls
{
    public class DashboardUserControl : UserControl
    {
        private System.ComponentModel.IContainer components = null;
        private TableLayoutPanel mainLayout;
        private HorizontalCard cardOverall;
        private HorizontalCard cardAcademicLoad;
        private HorizontalCard cardMonthlyActivity;
        private HorizontalCard cardAchievement;

        private int _studentId;

        public DashboardUserControl(int studentId = 0)
        {
            _studentId = studentId;
            InitializeComponent();
            ApplyRuntimeSettings();
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
            this.mainLayout = new System.Windows.Forms.TableLayoutPanel();
            this.cardOverall = new EduTrackPro.StudentPortal.Controls.HorizontalCard();
            this.cardAcademicLoad = new EduTrackPro.StudentPortal.Controls.HorizontalCard();
            this.cardMonthlyActivity = new EduTrackPro.StudentPortal.Controls.HorizontalCard();
            this.cardAchievement = new EduTrackPro.StudentPortal.Controls.HorizontalCard();
            this.mainLayout.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainLayout
            // 
            this.mainLayout.ColumnCount = 1;
            this.mainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainLayout.Controls.Add(this.cardOverall, 0, 0);
            this.mainLayout.Controls.Add(this.cardAcademicLoad, 0, 1);
            this.mainLayout.Controls.Add(this.cardMonthlyActivity, 0, 2);
            this.mainLayout.Controls.Add(this.cardAchievement, 0, 3);
            this.mainLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainLayout.Location = new System.Drawing.Point(0, 0);
            this.mainLayout.Name = "mainLayout";
            this.mainLayout.Padding = new System.Windows.Forms.Padding(20);
            this.mainLayout.RowCount = 4;
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.mainLayout.Size = new System.Drawing.Size(800, 600);
            this.mainLayout.TabIndex = 0;
            // 
            // cardOverall
            // 
            this.cardOverall.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardOverall.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cardOverall.Location = new System.Drawing.Point(20, 20);
            this.cardOverall.Margin = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.cardOverall.Name = "cardOverall";
            this.cardOverall.Percentage = 87;
            this.cardOverall.MainValue = "87%";
            this.cardOverall.SubText = "Excellent progress - Keep it up!";
            this.cardOverall.Size = new System.Drawing.Size(760, 130);
            this.cardOverall.TabIndex = 0;
            this.cardOverall.Title = "Overall Attendance";
            this.cardOverall.Load += new System.EventHandler(this.cardOverall_Load);
            // 
            // cardAcademicLoad
            // 
            this.cardAcademicLoad.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardAcademicLoad.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cardAcademicLoad.Location = new System.Drawing.Point(20, 160);
            this.cardAcademicLoad.Margin = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.cardAcademicLoad.Name = "cardAcademicLoad";
            this.cardAcademicLoad.Percentage = 83;
            this.cardAcademicLoad.MainValue = "6 Courses";
            this.cardAcademicLoad.SubText = "157 of 180 classes attended";
            this.cardAcademicLoad.Size = new System.Drawing.Size(760, 130);
            this.cardAcademicLoad.TabIndex = 1;
            this.cardAcademicLoad.Title = "Enrolled Courses";
            // 
            // cardMonthlyActivity
            // 
            this.cardMonthlyActivity.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardMonthlyActivity.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cardMonthlyActivity.Location = new System.Drawing.Point(20, 300);
            this.cardMonthlyActivity.Margin = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.cardMonthlyActivity.Name = "cardMonthlyActivity";
            this.cardMonthlyActivity.Percentage = 85;
            this.cardMonthlyActivity.MainValue = "72 Sessions";
            this.cardMonthlyActivity.SubText = "↑ 12% increase from last month";
            this.cardMonthlyActivity.Size = new System.Drawing.Size(760, 130);
            this.cardMonthlyActivity.TabIndex = 2;
            this.cardMonthlyActivity.Title = "Classes This Month";
            // 
            // cardAchievement
            // 
            this.cardAchievement.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardAchievement.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cardAchievement.Location = new System.Drawing.Point(20, 440);
            this.cardAchievement.Margin = new System.Windows.Forms.Padding(0);
            this.cardAchievement.Name = "cardAchievement";
            this.cardAchievement.Percentage = 100;
            this.cardAchievement.MainValue = "15 Days";
            this.cardAchievement.SubText = "Current Streak: 4 Days";
            this.cardAchievement.Size = new System.Drawing.Size(760, 140);
            this.cardAchievement.TabIndex = 3;
            this.cardAchievement.Title = "Perfect Attendance Days";
            // 
            // DashboardUserControl
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.Controls.Add(this.mainLayout);
            this.Name = "DashboardUserControl";
            this.Size = new System.Drawing.Size(800, 600);
            this.mainLayout.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private void ApplyRuntimeSettings()
        {
            if (!DesignMode && LicenseManager.UsageMode != LicenseUsageMode.Designtime)
            {
                this.Load += DashboardUserControl_Load;
            }
        }

        private void DashboardUserControl_Load(object sender, EventArgs e)
        {
            LoadDashboardData();
        }

        /// <summary>
        /// Try to load an icon image: checks several locations including exe dir, root, and subfolders.
        private static Image TryLoadIcon(string fileName)
        {
            string[] searchPaths = {
                AppDomain.CurrentDomain.BaseDirectory,                       
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", ".."), 
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "StudentPortal"),
                Directory.GetCurrentDirectory(),                             
                Path.Combine(Directory.GetCurrentDirectory(), "StudentPortal")
            };

            foreach (var basePath in searchPaths)
            {
                try {
                    string fullPath = Path.GetFullPath(Path.Combine(basePath, fileName));
                    if (File.Exists(fullPath)) return Image.FromFile(fullPath);
                } catch { }
            }
            return null;
        }

        private void LoadDashboardData()
        {
            Image trendIcon    = TryLoadIcon("trend.png");
            Image bookIcon     = TryLoadIcon("open-book.png");
            Image calendarIcon = TryLoadIcon("calendar.png");
            Image awardIcon    = TryLoadIcon("award.png");

            int currentId = _studentId > 0 ? _studentId : Main_student.currentStudentId;
            string attendanceRate = DataService.Instance.GetStudentAttendanceRate(currentId);
            int enrolledCoursesCount = DataService.Instance.GetStudentCourses(currentId).Count;
            int monthlySessions = DataService.Instance.GetStudentMonthlySessions(currentId);
            int perfectDaysCount = DataService.Instance.GetStudentPerfectDays(currentId);

            int overallPercentage = 0;
            int.TryParse(attendanceRate.Replace("%", ""), out overallPercentage);

            this.cardOverall.SetupCard(
                "Overall Attendance", 
                attendanceRate, 
                overallPercentage >= 75 ? "Excellent progress - Keep it up!" : "Needs Improvement", 
                overallPercentage, 
                Color.FromArgb(39, 174, 96), 
                CardIconType.Trend,
                trendIcon
            );

            // Enrollment progress: assume 5 courses is a full load (100%)
            int enrollmentProgress = Math.Min(100, (int)(enrolledCoursesCount * 20.0));
            this.cardAcademicLoad.SetupCard(
                "Enrolled Courses", 
                $"{enrolledCoursesCount} Courses", 
                "Total registered courses", 
                enrollmentProgress, 
                Color.FromArgb(47, 85, 151), 
                CardIconType.Info,
                bookIcon
            );

            // Monthly progress: assume 4 sessions is a typical goal for a short period (100%)
            int monthlyProgress = Math.Min(100, (int)(monthlySessions * 25.0));
            this.cardMonthlyActivity.SetupCard(
                "Classes This Month", 
                $"{monthlySessions} Sessions", 
                "Attendance this month", 
                monthlyProgress, 
                Color.FromArgb(26, 188, 156), 
                CardIconType.Trend,
                calendarIcon
            );

            // Achievement progress: show as percentage of a 5-day perfect goal
            int achievementProgress = Math.Min(100, perfectDaysCount * 20);
            this.cardAchievement.SetupCard(
                "Perfect Attendance Days", 
                $"{perfectDaysCount} Days", 
                "Total perfect days", 
                achievementProgress, 
                Color.FromArgb(241, 196, 15), 
                CardIconType.Trophy,
                awardIcon
            );
        }

        private void cardCourses_Load(object sender, EventArgs e)
        {

        }

        private void cardOverall_Load(object sender, EventArgs e)
        {

        }
    }
}


