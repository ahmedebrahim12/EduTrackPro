using System;
using System.Drawing;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;
using EduTrackPro.Data;

namespace EduTrackPro
{
    public partial class RegisterForm : Form
    {
        OracleConnection conn;

        public RegisterForm()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
        }

        private void RegisterForm_Load(object sender, EventArgs e)
        {
            conn = new OracleConnection(DataService.ordb);
            textBoxPass.PasswordChar    = '*';
            textBoxConPass.PasswordChar = '*';
        }

        // Designer compatibility stubs
        private void label1_Click(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
        private void label3_Click(object sender, EventArgs e) { }
        private void label4_Click(object sender, EventArgs e) { }
        private void textBoxName_TextChanged(object sender, EventArgs e) { }
        private void textBoxEmail_TextChanged(object sender, EventArgs e) { }
        private void textBoxPass_TextChanged(object sender, EventArgs e) { }
        private void textBoxConPass_TextChanged(object sender, EventArgs e) { }

        // Sign Up Action Logic
        private void SignUpButton_Click(object sender, EventArgs e)
        {
            // Validation
            if (string.IsNullOrWhiteSpace(textBoxName.Text)    ||
                string.IsNullOrWhiteSpace(textBoxEmail.Text)   ||
                string.IsNullOrWhiteSpace(textBoxPass.Text)    ||
                string.IsNullOrWhiteSpace(textBoxConPass.Text))
            {
                MessageBox.Show("Please fill all fields.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (textBoxPass.Text != textBoxConPass.Text)
            {
                MessageBox.Show("Passwords do not match.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxPass.Text = textBoxConPass.Text = "";
                textBoxPass.Focus();
                return;
            }

            try
            {
                if (conn.State != System.Data.ConnectionState.Closed) conn.Close();
                conn.Open();

                if (selectedRole == "Student")
                    RegisterStudent();
                else
                    RegisterInstructor();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Database connection error during registration: " + ex.Message,
                    "Registration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RegisterStudent()
        {
            // Check duplicate email
            var chk = new OracleCommand("SELECT COUNT(*) FROM STUDENT WHERE EMAIL=:e", conn);
            chk.Parameters.Add("e", textBoxEmail.Text.Trim());
            if (Convert.ToInt32(chk.ExecuteScalar()) > 0)
            {
                MessageBox.Show("This email is already registered!", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxEmail.Text = "";
                conn.Close();
                return;
            }

            // Get next ID
            var idCmd = new OracleCommand("SELECT NVL(MAX(STUDENTID),0)+1 FROM STUDENT", conn);
            int newId = Convert.ToInt32(idCmd.ExecuteScalar());

            // Insert
            var ins = new OracleCommand(
                "INSERT INTO STUDENT (STUDENTID, NAME, EMAIL, STUDENTPASS) VALUES (:id,:n,:e,:p)", conn);
            ins.Parameters.Add("id", newId);
            ins.Parameters.Add("n",  textBoxName.Text.Trim());
            ins.Parameters.Add("e",  textBoxEmail.Text.Trim());
            ins.Parameters.Add("p",  textBoxPass.Text);
            ins.ExecuteNonQuery();
            conn.Close();

            MessageBox.Show("Student account created successfully! You can now log in.",
                "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            GoToLogin();
        }

        private void RegisterInstructor()
        {
            // Check duplicate email
            var chk = new OracleCommand("SELECT COUNT(*) FROM INSTRUCTOR WHERE EMAIL=:e", conn);
            chk.Parameters.Add("e", textBoxEmail.Text.Trim());
            if (Convert.ToInt32(chk.ExecuteScalar()) > 0)
            {
                MessageBox.Show("This email is already registered!", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxEmail.Text = "";
                conn.Close();
                return;
            }

            // Get next ID
            var idCmd = new OracleCommand("SELECT NVL(MAX(INSTRUCTORID),0)+1 FROM INSTRUCTOR", conn);
            int newId = Convert.ToInt32(idCmd.ExecuteScalar());

            // Insert
            var ins = new OracleCommand(
                "INSERT INTO INSTRUCTOR (INSTRUCTORID, NAME, EMAIL, INSTRUCTORPASS) VALUES (:id,:n,:e,:p)", conn);
            ins.Parameters.Add("id", newId);
            ins.Parameters.Add("n",  textBoxName.Text.Trim());
            ins.Parameters.Add("e",  textBoxEmail.Text.Trim());
            ins.Parameters.Add("p",  textBoxPass.Text);
            ins.ExecuteNonQuery();
            conn.Close();

            MessageBox.Show("Instructor account created successfully! You can now log in.",
                "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            GoToLogin();
        }

        private void GoToLogin()
        {
            LoginForm lf = new LoginForm();
            lf.Show();
            this.Close();
        }

        // Cancel / Back to Login
        private void CancelButton_Click(object sender, EventArgs e) => GoToLogin();
    }
}
