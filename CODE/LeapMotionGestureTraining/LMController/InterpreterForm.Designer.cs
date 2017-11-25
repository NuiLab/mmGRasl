namespace LeapMotionGestureTraining.LMController
{
    partial class InterpreterForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing && (components != null))
        //    {
        //        components.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.leapMotionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.trainingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.interpreterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnCapture = new System.Windows.Forms.Button();
            this.txbDebugConsole = new System.Windows.Forms.TextBox();
            this.lbCaptureText = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.leapMotionToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(12, 4, 0, 4);
            this.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.menuStrip1.Size = new System.Drawing.Size(2418, 44);
            this.menuStrip1.TabIndex = 53;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // leapMotionToolStripMenuItem
            // 
            this.leapMotionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.trainingToolStripMenuItem,
            this.testingToolStripMenuItem,
            this.interpreterToolStripMenuItem});
            this.leapMotionToolStripMenuItem.Name = "leapMotionToolStripMenuItem";
            this.leapMotionToolStripMenuItem.Size = new System.Drawing.Size(90, 36);
            this.leapMotionToolStripMenuItem.Text = "Menu";
            // 
            // trainingToolStripMenuItem
            // 
            this.trainingToolStripMenuItem.Name = "trainingToolStripMenuItem";
            this.trainingToolStripMenuItem.Size = new System.Drawing.Size(268, 38);
            this.trainingToolStripMenuItem.Text = "Training";
            this.trainingToolStripMenuItem.Click += new System.EventHandler(this.trainingToolStripMenuItem_Click);
            // 
            // testingToolStripMenuItem
            // 
            this.testingToolStripMenuItem.Name = "testingToolStripMenuItem";
            this.testingToolStripMenuItem.Size = new System.Drawing.Size(268, 38);
            this.testingToolStripMenuItem.Text = "Testing";
            this.testingToolStripMenuItem.Click += new System.EventHandler(this.testingToolStripMenuItem_Click);
            // 
            // interpreterToolStripMenuItem
            // 
            this.interpreterToolStripMenuItem.Name = "interpreterToolStripMenuItem";
            this.interpreterToolStripMenuItem.Size = new System.Drawing.Size(268, 38);
            this.interpreterToolStripMenuItem.Text = "Interpreter";
            // 
            // btnCapture
            // 
            this.btnCapture.Location = new System.Drawing.Point(109, 595);
            this.btnCapture.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.btnCapture.Name = "btnCapture";
            this.btnCapture.Size = new System.Drawing.Size(150, 44);
            this.btnCapture.TabIndex = 59;
            this.btnCapture.Text = "Interpret";
            this.btnCapture.UseVisualStyleBackColor = true;
            this.btnCapture.Click += new System.EventHandler(this.btnCapture_Click);
            // 
            // txbDebugConsole
            // 
            this.txbDebugConsole.BackColor = System.Drawing.SystemColors.MenuText;
            this.txbDebugConsole.ForeColor = System.Drawing.SystemColors.Window;
            this.txbDebugConsole.Location = new System.Drawing.Point(713, 602);
            this.txbDebugConsole.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txbDebugConsole.Multiline = true;
            this.txbDebugConsole.Name = "txbDebugConsole";
            this.txbDebugConsole.ReadOnly = true;
            this.txbDebugConsole.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txbDebugConsole.Size = new System.Drawing.Size(1690, 666);
            this.txbDebugConsole.TabIndex = 57;
            this.txbDebugConsole.Text = "Timestamp:             \r\nFrames Per Second:\r\nDisplay is Valid:\r\nHand Count :\r\nGes" +
    "ture Count :\r\n\r\n\r\n\r\n";
            // 
            // lbCaptureText
            // 
            this.lbCaptureText.AutoSize = true;
            this.lbCaptureText.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbCaptureText.Location = new System.Drawing.Point(15, 681);
            this.lbCaptureText.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lbCaptureText.Name = "lbCaptureText";
            this.lbCaptureText.Size = new System.Drawing.Size(368, 44);
            this.lbCaptureText.TabIndex = 56;
            this.lbCaptureText.Text = "Gesture Detected : ";
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.SystemColors.ControlDark;
            this.pictureBox2.Location = new System.Drawing.Point(1603, 76);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(800, 481);
            this.pictureBox2.TabIndex = 55;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.pictureBox1.Location = new System.Drawing.Point(713, 76);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(800, 481);
            this.pictureBox1.TabIndex = 54;
            this.pictureBox1.TabStop = false;
            // 
            // InterpreterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(2418, 1302);
            this.Controls.Add(this.btnCapture);
            this.Controls.Add(this.txbDebugConsole);
            this.Controls.Add(this.lbCaptureText);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.menuStrip1);
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Name = "InterpreterForm";
            this.Text = "Leap Motion Gesture Interpreter";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem leapMotionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem trainingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem testingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem interpreterToolStripMenuItem;
        private System.Windows.Forms.Button btnCapture;
        private System.Windows.Forms.TextBox txbDebugConsole;
        private System.Windows.Forms.Label lbCaptureText;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}