
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
            this.crystalReportControl1 = new attendance_management_system.CrystalReportControl();
            this.cr_button = new System.Windows.Forms.Button();
            this.side_bar_panal.SuspendLayout();
            this.SuspendLayout();
            // 
            // side_bar_panal
            // 
            this.side_bar_panal.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.side_bar_panal.Controls.Add(this.cr_button);
            this.side_bar_panal.Controls.Add(this.Title_panal);
            this.side_bar_panal.Location = new System.Drawing.Point(0, 0);
            this.side_bar_panal.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.side_bar_panal.Name = "side_bar_panal";
            this.side_bar_panal.Size = new System.Drawing.Size(283, 556);
            this.side_bar_panal.TabIndex = 0;
            // 
            // Title_panal
            // 
            this.Title_panal.Location = new System.Drawing.Point(0, 0);
            this.Title_panal.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Title_panal.Name = "Title_panal";
            this.Title_panal.Size = new System.Drawing.Size(281, 112);
            this.Title_panal.TabIndex = 0;
            this.Title_panal.Paint += new System.Windows.Forms.PaintEventHandler(this.Title_panal_Paint);
            // 
            // crystalReportControl1
            // 
            this.crystalReportControl1.Dock = System.Windows.Forms.DockStyle.Right;
            this.crystalReportControl1.Location = new System.Drawing.Point(288, 0);
            this.crystalReportControl1.Name = "crystalReportControl1";
            this.crystalReportControl1.Size = new System.Drawing.Size(779, 554);
            this.crystalReportControl1.TabIndex = 1;
            // 
            // cr_button
            // 
            this.cr_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cr_button.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.cr_button.Location = new System.Drawing.Point(0, 228);
            this.cr_button.Name = "cr_button";
            this.cr_button.Size = new System.Drawing.Size(283, 85);
            this.cr_button.TabIndex = 1;
            this.cr_button.Text = "Crystal Report";
            this.cr_button.UseVisualStyleBackColor = false;
            this.cr_button.Click += new System.EventHandler(this.cr_button_Click);
            // 
            // Main_student
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 554);
            this.ControlBox = false;
            this.Controls.Add(this.crystalReportControl1);
            this.Controls.Add(this.side_bar_panal);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Main_student";
            this.Text = "Student";
            this.side_bar_panal.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel side_bar_panal;
        private System.Windows.Forms.Panel Title_panal;
        private System.Windows.Forms.Button cr_button;
        private CrystalReportControl crystalReportControl1;
    }
}

