using System;
using System.Data;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client; 
using CrystalDecisions.CrystalReports.Engine;
using EduTrackPro.Data; // Added for DataService.ordb

namespace EduTrackPro.StudentPortal.Controls
{
    public partial class CrystalReportControl : UserControl
    {
        private string connectionString = DataService.ordb;

        public CrystalReportControl()
        {
            InitializeComponent();
        }


        public void LoadStudentAttendanceReport(int studentId)
        {
            if (this.DesignMode) return;

            try
            {
                using (OracleConnection conn = new OracleConnection(connectionString))
                {
                    DataSet1 ds = new DataSet1();
                    
                    // 1. Fill Student Table
                    string sqlStudent = "SELECT * FROM Student WHERE StudentID = :p_id";
                    using(OracleDataAdapter da = new OracleDataAdapter(sqlStudent, conn)) {
                        da.SelectCommand.Parameters.Add("p_id", OracleDbType.Int32).Value = studentId;
                        da.Fill(ds, "Student");
                    }

                    // 2. Fill Attendance Table
                    string sqlAttendance = "SELECT * FROM Attendance WHERE StudentID = :p_id";
                    using(OracleDataAdapter da = new OracleDataAdapter(sqlAttendance, conn)) {
                        da.SelectCommand.Parameters.Add("p_id", OracleDbType.Int32).Value = studentId;
                        da.Fill(ds, "Attendance");
                    }

                    // 3. Fill CourseSession Table
                    string sqlSession = "SELECT * FROM CourseSession WHERE SessionID IN (SELECT SessionID FROM Attendance WHERE StudentID = :p_id)";
                    using(OracleDataAdapter da = new OracleDataAdapter(sqlSession, conn)) {
                        da.SelectCommand.Parameters.Add("p_id", OracleDbType.Int32).Value = studentId;
                        da.Fill(ds, "CourseSession");
                    }

                    // 4. Fill Course Table
                    string sqlCourse = "SELECT * FROM Course WHERE CourseID IN (SELECT CourseID FROM CourseSession WHERE SessionID IN (SELECT SessionID FROM Attendance WHERE StudentID = :p_id))";
                    using(OracleDataAdapter da = new OracleDataAdapter(sqlCourse, conn)) {
                        da.SelectCommand.Parameters.Add("p_id", OracleDbType.Int32).Value = studentId;
                        da.Fill(ds, "Course");
                    }

                    // Data found!
                    int rowCount = ds.Tables["Attendance"].Rows.Count;

                    if (rowCount > 0)
                    {
                        CrystalReport1 cr = new CrystalReport1();
                        cr.SetDataSource(ds);

                        var emptyConnInfo = new CrystalDecisions.Shared.ConnectionInfo
                        {
                            AllowCustomConnection = true,
                            IntegratedSecurity = false,
                            ServerName = "",
                            DatabaseName = "",
                            UserID = "",
                            Password = ""
                        };

                        var tableLogOnInfos = new CrystalDecisions.Shared.TableLogOnInfos();
                        foreach (CrystalDecisions.CrystalReports.Engine.Table table in cr.Database.Tables)
                        {
                            var logOnInfo = table.LogOnInfo;
                            logOnInfo.ConnectionInfo = emptyConnInfo;
                            table.ApplyLogOnInfo(logOnInfo);
                            tableLogOnInfos.Add(logOnInfo);
                        }

                        // Apply to viewer
                        crystalReportViewer1.LogOnInfo = tableLogOnInfos;
                        crystalReportViewer1.ReportSource = cr;
                    }
                    else
                    {
                        MessageBox.Show($"No data found in Oracle for Student ID: {studentId}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Error: " + ex.Message);
            }
        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {
            // Usually kept empty; report loading is triggered by the method above.
        }
    }
}
