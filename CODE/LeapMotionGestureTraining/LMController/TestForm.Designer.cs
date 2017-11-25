namespace LeapMotionGestureTraining.LMController
{
    partial class TestForm
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
            this.txbDebugConsole = new System.Windows.Forms.TextBox();
            this.lbCaptureText = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnRemoveAllTestData = new System.Windows.Forms.Button();
            this.btnTest = new System.Windows.Forms.Button();
            this.btnCapture = new System.Windows.Forms.Button();
            this.trvTestData = new System.Windows.Forms.TreeView();
            this.lbListHumanSign = new System.Windows.Forms.Label();
            this.txbTrainResult = new System.Windows.Forms.TextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.leapMotionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.trainingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.interpreterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txbDebugConsole
            // 
            this.txbDebugConsole.BackColor = System.Drawing.SystemColors.MenuText;
            this.txbDebugConsole.ForeColor = System.Drawing.SystemColors.Window;
            this.txbDebugConsole.Location = new System.Drawing.Point(586, 613);
            this.txbDebugConsole.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txbDebugConsole.Multiline = true;
            this.txbDebugConsole.Name = "txbDebugConsole";
            this.txbDebugConsole.ReadOnly = true;
            this.txbDebugConsole.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txbDebugConsole.Size = new System.Drawing.Size(1680, 594);
            this.txbDebugConsole.TabIndex = 43;
            this.txbDebugConsole.Text = "Timestamp:             \r\nFrames Per Second:\r\nDisplay is Valid:\r\nHand Count :\r\nGes" +
    "ture Count :\r\n\r\n\r\n\r\n";
            // 
            // lbCaptureText
            // 
            this.lbCaptureText.AutoSize = true;
            this.lbCaptureText.Location = new System.Drawing.Point(1362, 567);
            this.lbCaptureText.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lbCaptureText.Name = "lbCaptureText";
            this.lbCaptureText.Size = new System.Drawing.Size(123, 25);
            this.lbCaptureText.TabIndex = 42;
            this.lbCaptureText.Text = "Capure in 3";
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.SystemColors.ControlDark;
            this.pictureBox2.Location = new System.Drawing.Point(1464, 81);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(800, 481);
            this.pictureBox2.TabIndex = 41;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.pictureBox1.Location = new System.Drawing.Point(586, 81);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(800, 481);
            this.pictureBox1.TabIndex = 40;
            this.pictureBox1.TabStop = false;
            // 
            // btnRemoveAllTestData
            // 
            this.btnRemoveAllTestData.Location = new System.Drawing.Point(24, 813);
            this.btnRemoveAllTestData.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.btnRemoveAllTestData.Name = "btnRemoveAllTestData";
            this.btnRemoveAllTestData.Size = new System.Drawing.Size(278, 44);
            this.btnRemoveAllTestData.TabIndex = 44;
            this.btnRemoveAllTestData.Text = "Remove all test data";
            this.btnRemoveAllTestData.UseVisualStyleBackColor = true;
            this.btnRemoveAllTestData.Click += new System.EventHandler(this.btnRemoveAllTestData_Click);
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(24, 669);
            this.btnTest.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(150, 44);
            this.btnTest.TabIndex = 45;
            this.btnTest.Text = "Test";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // btnCapture
            // 
            this.btnCapture.Location = new System.Drawing.Point(24, 742);
            this.btnCapture.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.btnCapture.Name = "btnCapture";
            this.btnCapture.Size = new System.Drawing.Size(150, 44);
            this.btnCapture.TabIndex = 46;
            this.btnCapture.Text = "Capture";
            this.btnCapture.UseVisualStyleBackColor = true;
            this.btnCapture.Click += new System.EventHandler(this.btnCapture_Click);
            // 
            // trvTestData
            // 
            this.trvTestData.Location = new System.Drawing.Point(24, 112);
            this.trvTestData.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.trvTestData.Name = "trvTestData";
            this.trvTestData.ShowPlusMinus = false;
            this.trvTestData.Size = new System.Drawing.Size(520, 506);
            this.trvTestData.TabIndex = 49;
            this.trvTestData.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeViewMouseDoubleClick);
            // 
            // lbListHumanSign
            // 
            this.lbListHumanSign.AutoSize = true;
            this.lbListHumanSign.Location = new System.Drawing.Point(24, 81);
            this.lbListHumanSign.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lbListHumanSign.Name = "lbListHumanSign";
            this.lbListHumanSign.Size = new System.Drawing.Size(105, 25);
            this.lbListHumanSign.TabIndex = 48;
            this.lbListHumanSign.Text = "Test Data";
            // 
            // txbTrainResult
            // 
            this.txbTrainResult.Location = new System.Drawing.Point(24, 883);
            this.txbTrainResult.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txbTrainResult.Multiline = true;
            this.txbTrainResult.Name = "txbTrainResult";
            this.txbTrainResult.ReadOnly = true;
            this.txbTrainResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txbTrainResult.Size = new System.Drawing.Size(520, 323);
            this.txbTrainResult.TabIndex = 51;
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
            this.menuStrip1.Size = new System.Drawing.Size(2296, 44);
            this.menuStrip1.TabIndex = 52;
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
            // 
            // interpreterToolStripMenuItem
            // 
            this.interpreterToolStripMenuItem.Name = "interpreterToolStripMenuItem";
            this.interpreterToolStripMenuItem.Size = new System.Drawing.Size(268, 38);
            this.interpreterToolStripMenuItem.Text = "Interpreter";
            this.interpreterToolStripMenuItem.Click += new System.EventHandler(this.interpreterToolStripMenuItem_Click);
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(2296, 1233);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.txbTrainResult);
            this.Controls.Add(this.trvTestData);
            this.Controls.Add(this.lbListHumanSign);
            this.Controls.Add(this.btnCapture);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.btnRemoveAllTestData);
            this.Controls.Add(this.txbDebugConsole);
            this.Controls.Add(this.lbCaptureText);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Name = "TestForm";
            this.Text = "Leap Motion Gesture Test";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txbDebugConsole;
        private System.Windows.Forms.Label lbCaptureText;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnRemoveAllTestData;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Button btnCapture;
        private System.Windows.Forms.TreeView trvTestData;
        private System.Windows.Forms.Label lbListHumanSign;
        private System.Windows.Forms.TextBox txbTrainResult;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem leapMotionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem trainingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem testingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem interpreterToolStripMenuItem;
    }
}