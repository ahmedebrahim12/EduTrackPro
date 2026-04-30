
namespace attendance_management_system
{
    partial class Main_student
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.side_bar_panal = new System.Windows.Forms.Panel();
            this.Title_panal = new System.Windows.Forms.Panel();
            this.side_bar_panal.SuspendLayout();
            this.SuspendLayout();
            // 
            // side_bar_panal
            // 
            this.side_bar_panal.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.side_bar_panal.Controls.Add(this.Title_panal);
            this.side_bar_panal.Location = new System.Drawing.Point(0, 0);
            this.side_bar_panal.Name = "side_bar_panal";
            this.side_bar_panal.Size = new System.Drawing.Size(212, 452);
            this.side_bar_panal.TabIndex = 0;
            // 
            // Title_panal
            // 
            this.Title_panal.Location = new System.Drawing.Point(0, 0);
            this.Title_panal.Name = "Title_panal";
            this.Title_panal.Size = new System.Drawing.Size(211, 91);
            this.Title_panal.TabIndex = 0;
            this.Title_panal.Paint += new System.Windows.Forms.PaintEventHandler(this.Title_panal_Paint);
            // 
            // Main_student
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.ControlBox = false;
            this.Controls.Add(this.side_bar_panal);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "Main_student";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Text = "Student";
            this.Load += new System.EventHandler(this.Main_student_Load);
            this.side_bar_panal.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel side_bar_panal;
        private System.Windows.Forms.Panel Title_panal;
    }
}

