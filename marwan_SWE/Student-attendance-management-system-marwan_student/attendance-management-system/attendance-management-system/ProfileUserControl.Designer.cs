
namespace attendance_management_system
{
    partial class ProfileUserControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.profileinfo = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.name_text = new System.Windows.Forms.TextBox();
            this.name = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pass_text = new System.Windows.Forms.TextBox();
            this.password = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.email_text = new System.Windows.Forms.TextBox();
            this.email = new System.Windows.Forms.Label();
            this.button = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // profileinfo
            // 
            this.profileinfo.AutoSize = true;
            this.profileinfo.Font = new System.Drawing.Font("Segoe UI", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.profileinfo.Location = new System.Drawing.Point(30, 9);
            this.profileinfo.Name = "profileinfo";
            this.profileinfo.Size = new System.Drawing.Size(298, 40);
            this.profileinfo.TabIndex = 0;
            this.profileinfo.Text = "Profile Information :";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.name_text);
            this.panel1.Controls.Add(this.name);
            this.panel1.Location = new System.Drawing.Point(17, 76);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(878, 109);
            this.panel1.TabIndex = 1;
            // 
            // name_text
            // 
            this.name_text.BackColor = System.Drawing.Color.Silver;
            this.name_text.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.name_text.Location = new System.Drawing.Point(20, 58);
            this.name_text.Name = "name_text";
            this.name_text.Size = new System.Drawing.Size(580, 13);
            this.name_text.TabIndex = 1;
            // 
            // name
            // 
            this.name.AutoSize = true;
            this.name.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.name.Location = new System.Drawing.Point(15, 11);
            this.name.Name = "name";
            this.name.Size = new System.Drawing.Size(86, 30);
            this.name.TabIndex = 0;
            this.name.Text = "Name : ";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.pass_text);
            this.panel2.Controls.Add(this.password);
            this.panel2.Location = new System.Drawing.Point(17, 202);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(878, 115);
            this.panel2.TabIndex = 2;
           // this.panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.panel2_Paint);
            // 
            // pass_text
            // 
            this.pass_text.BackColor = System.Drawing.Color.Silver;
            this.pass_text.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.pass_text.Location = new System.Drawing.Point(20, 62);
            this.pass_text.Name = "pass_text";
            this.pass_text.Size = new System.Drawing.Size(580, 13);
            this.pass_text.TabIndex = 2;
            // 
            // password
            // 
            this.password.AutoSize = true;
            this.password.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.password.Location = new System.Drawing.Point(15, 13);
            this.password.Name = "password";
            this.password.Size = new System.Drawing.Size(116, 30);
            this.password.TabIndex = 1;
            this.password.Text = "Password : ";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.White;
            this.panel3.Controls.Add(this.email_text);
            this.panel3.Controls.Add(this.email);
            this.panel3.Location = new System.Drawing.Point(17, 344);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(878, 117);
            this.panel3.TabIndex = 2;
            // 
            // email_text
            // 
            this.email_text.BackColor = System.Drawing.Color.Silver;
            this.email_text.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.email_text.Location = new System.Drawing.Point(20, 61);
            this.email_text.Name = "email_text";
            this.email_text.Size = new System.Drawing.Size(580, 13);
            this.email_text.TabIndex = 3;
            // 
            // email
            // 
            this.email.AutoSize = true;
            this.email.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.email.Location = new System.Drawing.Point(15, 10);
            this.email.Name = "email";
            this.email.Size = new System.Drawing.Size(80, 30);
            this.email.TabIndex = 2;
            this.email.Text = "Email : ";
            // 
            // button
            // 
            this.button.BackColor = System.Drawing.Color.Green;
            this.button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button.ForeColor = System.Drawing.Color.White;
            this.button.Location = new System.Drawing.Point(608, 3);
            this.button.Name = "button";
            this.button.Size = new System.Drawing.Size(201, 67);
            this.button.TabIndex = 3;
            this.button.Text = "Save Changes";
            this.button.UseVisualStyleBackColor = false;
            this.button.Click += new System.EventHandler(this.button_Click);
            // 
            // ProfileUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.button);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.profileinfo);
            this.Name = "ProfileUserControl";
            this.Size = new System.Drawing.Size(920, 485);
            this.Load += new System.EventHandler(this.ProfileUserControl_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label profileinfo;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox name_text;
        private System.Windows.Forms.Label name;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox pass_text;
        private System.Windows.Forms.Label password;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TextBox email_text;
        private System.Windows.Forms.Label email;
        private System.Windows.Forms.Button button;
    }
}
