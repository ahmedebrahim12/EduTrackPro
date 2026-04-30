using System;
using System.Windows.Forms;
using System.Drawing;
using Oracle.ManagedDataAccess.Client;
using EduTrackPro.Components;
using EduTrackPro.Data;

namespace EduTrackPro
{
    public class SettingsView : BaseView
    {
        private TextBox txtHost;
        private TextBox txtPort;
        private TextBox txtService;
        private TextBox txtUser;
        private TextBox txtPass;
        private Label lblStatus;

        public SettingsView() : base("Database Configuration", "Connect the application to your local Oracle instance.")
        {
            SetupSettingsForm();
        }

        private void SetupSettingsForm()
        {
            var card = new CustomCard { Location = new Point(0, 80), Size = new Size(500, 600), Padding = new Padding(20) };
            var lblTitle = new Label { Text = "Connection Details", Font = Theme.SubHeaderFont, Location = new Point(20, 20), AutoSize = true };
            
            int y = 70;
            txtHost = CreateField(card, "Host (e.g. localhost):", "localhost", ref y);
            txtPort = CreateField(card, "Port (e.g. 1521):", "1521", ref y);
            txtService = CreateField(card, "Service Name / SID (e.g. xe):", "xe", ref y);
            txtUser = CreateField(card, "Database User:", "hr", ref y);
            txtPass = CreateField(card, "Password:", "hr", ref y, true);

            lblStatus = new Label { Text = "Status: Not Tested", Font = Theme.SmallFont, ForeColor = Color.Gray, Location = new Point(20, y), AutoSize = true };

            var btnTest = new CustomButton { Text = "Test & Save Connection", Location = new Point(20, y + 30), Size = new Size(250, 45) };
            btnTest.Click += (s, e) => TestAndSave();

            card.Controls.AddRange(new Control[] { lblTitle, lblStatus, btnTest });
            this.Controls.Add(card);
        }

        private TextBox CreateField(Control parent, string label, string def, ref int y, bool isPass = false)
        {
            var lbl = new Label { Text = label, Font = Theme.SmallFont, ForeColor = Theme.TextLight, Location = new Point(20, y), AutoSize = true };
            var txt = new TextBox { Text = def, Location = new Point(20, y + 20), Width = 440, Font = Theme.BodyFont, PasswordChar = isPass ? '•' : '\0' };
            parent.Controls.AddRange(new Control[] { lbl, txt });
            y += 70;
            return txt;
        }

        private void TestAndSave()
        {
            string connStr = $"Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={txtHost.Text})(PORT={txtPort.Text}))(CONNECT_DATA=(SERVICE_NAME={txtService.Text})));User Id={txtUser.Text};Password={txtPass.Text};";
            
            lblStatus.Text = "Status: Testing...";
            lblStatus.ForeColor = Color.Blue;

            try
            {
                using (OracleConnection conn = new OracleConnection(connStr))
                {
                    conn.Open();
                    DataService.ordb = connStr;
                    DataService.Instance.ResetConnection(); // Update global connection
                    lblStatus.Text = "Status: Connected Successfully!";
                    lblStatus.ForeColor = Color.Green;
                    MessageBox.Show("Connection Successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Status: Connection Failed";
                lblStatus.ForeColor = Color.Red;
                MessageBox.Show("Failed to connect to Oracle.\n\nError: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
