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
    public partial class Form1 : Form
    {
        string ordb = "Data source=orcl; User Id=attendance; Password=1234 ";
        OracleConnection conn;
        public Form1()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            conn=new OracleConnection(ordb);
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
            if (textBox1.Text == "" || textBoxPass.Text == "")
            {
                MessageBox.Show("Please fill all fields");
                return;
            }
            conn.Open();
            // If Student Login
            string q1 = "SELECT STUDENTID FROM STUDENT WHERE EMAIL=:e AND STUDENTPASS=:p";

            OracleCommand cmd = new OracleCommand(q1, conn);
            cmd.Parameters.Add(":e", textBox1.Text);
            cmd.Parameters.Add(":p", textBoxPass.Text);

            OracleDataReader dr1 = cmd.ExecuteReader();

            if (dr1.Read())
            {
                MessageBox.Show("Login Success");
                /*
                       Student Form Object here
                 
                 */
                this.Hide();
                dr1.Close();
                conn.Close();
                return;
            }
            dr1.Close();

            // If Instructor LOgin
            string q2 = "SELECT INSTRUCTORID FROM INSTRUCTOR WHERE EMAIL=:e AND INSTRUCTORPASS=:p";
            OracleCommand cmd2 = new OracleCommand(q2, conn);
            cmd2.Parameters.Add(":e", textBox1.Text);
            cmd2.Parameters.Add(":p", textBoxPass.Text);

            OracleDataReader dr2 = cmd2.ExecuteReader();

            if (dr2.Read())
            {
                MessageBox.Show("Login Success");
                /* 
                    Instructor Form Object here
                 */
                this.Hide();
            }
            else
            {
                MessageBox.Show("Wrong Email or Password");
                textBox1.Text = "";
                textBoxPass.Text = "";
            }

            dr2.Close();
            conn.Close();

        }
        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            Form2 SignUpForm = new Form2();
            SignUpForm.Show();
            this.Hide();
        }
        
    }
}
