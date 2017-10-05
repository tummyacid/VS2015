namespace VASImportFollettDatabase
{
    partial class frmMain
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
            this.btnImportXls = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnImportXls
            // 
            this.btnImportXls.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImportXls.Location = new System.Drawing.Point(12, 12);
            this.btnImportXls.Name = "btnImportXls";
            this.btnImportXls.Size = new System.Drawing.Size(175, 55);
            this.btnImportXls.TabIndex = 0;
            this.btnImportXls.Text = "Import XLS";
            this.btnImportXls.UseVisualStyleBackColor = true;
            this.btnImportXls.Click += new System.EventHandler(this.btnImportXls_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(199, 79);
            this.Controls.Add(this.btnImportXls);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmMain";
            this.Text = "Follett Order Import";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnImportXls;
    }
}

