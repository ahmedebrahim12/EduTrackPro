using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;

namespace SWE_Project
{
    public partial class Form2 : Form
    {
        string ordb = "Data source=orcl; User Id=attendance; Password=1234 ";
        OracleConnection conn;
        public Form2()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
           conn= new OracleConnection(ordb);
            textBoxPass.PasswordChar = '*';
            textBoxConPass.PasswordChar = '*';
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBoxName_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBoxEmail_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBoxPass_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBoxConPass_TextChanged(object sender, EventArgs e)
        {

        }

        private void SignUpButton_Click(object sender, EventArgs e)
        {
            if (textBoxName.Text == "" || textBoxEmail.Text == "" || textBoxPass.Text == "" || textBoxConPass.Text == "")
            {
                MessageBox.Show("Please fill all fields");
                return;
            }
            if(textBoxPass.Text != textBoxConPass.Text)
            {
                MessageBox.Show("password and confirm password are not the same");
                textBoxPass.Text = "";
                textBoxConPass.Text = "";
                return;
            }
            conn.Open();

            string checkEmail = "SELECT EMAIL FROM STUDENT WHERE EMAIL = :e";
            OracleCommand cmdCheck = new OracleCommand(checkEmail, conn);
            cmdCheck.Parameters.Add(":e", textBoxEmail.Text);
            OracleDataReader drCheck = cmdCheck.ExecuteReader();

            if (drCheck.Read())
            {
                MessageBox.Show("This email is already registered!");
                textBoxEmail.Text = "";
                textBoxPass.Text = "";
                textBoxConPass.Text = "";
                drCheck.Close();
                conn.Close();
                return;
            }
            drCheck.Close();

            int ID = 1;
            string getQID = "SELECT MAX(STUDENTID) FROM STUDENT";
            OracleCommand cmd = new OracleCommand(getQID, conn);
            OracleDataReader dr = cmd.ExecuteReader();
            if (dr.Read() && !dr.IsDBNull(0))
            {
                ID = Convert.ToInt32(dr[0]) + 1;
            }
            dr.Close();
            
            string q = "INSERT INTO STUDENT (STUDENTID, NAME, EMAIL, STUDENTPASS) VALUES (:id,:n,:e,:p)";
            cmd.CommandText = q;
            cmd.Parameters.Clear();
            cmd.Parameters.Add(":id", ID);
            cmd.Parameters.Add(":n", textBoxName.Text);
            cmd.Parameters.Add(":e", textBoxEmail.Text);
            cmd.Parameters.Add(":p", textBoxPass.Text);
            cmd.ExecuteNonQuery();

            MessageBox.Show("Account Created Successfully");
            Form1 LoginForm = new Form1();
            LoginForm.Show();
            conn.Close();
            this.Close();

        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            Form1 LoginForm = new Form1();
            LoginForm.Show();
            this.Hide();
        }
    }
}
