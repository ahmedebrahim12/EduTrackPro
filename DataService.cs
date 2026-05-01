using System;
using System.Collections.Generic;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using System.Linq;
using EduTrackPro.Models;

namespace EduTrackPro.Data
{
    public class DataService
    {
        private static DataService? instance;
        public static DataService Instance => instance ??= new DataService();

        public static string ordb = "Data Source=localhost:1521/xepdb1;User Id=SYS;Password=sys;DBA Privilege=SYSDBA;";
        private OracleConnection? conn;

        private DataService()
        {
        }

        private bool TryConnect()
        {
            try
            {
                if (conn == null) conn = new OracleConnection(ordb);
                if (conn.State != ConnectionState.Open) conn.Open();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void ResetConnection()
        {
            conn?.Dispose();
            conn = null;
            TryConnect();
        }

        public List<Course> GetCoursesConnected()
        {
            List<Course> courses = new List<Course>();
            if (TryConnect())
            {
                try {
                    OracleCommand cmd = new OracleCommand("SELECT CourseID, CourseName FROM Course", conn);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                            courses.Add(new Course { CourseID = reader.GetInt32(0), CourseName = reader.GetString(1) });
                    }
                } catch (Exception ex) { System.Windows.Forms.MessageBox.Show("Error getting courses: " + ex.Message); }
            }
            return courses;
        }

        public void AddCourseConnected(string name)
        {
            if (TryConnect())
            {
                try {
                    int nextId = 1;
                    using (var cmdId = new OracleCommand("SELECT NVL(MAX(CourseID), 0) + 1 FROM Course", conn)) {
                        nextId = Convert.ToInt32(cmdId.ExecuteScalar());
                    }
                    OracleCommand cmd = new OracleCommand("INSERT INTO Course (CourseID, CourseName) VALUES (:id, :name)", conn);
                    cmd.BindByName = true;
                    cmd.Parameters.Add("id", nextId);
                    cmd.Parameters.Add("name", name);
                    cmd.ExecuteNonQuery();
                    new OracleCommand("COMMIT", conn).ExecuteNonQuery();
                } catch (Exception ex) {
                    System.Windows.Forms.MessageBox.Show("Error adding course: " + ex.Message);
                }
            }
            PublishNotificationConnected(null, "Course Added", $"Course '{name}' has been created.", "Success");
        }

        public int GetStudentCountByCourseConnected(int courseId)
        {
            if (TryConnect())
            {
                try {
                    OracleCommand cmd = new OracleCommand("GET_STUDENT_COUNT", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.BindByName = true;
                    cmd.Parameters.Add("p_course_id", courseId);
                    cmd.Parameters.Add("p_count", OracleDbType.Int32, ParameterDirection.Output);
                    cmd.ExecuteNonQuery();
                    return Convert.ToInt32(cmd.Parameters["p_count"].Value.ToString());
                } catch (Exception ex) {
                    System.Windows.Forms.MessageBox.Show("Error getting student count: " + ex.Message);
                }
            }
            return 0;
        }

        public void UpdateCourseDisconnected(int courseId, string newName)
        {
            if (TryConnect())
            {
                try {
                    string sql = "SELECT * FROM Course WHERE CourseID = :id";
                    OracleDataAdapter adapter = new OracleDataAdapter(sql, ordb);
                    adapter.SelectCommand.Parameters.Add("id", courseId);
                    
                    OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds, "Course");

                    if (ds.Tables["Course"].Rows.Count > 0)
                    {
                        ds.Tables["Course"].Rows[0]["CourseName"] = newName;
                        adapter.Update(ds, "Course");
                    }
                } catch (Exception ex) {
                    System.Windows.Forms.MessageBox.Show("Error updating course: " + ex.Message);
                }
            }
        }

        public DataTable GetStudentsForAttendance(int courseId)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("StudentID", typeof(int));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Status", typeof(string));

            if (TryConnect())
            {
                try {
                    string sql = @"SELECT StudentID, Name FROM Student 
                                 WHERE StudentID IN (SELECT StudentID FROM Enrollment WHERE CourseID = :cid)";
                    OracleCommand cmd = new OracleCommand(sql, conn);
                    cmd.BindByName = true;
                    cmd.Parameters.Add("cid", courseId);
                    using (var reader = cmd.ExecuteReader()) {
                        while (reader.Read()) {
                            dt.Rows.Add(Convert.ToInt32(reader["StudentID"]), reader["Name"].ToString(), "Undefined");
                        }
                    }
                } catch (Exception ex) {
                    System.Windows.Forms.MessageBox.Show("Error getting students for attendance: " + ex.Message);
                }
            }
            return dt;
        }

        public List<Session> GetSessionsByProcedureConnected(int courseId)
        {
            List<Session> sessions = new List<Session>();
            if (TryConnect())
            {
                try {
                    OracleCommand cmd = new OracleCommand("GET_COURSE_SESSIONS", conn);
                    cmd.BindByName = true;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("p_course_id", OracleDbType.Int32).Value = courseId;
                    cmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            sessions.Add(new Session {
                                SessionID = reader.GetInt32(0),
                                CourseID = courseId,
                                SessionDate = reader.IsDBNull(2) ? DateTime.Now : reader.GetDateTime(2),
                                StartTime = reader.IsDBNull(3) ? "" : reader.GetString(3),
                                EndTime = reader.IsDBNull(4) ? "" : reader.GetString(4)
                            });
                        }
                    }
                } 
                catch (Exception ex) 
                {
                    System.Windows.Forms.MessageBox.Show("Error getting sessions: " + ex.Message);
                    try {
                        OracleCommand cmd = new OracleCommand("SELECT SessionID, CourseID, SessionDate, StartTime, EndTime FROM courseSession WHERE CourseID = :cid", conn);
                        cmd.BindByName = true;
                        cmd.Parameters.Add("cid", OracleDbType.Int32).Value = courseId;
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                                sessions.Add(new Session {
                                    SessionID = reader.GetInt32(0),
                                    CourseID = reader.GetInt32(1),
                                    SessionDate = reader.IsDBNull(2) ? DateTime.Now : reader.GetDateTime(2),
                                    StartTime = reader.IsDBNull(3) ? "" : reader.GetString(3),
                                    EndTime = reader.IsDBNull(4) ? "" : reader.GetString(4)
                                });
                        }
                    } catch (Exception ex2) { System.Windows.Forms.MessageBox.Show("Fallback failed: " + ex2.Message); }
                }
            }
            return sessions;
        }

        public void SaveAttendanceDisconnected(int sessionId, int courseId, DataTable data) => SaveAttendanceConnected(sessionId, courseId, data);

        public void SaveSessionConnected(Session session)
        {
            if (TryConnect())
            {
                try {
                    int nextId = 1;
                    using (var cmdId = new OracleCommand("SELECT NVL(MAX(SessionID), 0) + 1 FROM courseSession", conn)) {
                        nextId = Convert.ToInt32(cmdId.ExecuteScalar());
                    }

                    OracleCommand cmd = new OracleCommand("INSERT INTO courseSession (SessionID, CourseID, SessionDate, StartTime, EndTime) VALUES (:sid, :cid, :d, :st, :et)", conn);
                    cmd.BindByName = true;
                    cmd.Parameters.Add("sid", OracleDbType.Int32).Value = nextId;
                    cmd.Parameters.Add("cid", OracleDbType.Int32).Value = session.CourseID;
                    cmd.Parameters.Add("d", OracleDbType.Date).Value = session.SessionDate;
                    cmd.Parameters.Add("st", OracleDbType.Varchar2).Value = session.StartTime;
                    cmd.Parameters.Add("et", OracleDbType.Varchar2).Value = session.EndTime;
                    cmd.ExecuteNonQuery();
                    new OracleCommand("COMMIT", conn).ExecuteNonQuery();
                    PublishNotificationConnected(null, "New Session Scheduled", $"Session for {session.SessionDate.ToShortDateString()} at {session.StartTime}.", "Info");
                } catch (Exception ex) {
                    System.Windows.Forms.MessageBox.Show("Error saving session: " + ex.Message);
                }
            }
        }

        public void SaveAttendanceConnected(int sessionId, int courseId, DataTable attendanceData)
        {
            if (TryConnect())
            {
                try {
                    // First clear existing for this session to allow "Edit"
                    OracleCommand del = new OracleCommand("DELETE FROM Attendance WHERE SessionID = :sid", conn);
                    del.BindByName = true;
                    del.Parameters.Add("sid", sessionId);
                    del.ExecuteNonQuery();

                    int nextId = 1;
                    using (var cmdId = new OracleCommand("SELECT NVL(MAX(AttendanceID), 0) + 1 FROM Attendance", conn)) {
                        nextId = Convert.ToInt32(cmdId.ExecuteScalar());
                    }

                    var processedStudents = new HashSet<int>();
                    foreach (DataRow row in attendanceData.Rows)
                    {
                        int stid = Convert.ToInt32(row["StudentID"]);
                        if (processedStudents.Contains(stid)) continue;
                        processedStudents.Add(stid);

                        OracleCommand cmd = new OracleCommand("INSERT INTO Attendance (AttendanceID, SessionID, StudentID, Status) VALUES (:aid, :sid, :stid, :stat)", conn);
                        cmd.BindByName = true;
                        cmd.Parameters.Add("aid", nextId++);
                        cmd.Parameters.Add("sid", sessionId);
                        cmd.Parameters.Add("stid", stid);
                        cmd.Parameters.Add("stat", row["Status"].ToString());
                        cmd.ExecuteNonQuery();
                    }
                    new OracleCommand("COMMIT", conn).ExecuteNonQuery();
                } catch (Exception ex) {
                    System.Windows.Forms.MessageBox.Show("Error saving attendance: " + ex.Message);
                }
            }
        }

        public Dictionary<int, string> GetExistingAttendance(int sessionId)
        {
            var dict = new Dictionary<int, string>();
            if (TryConnect())
            {
                try {
                    OracleCommand cmd = new OracleCommand("SELECT StudentID, Status FROM Attendance WHERE SessionID = :sid", conn);
                    cmd.BindByName = true;
                    cmd.Parameters.Add("sid", sessionId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                            dict[reader.GetInt32(0)] = reader.GetString(1);
                    }
                } catch (Exception ex) {
                    System.Windows.Forms.MessageBox.Show("Error getting existing attendance: " + ex.Message);
                }
            }
            return dict;
        }

        public bool IsAttendanceTaken(int sessionId)
        {
            if (TryConnect())
            {
                try {
                    OracleCommand cmd = new OracleCommand("SELECT COUNT(*) FROM Attendance WHERE SessionID = :sid", conn);
                    cmd.BindByName = true;
                    cmd.Parameters.Add("sid", sessionId);
                    return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                } catch { }
            }
            return false;
        }

        public void PublishNotificationConnected(int? recipientId, string title, string desc, string type)
        {
            if (TryConnect())
            {
                try {
                    int nextId = 1;
                    using (var cmdId = new OracleCommand("SELECT NVL(MAX(NotificationID), 0) + 1 FROM Notification", conn)) {
                        nextId = Convert.ToInt32(cmdId.ExecuteScalar());
                    }

                    OracleCommand cmd = new OracleCommand("INSERT INTO Notification (NotificationID, RecipientID, Title, Description, Type, CreatedAt) VALUES (:nid, :rid, :t, :d, :tp, :dt)", conn);
                    cmd.BindByName = true;
                    cmd.Parameters.Add("nid", nextId);
                    cmd.Parameters.Add("rid", recipientId == null ? DBNull.Value : (object)recipientId);
                    cmd.Parameters.Add("t", title);
                    cmd.Parameters.Add("d", desc);
                    cmd.Parameters.Add("tp", type);
                    cmd.Parameters.Add("dt", OracleDbType.TimeStamp).Value = DateTime.Now;
                    cmd.ExecuteNonQuery();
                    new OracleCommand("COMMIT", conn).ExecuteNonQuery();
                } catch (Exception ex) {
                    System.Windows.Forms.MessageBox.Show("Error publishing notification: " + ex.Message);
                }
            }
        }

        public List<string> GetNotificationHistory()
        {
            List<string> history = new List<string>();
            if (TryConnect())
            {
                try {
                    OracleCommand cmd = new OracleCommand("SELECT Title, Description, Type, RecipientID FROM Notification WHERE Title <> 'Activity Log' ORDER BY CreatedAt DESC", conn);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string target = reader.IsDBNull(3) ? "ALL" : $"STU:{reader.GetInt32(3)}";
                            history.Add($"[{reader.GetString(2)}] {target} - {reader.GetString(0)}: {reader.GetString(1)}");
                        }
                    }
                } catch (Exception ex) { System.Windows.Forms.MessageBox.Show("Error getting notification history: " + ex.Message); }
            }
            return history;
        }

        public bool SaveAttendance(int sessionId, int studentId, string status)
        {
            if (TryConnect())
            {
                try
                {
                    int nextId = 1;
                    using (var cmdId = new OracleCommand("SELECT NVL(MAX(AttendanceID), 0) + 1 FROM Attendance", conn)) {
                        nextId = Convert.ToInt32(cmdId.ExecuteScalar());
                    }

                    string sql = "INSERT INTO Attendance (AttendanceID, SessionID, StudentID, Status) VALUES (:aid, :sid, :stId, :status)";
                    OracleCommand cmd = new OracleCommand(sql, conn);
                    cmd.BindByName = true;
                    cmd.Parameters.Add("aid", nextId);
                    cmd.Parameters.Add("sid", sessionId);
                    cmd.Parameters.Add("stId", studentId);
                    cmd.Parameters.Add("status", status);
                    cmd.ExecuteNonQuery();
                    new OracleCommand("COMMIT", conn).ExecuteNonQuery();
                    return true;
                }
                catch (Exception ex) { System.Windows.Forms.MessageBox.Show("Error saving attendance record: " + ex.Message); }
            }
            return false; 
        }

        public DataTable GetAttendanceReport(int sessionId)
        {
            DataTable dt = new DataTable();
            if (TryConnect())
            {
                try {
                    string sql = "SELECT StudentID, Status, MarkedTime FROM Attendance WHERE SessionID = :sid";
                    OracleCommand cmd = new OracleCommand(sql, conn);
                    cmd.BindByName = true;
                    cmd.Parameters.Add("sid", sessionId);
                    OracleDataAdapter adapter = new OracleDataAdapter(cmd);
                    adapter.Fill(dt);
                } catch (Exception ex) { System.Windows.Forms.MessageBox.Show("Error getting attendance report: " + ex.Message); }
            }
            return dt;
        }

        public string GetStudentName(int id)
        {
            if (TryConnect())
            {
                try {
                    OracleCommand cmd = new OracleCommand("SELECT Name FROM Student WHERE StudentID = :id", conn);
                    cmd.BindByName = true;
                    cmd.Parameters.Add("id", id);
                    object res = cmd.ExecuteScalar();
                    if (res != null) return res.ToString();
                } catch (Exception ex) { System.Windows.Forms.MessageBox.Show("Error getting student name: " + ex.Message); }
            }
            return "Unknown Student";
        }

        public string GetInstructorName(int instructorId)
        {
            if (TryConnect())
            {
                try {
                    OracleCommand cmd = new OracleCommand("SELECT Name FROM Instructor WHERE InstructorID = :id", conn);
                    cmd.BindByName = true;
                    cmd.Parameters.Add("id", instructorId);
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value) return result.ToString()!;
                } catch (Exception ex) { System.Windows.Forms.MessageBox.Show("Error getting instructor name: " + ex.Message); }
            }
            return "Instructor";
        }

        public int GetTotalCourses() => GetCoursesConnected().Count;

        public int GetUpcomingSessions()
        {
            if (TryConnect())
            {
                try {
                    OracleCommand cmd = new OracleCommand("SELECT COUNT(*) FROM courseSession WHERE SessionDate >= CURRENT_DATE", conn);
                    object result = cmd.ExecuteScalar();
                    if (result != null) return Convert.ToInt32(result);
                } catch (Exception ex) { System.Windows.Forms.MessageBox.Show("Error getting upcoming sessions: " + ex.Message); }
            }
            return 0;
        }

        public string GetAttendanceRate()
        {
            if (TryConnect())
            {
                try {
                    int present = 0, total = 0;
                    using (var cmd = new OracleCommand("SELECT COUNT(*) FROM Attendance WHERE TRIM(UPPER(Status)) = 'PRESENT'", conn)) {
                        present = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                    using (var cmd = new OracleCommand("SELECT COUNT(*) FROM Attendance", conn)) {
                        total = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                    if (total == 0) return "0%";
                    return $"{(int)Math.Round((double)present / total * 100)}%";
                } catch (Exception ex) { System.Windows.Forms.MessageBox.Show("Error getting attendance rate: " + ex.Message); }
            }
            return "0%";
        }

        public int GetActiveStudentsCount()
        {
            if (TryConnect())
            {
                try {
                    OracleCommand cmd = new OracleCommand("SELECT COUNT(*) FROM Student", conn);
                    object result = cmd.ExecuteScalar();
                    if (result != null) return Convert.ToInt32(result);
                } catch (Exception ex) { System.Windows.Forms.MessageBox.Show("Error getting active students count: " + ex.Message); }
            }
            return 0;
        }

        public void AddActivity(string activity) 
        {
            PublishNotificationConnected(null, "Activity Log", activity, "Info");
        }

        public List<string> GetRecentActivities()
        {
            List<string> activities = new List<string>();
            if (TryConnect())
            {
                try {
                    OracleCommand cmd = new OracleCommand("SELECT Description, CreatedAt FROM Notification WHERE Title = 'Activity Log' ORDER BY CreatedAt DESC", conn);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                            activities.Add($"{reader.GetDateTime(1):HH:mm} - {reader.GetString(0)}");
                    }
                } catch (Exception ex) { System.Windows.Forms.MessageBox.Show("Error getting recent activities: " + ex.Message); }
            }
            return activities;
        }

        public List<Course> GetStudentCourses(int studentId)
        {
            List<Course> courses = new List<Course>();
            if (TryConnect())
            {
                try {
                    string sql = @"SELECT c.CourseID, c.CourseName FROM Course c 
                                 JOIN Enrollment e ON c.CourseID = e.CourseID 
                                 WHERE e.StudentID = :sid";
                    OracleCommand cmd = new OracleCommand(sql, conn);
                    cmd.BindByName = true;
                    cmd.Parameters.Add("sid", studentId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                            courses.Add(new Course { CourseID = reader.GetInt32(0), CourseName = reader.GetString(1) });
                    }
                } catch (Exception ex) { System.Windows.Forms.MessageBox.Show("Error getting student courses: " + ex.Message); }
            }
            return courses;
        }

        public string GetStudentAttendanceRate(int studentId)
        {
            if (TryConnect())
            {
                try {
                    int present = 0, total = 0;
                    using (var cmd = new OracleCommand("SELECT COUNT(*) FROM Attendance WHERE StudentID = :sid AND UPPER(Status) = 'PRESENT'", conn)) {
                        cmd.BindByName = true;
                        cmd.Parameters.Add("sid", studentId);
                        present = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                    using (var cmd = new OracleCommand("SELECT COUNT(*) FROM Attendance WHERE StudentID = :sid", conn)) {
                        cmd.BindByName = true;
                        cmd.Parameters.Add("sid", studentId);
                        total = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                    if (total == 0) return "0%";
                    return $"{(int)Math.Round((double)present / total * 100)}%";
                } catch (Exception ex) { System.Windows.Forms.MessageBox.Show("Error getting student attendance rate: " + ex.Message); }
            }
            return "0%";
        }

        public List<string> GetStudentNotifications(int studentId)
        {
            List<string> history = new List<string>();
            if (TryConnect())
            {
                try {
                    string sql = "SELECT Title, Description, Type, CreatedAt FROM Notification WHERE (RecipientID = :sid OR RecipientID IS NULL) AND Title <> 'Activity Log' ORDER BY CreatedAt DESC";
                    OracleCommand cmd = new OracleCommand(sql, conn);
                    cmd.BindByName = true;
                    cmd.Parameters.Add("sid", studentId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read()) {
                            string type = reader.GetString(2);
                            string title = reader.GetString(0);
                            string desc = reader.GetString(1);
                            DateTime dt = reader.GetDateTime(3);
                            history.Add($"{dt:yyyy-MM-dd HH:mm:ss}|[{type}] {title}: {desc}");
                        }
                    }
                } catch (Exception ex) { System.Windows.Forms.MessageBox.Show("Error getting student notifications: " + ex.Message); }
            }
            return history;
        }

        public int GetStudentMonthlySessions(int studentId)
        {
            if (TryConnect())
            {
                try {
                    OracleCommand cmd = new OracleCommand("SELECT COUNT(*) FROM Attendance WHERE StudentID = :sid AND SessionID IN (SELECT SessionID FROM courseSession WHERE SessionDate >= SYSDATE - 30)", conn);
                    cmd.BindByName = true;
                    cmd.Parameters.Add("sid", studentId);
                    return Convert.ToInt32(cmd.ExecuteScalar());
                } catch (Exception ex) { System.Windows.Forms.MessageBox.Show("Error getting student monthly sessions: " + ex.Message); }
            }
            return 0;
        }

        public int GetStudentPerfectDays(int studentId)
        {
            if (TryConnect())
            {
                try {
                    string sql = @"
                        SELECT COUNT(*) FROM (
                            SELECT s.SessionDate
                            FROM courseSession s
                            JOIN Enrollment e ON e.CourseID = s.CourseID AND e.StudentID = :sid
                            WHERE EXISTS (
                                SELECT 1 FROM Attendance a1
                                WHERE a1.SessionID = s.SessionID
                                  AND a1.StudentID = :sid_ex
                                  AND UPPER(a1.Status) = 'PRESENT'
                            )
                            AND NOT EXISTS (
                                SELECT 1 FROM Attendance a2
                                WHERE a2.SessionID = s.SessionID
                                  AND a2.StudentID = :sid_nx
                                  AND UPPER(a2.Status) <> 'PRESENT'
                            )
                            GROUP BY s.SessionDate
                        )";
                    OracleCommand cmd = new OracleCommand(sql, conn);
                    cmd.BindByName = true;
                    cmd.Parameters.Add("sid",    studentId);
                    cmd.Parameters.Add("sid_ex", studentId);
                    cmd.Parameters.Add("sid_nx", studentId);
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value) return Convert.ToInt32(result);
                } catch (Exception ex) { System.Windows.Forms.MessageBox.Show("Error getting student perfect days: " + ex.Message); }
            }
            return 0;
        }
        public List<Student> GetAllStudents()
        {
            List<Student> students = new List<Student>();
            if (TryConnect())
            {
                try {
                    OracleCommand cmd = new OracleCommand("SELECT StudentID, Name FROM Student", conn);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                            students.Add(new Student { StudentID = reader.GetInt32(0), Name = reader.GetString(1) });
                    }
                } catch { }
            }
            return students;
        }

        public bool IsStudentEnrolled(int studentId, int courseId)
        {
            if (TryConnect())
            {
                try {
                    OracleCommand cmd = new OracleCommand("SELECT COUNT(*) FROM Enrollment WHERE StudentID = :sid AND CourseID = :cid", conn);
                    cmd.BindByName = true;
                    cmd.Parameters.Add("sid", studentId);
                    cmd.Parameters.Add("cid", courseId);
                    return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                } catch { }
            }
            return false;
        }

        public void EnrollStudent(int studentId, int courseId)
        {
            if (IsStudentEnrolled(studentId, courseId)) return;

            if (TryConnect())
            {
                try {
                    int nextId = 1;
                    using (var cmdId = new OracleCommand("SELECT NVL(MAX(EnrollmentID), 0) + 1 FROM Enrollment", conn)) {
                        nextId = Convert.ToInt32(cmdId.ExecuteScalar());
                    }

                    OracleCommand cmd = new OracleCommand("INSERT INTO Enrollment (EnrollmentID, StudentID, CourseID) VALUES (:eid, :sid, :cid)", conn);
                    cmd.BindByName = true;
                    cmd.Parameters.Add("eid", nextId);
                    cmd.Parameters.Add("sid", studentId);
                    cmd.Parameters.Add("cid", courseId);
                    cmd.ExecuteNonQuery();
                    new OracleCommand("COMMIT", conn).ExecuteNonQuery();
                } catch (Exception ex) {
                    System.Windows.Forms.MessageBox.Show("Error enrolling student: " + ex.Message);
                }
            }
        }
    }
}
