using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;

namespace attendance_management_system
{
    public partial class CoursesControlForm : UserControl
    {
        public CoursesControlForm()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;
            this.Load += CoursesControlForm_Load;
            this.btnShowAttendance.Click += new System.EventHandler(this.btnShowAttendance_Click);
        }

        private void CoursesControlForm_Load(object sender, EventArgs e)
        {
            LoadEnrolledCourses();
        }
        private void LoadEnrolledCourses()
        {
            try
            {
                using (OracleConnection conn = new OracleConnection(Main_student.connectionString))
                {
                    conn.Open();
                    string query = @"SELECT c.CourseID, c.CourseName 
                                     FROM Course c 
                                     JOIN Enrollment e ON c.CourseID = e.CourseID 
                                     WHERE e.StudentID = :id";

                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    {
                        cmd.BindByName = true;
                        cmd.Parameters.Add(new OracleParameter("id", Main_student.currentStudentId));

                        using (OracleDataReader reader = cmd.ExecuteReader())
                        {
                            DataTable dt = new DataTable();
                            dt.Load(reader);
                            courses_combo.DataSource = dt;
                            courses_combo.DisplayMember = "COURSENAME"; 
                            courses_combo.ValueMember = "COURSEID";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while loading courses: " + ex.Message);
            }
        }

        private void btnShowAttendance_Click(object sender, EventArgs e)
        {
            if (courses_combo.SelectedValue == null)
            {
                MessageBox.Show("Select a course first.", "Notice",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int selectedCourseId = 0;
            try
            {
                selectedCourseId = Convert.ToInt32(courses_combo.SelectedValue);
            }
            catch
            {
                MessageBox.Show("Please select a valid course.");
                return;
            }
            if (selectedCourseId == 0) return;

            try
            {
                using (OracleConnection conn = new OracleConnection(Main_student.connectionString))
                {
                    conn.Open();
                    using (OracleCommand cmd = new OracleCommand("GetCourseAttendance", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("p_StudentID", OracleDbType.Int32).Value = Main_student.currentStudentId;
                        cmd.Parameters.Add("p_CourseID", OracleDbType.Int32).Value = selectedCourseId;
                        cmd.Parameters.Add("p_Recordset", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                        using (OracleDataAdapter da = new OracleDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            if (dt.Columns.Contains("SESSIONDATE")) dt.Columns["SESSIONDATE"].ColumnName = "Date";
                            if (dt.Columns.Contains("STARTTIME")) dt.Columns["STARTTIME"].ColumnName = "Start Time";
                            if (dt.Columns.Contains("ENDTIME")) dt.Columns["ENDTIME"].ColumnName = "End Time";
                            if (dt.Columns.Contains("STATUS")) dt.Columns["STATUS"].ColumnName = "Status";
                            if (dt.Columns.Contains("MARKEDTIME")) dt.Columns["MARKEDTIME"].ColumnName = "Marked At";
                            courses_gridview.DataSource = dt;
                            courses_gridview.ClearSelection();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while loading attendance: " + ex.Message, "Database error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}