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
                    // Use :p_id to avoid confusion with reserved keywords
                    string sql = @"SELECT 
                                        s.Name AS ""Name"", 
                                        s.Email AS ""Email"", 
                                        c.CourseName AS ""CourseName"", 
                                        cs.SessionDate AS ""SessionDate"", 
                                        cs.StartTime AS ""StartTime"", 
                                        a.Status AS ""Status"", 
                                        a.MarkedTime AS ""MarkedTime""
                                    FROM Student s
                                    INNER JOIN Attendance a ON s.StudentID = a.StudentID
                                    INNER JOIN CourseSession cs ON a.SessionID = cs.SessionID
                                    INNER JOIN Course c ON cs.CourseID = c.CourseID
                                    WHERE s.StudentID = :p_id
                                    ORDER BY cs.SessionDate DESC";

                    OracleCommand cmd = new OracleCommand(sql, conn);

                    // CRITICAL: This ensures the parameter name in C# matches the :p_id in SQL
                    cmd.BindByName = true;
                    cmd.Parameters.Add("p_id", OracleDbType.Int32).Value = studentId;

                    OracleDataAdapter da = new OracleDataAdapter(cmd);
                    DataSet1 ds = new DataSet1();

                    // Match the table name exactly as it appears in your DataSet1.xsd
                    da.Fill(ds, "Attendance");

                    foreach (DataColumn col in ds.Tables["Attendance"].Columns)
                    {
                        // Check if these names match your .xsd and Report field names exactly
                        Console.WriteLine("Column Name: " + col.ColumnName);
                    }

                    if (ds.Tables["Attendance"].Rows.Count > 0)
                    {
                        CrystalReport1 cr = new CrystalReport1();

                        // It is safer to set the source to the whole DataSet 
                        // so Crystal can match the Table Name internally.
                        cr.SetDataSource(ds);

                        crystalReportViewer1.ReportSource = cr;

                        // Use this specific order for refreshing
                        crystalReportViewer1.Show();
                        crystalReportViewer1.RefreshReport();
                    }
                    else
                    {
                        MessageBox.Show($"No data found for Student ID: {studentId}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {
            // Usually kept empty; report loading is triggered by the method above.
        }
    }
}