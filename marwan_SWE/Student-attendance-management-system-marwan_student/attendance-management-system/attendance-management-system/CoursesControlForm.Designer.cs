
namespace attendance_management_system
{
    partial class CoursesControlForm
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
            this.courses = new System.Windows.Forms.Label();
            this.courses_combo = new System.Windows.Forms.ComboBox();
            this.courses_gridview = new System.Windows.Forms.DataGridView();
            this.btnShowAttendance = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.courses_gridview)).BeginInit();
            this.SuspendLayout();
            // 
            // courses
            // 
            this.courses.AutoSize = true;
            this.courses.Font = new System.Drawing.Font("Segoe UI", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.courses.Location = new System.Drawing.Point(23, 11);
            this.courses.Name = "courses";
            this.courses.Size = new System.Drawing.Size(201, 40);
            this.courses.TabIndex = 0;
            this.courses.Text = "My Courses : ";
            // 
            // courses_combo
            // 
            this.courses_combo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.courses_combo.FormattingEnabled = true;
            this.courses_combo.Location = new System.Drawing.Point(30, 96);
            this.courses_combo.Name = "courses_combo";
            this.courses_combo.Size = new System.Drawing.Size(625, 21);
            this.courses_combo.TabIndex = 1;
            // 
            // courses_gridview
            // 
            this.courses_gridview.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.courses_gridview.BackgroundColor = System.Drawing.Color.White;
            this.courses_gridview.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.courses_gridview.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.courses_gridview.Location = new System.Drawing.Point(30, 145);
            this.courses_gridview.Name = "courses_gridview";
            this.courses_gridview.RowHeadersVisible = false;
            this.courses_gridview.Size = new System.Drawing.Size(740, 284);
            this.courses_gridview.TabIndex = 2;
            // 
            // btnShowAttendance
            // 
            this.btnShowAttendance.BackColor = System.Drawing.Color.Green;
            this.btnShowAttendance.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShowAttendance.Font = new System.Drawing.Font("Segoe UI", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnShowAttendance.ForeColor = System.Drawing.Color.White;
            this.btnShowAttendance.Location = new System.Drawing.Point(678, 72);
            this.btnShowAttendance.Name = "btnShowAttendance";
            this.btnShowAttendance.Size = new System.Drawing.Size(232, 67);
            this.btnShowAttendance.TabIndex = 3;
            this.btnShowAttendance.Text = "View attendane";
            this.btnShowAttendance.UseVisualStyleBackColor = false;
            // 
            // CoursesControlForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnShowAttendance);
            this.Controls.Add(this.courses_gridview);
            this.Controls.Add(this.courses_combo);
            this.Controls.Add(this.courses);
            this.Name = "CoursesControlForm";
            this.Size = new System.Drawing.Size(913, 482);
            this.Load += new System.EventHandler(this.CoursesControlForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.courses_gridview)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label courses;
        private System.Windows.Forms.ComboBox courses_combo;
        private System.Windows.Forms.DataGridView courses_gridview;
        private System.Windows.Forms.Button btnShowAttendance;
    }
}
