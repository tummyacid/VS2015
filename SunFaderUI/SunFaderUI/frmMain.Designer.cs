namespace SunFaderUI
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
            this.btnSend = new System.Windows.Forms.Button();
            this.nudMax = new System.Windows.Forms.NumericUpDown();
            this.nudStep = new System.Windows.Forms.NumericUpDown();
            this.nudDelay = new System.Windows.Forms.NumericUpDown();
            this.grpPin = new System.Windows.Forms.GroupBox();
            this.radWarm = new System.Windows.Forms.RadioButton();
            this.radCold = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.trkIntensity = new System.Windows.Forms.TrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.nudMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudStep)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDelay)).BeginInit();
            this.grpPin.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trkIntensity)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(422, 83);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(56, 23);
            this.btnSend.TabIndex = 0;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Visible = false;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // nudMax
            // 
            this.nudMax.Location = new System.Drawing.Point(313, 57);
            this.nudMax.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudMax.Name = "nudMax";
            this.nudMax.Size = new System.Drawing.Size(56, 20);
            this.nudMax.TabIndex = 2;
            this.nudMax.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudMax.Visible = false;
            // 
            // nudStep
            // 
            this.nudStep.Location = new System.Drawing.Point(186, 57);
            this.nudStep.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudStep.Name = "nudStep";
            this.nudStep.Size = new System.Drawing.Size(56, 20);
            this.nudStep.TabIndex = 3;
            this.nudStep.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudStep.Visible = false;
            // 
            // nudDelay
            // 
            this.nudDelay.Location = new System.Drawing.Point(422, 57);
            this.nudDelay.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudDelay.Name = "nudDelay";
            this.nudDelay.Size = new System.Drawing.Size(56, 20);
            this.nudDelay.TabIndex = 4;
            this.nudDelay.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // grpPin
            // 
            this.grpPin.Controls.Add(this.radCold);
            this.grpPin.Controls.Add(this.radWarm);
            this.grpPin.Location = new System.Drawing.Point(13, 13);
            this.grpPin.Name = "grpPin";
            this.grpPin.Size = new System.Drawing.Size(111, 74);
            this.grpPin.TabIndex = 5;
            this.grpPin.TabStop = false;
            this.grpPin.Text = "Pin";
            // 
            // radWarm
            // 
            this.radWarm.AutoSize = true;
            this.radWarm.Checked = true;
            this.radWarm.Location = new System.Drawing.Point(23, 19);
            this.radWarm.Name = "radWarm";
            this.radWarm.Size = new System.Drawing.Size(53, 17);
            this.radWarm.TabIndex = 0;
            this.radWarm.TabStop = true;
            this.radWarm.Text = "Warm";
            this.radWarm.UseVisualStyleBackColor = true;
            // 
            // radCold
            // 
            this.radCold.AutoSize = true;
            this.radCold.Location = new System.Drawing.Point(23, 42);
            this.radCold.Name = "radCold";
            this.radCold.Size = new System.Drawing.Size(46, 17);
            this.radCold.TabIndex = 1;
            this.radCold.Text = "Cold";
            this.radCold.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(261, 57);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Intensity";
            this.label1.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(151, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Step";
            this.label2.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(382, 57);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Delay";
            // 
            // trkIntensity
            // 
            this.trkIntensity.Location = new System.Drawing.Point(130, 13);
            this.trkIntensity.Maximum = 255;
            this.trkIntensity.Name = "trkIntensity";
            this.trkIntensity.Size = new System.Drawing.Size(348, 45);
            this.trkIntensity.TabIndex = 9;
            this.trkIntensity.Scroll += new System.EventHandler(this.trkIntensity_Scroll);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(490, 114);
            this.ControlBox = false;
            this.Controls.Add(this.trkIntensity);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.grpPin);
            this.Controls.Add(this.nudDelay);
            this.Controls.Add(this.nudStep);
            this.Controls.Add(this.nudMax);
            this.Controls.Add(this.btnSend);
            this.Name = "frmMain";
            this.Text = "Desktop Fader";
            ((System.ComponentModel.ISupportInitialize)(this.nudMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudStep)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDelay)).EndInit();
            this.grpPin.ResumeLayout(false);
            this.grpPin.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trkIntensity)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.NumericUpDown nudMax;
        private System.Windows.Forms.NumericUpDown nudStep;
        private System.Windows.Forms.NumericUpDown nudDelay;
        private System.Windows.Forms.GroupBox grpPin;
        private System.Windows.Forms.RadioButton radCold;
        private System.Windows.Forms.RadioButton radWarm;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TrackBar trkIntensity;
    }
}

