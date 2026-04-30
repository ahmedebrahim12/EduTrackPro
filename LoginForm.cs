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

using EduTrackPro.Data;
using EduTrackPro.StudentPortal;

namespace EduTrackPro
{
    public partial class LoginForm : Form
    {
        OracleConnection conn;
        public LoginForm()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            conn = new OracleConnection(DataService.ordb);
            textBoxPass.PasswordChar = '*';
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBoxPass_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBoxPass.Text))
            {
                MessageBox.Show("Please fill all fields");
                return;
            }

            try
            {
                // Ensure connection is closed before opening
                if (conn.State != System.Data.ConnectionState.Closed)
                    conn.Close();
                conn.Open();

                // Check Student Login
                string q1 = "SELECT STUDENTID FROM STUDENT WHERE EMAIL=:e AND STUDENTPASS=:p";
                OracleCommand cmd = new OracleCommand(q1, conn);
                cmd.Parameters.Add("e", textBox1.Text.Trim());
                cmd.Parameters.Add("p", textBoxPass.Text.Trim());

                OracleDataReader dr1 = cmd.ExecuteReader();
                if (dr1.Read())
                {
                    int studentId = Convert.ToInt32(dr1["STUDENTID"]);
                    dr1.Close();
                    conn.Close();

                    Main_student studentForm = new Main_student(studentId);
                    studentForm.FormClosed += (s, args) => this.Show();
                    studentForm.Show();
                    this.Hide();
                    return;
                }
                dr1.Close();

                // Check Instructor Login
                string q2 = "SELECT INSTRUCTORID FROM INSTRUCTOR WHERE EMAIL=:e AND INSTRUCTORPASS=:p";
                OracleCommand cmd2 = new OracleCommand(q2, conn);
                cmd2.Parameters.Add("e", textBox1.Text.Trim());
                cmd2.Parameters.Add("p", textBoxPass.Text.Trim());

                OracleDataReader dr2 = cmd2.ExecuteReader();
                if (dr2.Read())
                {
                    int instructorId = Convert.ToInt32(dr2["INSTRUCTORID"]);
                    dr2.Close();
                    conn.Close();

                    MainForm instructorForm = new MainForm(instructorId);
                    instructorForm.FormClosed += (s, args) => this.Show();
                    instructorForm.Show();
                    this.Hide();
                }
                else
                {
                    dr2.Close();
                    conn.Close();
                    MessageBox.Show("Wrong Email or Password. Please try again.", "Login Failed",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBoxPass.Text = "";
                    textBoxPass.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Database connection error: " + ex.Message + "\n\n" +
                    "Please ensure your Oracle Database is running and accessible.",
                    "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            RegisterForm SignUpForm = new RegisterForm();
            SignUpForm.Show();
            this.Hide();
        }
        
    }
}
