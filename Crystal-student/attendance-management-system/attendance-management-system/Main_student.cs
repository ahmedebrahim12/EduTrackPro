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

        public static int currentStudentId = 2;
        public static string connectionString = "User Id=ahmed;Password=2005;Data Source=localhost:1521/FREE;";



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

        private void cr_button_Click(object sender, EventArgs e)
        {

            try
            {
                this.Cursor = Cursors.WaitCursor;
                crystalReportControl1.LoadStudentAttendanceReport(currentStudentId);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
    }
}
