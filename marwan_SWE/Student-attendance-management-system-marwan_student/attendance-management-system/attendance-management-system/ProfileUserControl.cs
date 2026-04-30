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
namespace attendance_management_system
{
    public partial class ProfileUserControl : UserControl
    {
        private OracleDataAdapter adapter;
        private OracleCommandBuilder commandBuilder;
        private DataSet dataSet;
        public ProfileUserControl()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;
            this.Load += ProfileUserControl_Load;
        }

        private void ProfileUserControl_Load(object sender, EventArgs e)
        {
            LoadProfileWithProcedure();
            LoadProfileDisconnected();
        }

        private void LoadProfileWithProcedure()
        {
            try
            {
                using (OracleConnection conn = new OracleConnection(Main_student.connectionString))
                {
                    conn.Open();
                    using (OracleCommand cmd = new OracleCommand("GetStudentProfile", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("p_StudentID", OracleDbType.Int32).Value = Main_student.currentStudentId;
                        OracleParameter pName = new OracleParameter("p_Name", OracleDbType.Varchar2, 100);
                        OracleParameter pEmail = new OracleParameter("p_Email", OracleDbType.Varchar2, 100);
                        OracleParameter pPass = new OracleParameter("p_Pass", OracleDbType.Varchar2, 100);
                        OracleParameter pTotalCourses = new OracleParameter("p_TotalCourses", OracleDbType.Int32);
                        pName.Direction = ParameterDirection.Output;
                        pEmail.Direction = ParameterDirection.Output;
                        pPass.Direction = ParameterDirection.Output;
                        pTotalCourses.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(pName);
                        cmd.Parameters.Add(pEmail);
                        cmd.Parameters.Add(pPass);
                        cmd.Parameters.Add(pTotalCourses);
                        cmd.ExecuteNonQuery();
                        name_text.Text = pName.Value != DBNull.Value ? pName.Value.ToString() : "";
                        email_text.Text = pEmail.Value != DBNull.Value ? pEmail.Value.ToString() : "";
                        pass_text.Text = pPass.Value != DBNull.Value ? pPass.Value.ToString() : "";
                        name_text.ReadOnly = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading profile: " + ex.Message);
            }
        }

        private void LoadProfileDisconnected()
        {
            try
            {
                OracleConnection conn = new OracleConnection(Main_student.connectionString);
                string query = "SELECT StudentID, Name, Email, studentPass FROM Student WHERE StudentID = :id";
                adapter = new OracleDataAdapter(query, conn);
                adapter.SelectCommand.Parameters.Add(new OracleParameter("id", Main_student.currentStudentId));
                commandBuilder = new OracleCommandBuilder(adapter);
                dataSet = new DataSet();
                adapter.Fill(dataSet, "StudentProfile");
                dataSet.Tables["StudentProfile"].PrimaryKey = new DataColumn[]
                {
                    dataSet.Tables["StudentProfile"].Columns["STUDENTID"]
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading disconnected: " + ex.Message);
            }
        }

        private void button_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataSet != null && dataSet.Tables["StudentProfile"].Rows.Count > 0)
                {
                    DataRow row = dataSet.Tables["StudentProfile"].Rows[0];
                    row["EMAIL"] = email_text.Text;
                    row["STUDENTPASS"] = pass_text.Text;
                    adapter.Update(dataSet, "StudentProfile");
                    MessageBox.Show("Profile updated ", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while saving: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}