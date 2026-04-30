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
                        
                        // 1. Pass the whole dataset (this is more reliable)
                        cr.SetDataSource(ds);

                        // 2. Clear all connection info
                        foreach (CrystalDecisions.CrystalReports.Engine.Table table in cr.Database.Tables)
                        {
                            CrystalDecisions.Shared.TableLogOnInfo logOnInfo = table.LogOnInfo;
                            logOnInfo.ConnectionInfo.ServerName = ""; 
                            logOnInfo.ConnectionInfo.DatabaseName = "";
                            logOnInfo.ConnectionInfo.UserID = "";
                            logOnInfo.ConnectionInfo.Password = "";
                            table.ApplyLogOnInfo(logOnInfo);
                        }

                        // 3. Force empty credentials
                        cr.SetDatabaseLogon("", "");

                        // 4. If the report has a parameter named "p_id" or "studentId", fill it
                        foreach (ParameterFieldDefinition def in cr.DataDefinition.ParameterFields)
                        {
                            if (def.Name.ToLower().Contains("id"))
                            {
                                cr.SetParameterValue(def.Name, studentId);
                            }
                        }
                        
                        crystalReportViewer1.ReportSource = cr;
                        crystalReportViewer1.RefreshReport();
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
