using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace attendance_management_system
{
    public partial class Main_student : Form
    {
        public static int currentStudentId = 101;
        public static string connectionString = "Data Source=localhost:1521/xepdb1;User Id=SYS;Password=sys;DBA Privilege=SYSDBA;";
        public Main_student()
        {
            InitializeComponent();
        }

        private void Title_panal_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Main_student_Load(object sender, EventArgs e)
        {

        }
    }
}
