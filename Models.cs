using System;

namespace EduTrackPro.Models
{
    public class Course
    {
        public int CourseID { get; set; }
        public string CourseName { get; set; } = string.Empty;
    }

    public class Student
    {
        public int StudentID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }

    public class Session
    {
        public int SessionID { get; set; }
        public int CourseID { get; set; }
        public DateTime SessionDate { get; set; }
        public string StartTime { get; set; } = "09:00 AM";
        public string EndTime { get; set; } = "10:30 AM";
        public string FormattedSession => $"{SessionDate:MMM dd, yyyy} ({StartTime})";
    }

    public class AttendanceRecord
    {
        public int AttendanceID { get; set; }
        public int SessionID { get; set; }
        public int StudentID { get; set; }
        public string Status { get; set; } = "Present";
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}
