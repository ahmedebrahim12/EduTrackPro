using System;
using System.Data;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;
using CrystalDecisions.CrystalReports.Engine;
using EduTrackPro.Data;

namespace EduTrackPro.InstructorReport
{
    public partial class Inst_CrystalReport : UserControl
    {
        private string connectionString = DataService.ordb;

        public Inst_CrystalReport()
        {
            InitializeComponent();
        }

        public void LoadInstructorAttendanceReport(int courseId, DateTime startDate, DateTime endDate)
        {
            if (this.DesignMode) return;

            try
            {
                using (OracleConnection conn = new OracleConnection(connectionString))
                {
                    EduTrackPro.StudentPortal.Controls.DataSet1 ds = new EduTrackPro.StudentPortal.Controls.DataSet1();

                    string sqlCourse = "SELECT DISTINCT * FROM Course WHERE CourseID = :p_id";
                    using (OracleDataAdapter da = new OracleDataAdapter(sqlCourse, conn))
                    {
                        da.SelectCommand.Parameters.Add("p_id", OracleDbType.Int32).Value = courseId;
                        da.Fill(ds, "Course");
                    }

                    string sqlSession = @"SELECT DISTINCT SessionID, CourseID, SessionDate, StartTime, EndTime 
                                         FROM CourseSession 
                                         WHERE CourseID = :p_id 
                                         AND SessionDate BETWEEN :p_start AND :p_end";
                    using (OracleDataAdapter da = new OracleDataAdapter(sqlSession, conn))
                    {
                        da.SelectCommand.Parameters.Add("p_id", OracleDbType.Int32).Value = courseId;
                        da.SelectCommand.Parameters.Add("p_start", OracleDbType.Date).Value = startDate.Date;
                        da.SelectCommand.Parameters.Add("p_end", OracleDbType.Date).Value = endDate.Date;
                        da.Fill(ds, "CourseSession");
                    }

                    string sqlStudent = @"SELECT DISTINCT StudentID, Name, Email FROM Student 
                                         WHERE StudentID IN (SELECT StudentID FROM Enrollment WHERE CourseID = :p_id)";
                    using (OracleDataAdapter da = new OracleDataAdapter(sqlStudent, conn))
                    {
                        da.SelectCommand.Parameters.Add("p_id", OracleDbType.Int32).Value = courseId;
                        da.Fill(ds, "Student");
                    }

                    string sqlAttendance = @"SELECT MAX(AttendanceID) as AttendanceID, StudentID, SessionID, MAX(Status) as Status, MAX(MarkedTime) as MarkedTime 
                                             FROM Attendance 
                                             WHERE SessionID IN (
                                                SELECT SessionID FROM CourseSession 
                                                WHERE CourseID = :p_id 
                                                AND SessionDate BETWEEN :p_start AND :p_end
                                             )
                                             GROUP BY StudentID, SessionID";
                    using (OracleDataAdapter da = new OracleDataAdapter(sqlAttendance, conn))
                    {
                        da.SelectCommand.Parameters.Add("p_id", OracleDbType.Int32).Value = courseId;
                        da.SelectCommand.Parameters.Add("p_start", OracleDbType.Date).Value = startDate.Date;
                        da.SelectCommand.Parameters.Add("p_end", OracleDbType.Date).Value = endDate.Date;
                        da.Fill(ds, "Attendance");
                    }

                    // Manually define relations in the DataSet to force Crystal to link correctly
                    try
                    {
                        if (ds.Tables.Contains("CourseSession") && ds.Tables.Contains("Attendance"))
                            ds.Relations.Add("Session_Attendance", ds.Tables["CourseSession"].Columns["SessionID"], ds.Tables["Attendance"].Columns["SessionID"]);
                        
                        if (ds.Tables.Contains("Student") && ds.Tables.Contains("Attendance"))
                            ds.Relations.Add("Student_Attendance", ds.Tables["Student"].Columns["StudentID"], ds.Tables["Attendance"].Columns["StudentID"]);
                    }
                    catch { /* Relation might already exist in typed dataset */ }

                    int rowCount = ds.Tables["Attendance"] != null ? ds.Tables["Attendance"].Rows.Count : 0;

                    if (rowCount > 0)
                    {
                        CrystalReport_Inst cr = new CrystalReport_Inst();
                        
                        // 1. Set the main data source
                        cr.SetDataSource(ds);

                        // 4. Bind to viewer
                        crystalReportViewer1.ReportSource = cr;

                        // 5. THE FIX: Force the missing links at the viewer level to stop duplication
                        // This tells Crystal exactly how to join the tables in memory
                        crystalReportViewer1.SelectionFormula = "{Attendance.SessionID} = {CourseSession.SessionID} AND {Attendance.StudentID} = {Student.StudentID}";
                        
                        crystalReportViewer1.RefreshReport();
                    }
                    else
                    {
                        MessageBox.Show($"No attendance data found for Course ID: {courseId}", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Report Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

