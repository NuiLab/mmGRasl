using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using Leap;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json.Linq;
using Ionic.Zip;
using LeapMotionGestureTraining.Model;
using LeapMotionGestureTraining.Helper;
using LeapMotionGestureTraining.LMController;
using LeapMotionGestureTraining.LMController.LMModule;
using LeapMotionGestureTraining.AppSettings;

namespace LeapMotionGestureTraining
{
    public class FrameDataForm : Form, ILeapEventDelegate
    {
        enum CaptureStatus {PreviewSign, PreAutoCapture, PreCapture, Capture, FinishCapture};    
        CaptureStatus mCaptureStatus;
        private Controller controller;
        private PictureBox pictureBox1;
        private LeapEventListener listener;
        private LMFrame currentFrame;
        private Frame currentLeapMotionFrame;
        private Label label2;
        private PictureBox pictureBox2;
        private Label label3;
        private bool isGenerateImage;
        long lastMillisecond = 0;
        long delayMillisecondToRecord = 100;
        private Button btnReloadData;
        private Label lbListHumanSign;
        Dictionary<string, Model.LMFrame> dictHumanSign;
        private TreeView trvHumanSign;
        private Label lbSelectedSign;
        private Button btnAutoCapture;
        private Label label7;
        private TextBox txbStartIndex;
        private Label label6;
        private TextBox txbFileCount;
        private Label label4;
        private TextBox txbTimeOut;
        private Label label8;
        private TextBox txbHumanSign;
        private Label label9;
        private Button btnStartAutCapture;
        private Label lbCaptureText;
        string choosedSign;

        //  auto capture settings
        string autoCaptureSign;
        long autoCaptureTimeOut;
        int autoCaptureFileCount;
        int autoCaptureStartIndex;
        long lastAutocaptureTime;
        private Label label5;
        private TextBox txbDebugConsole;
        private Button btnTrain;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem leapMotionToolStripMenuItem;
        private ToolStripMenuItem trainingToolStripMenuItem;
        private ToolStripMenuItem testingToolStripMenuItem;
        private ToolStripMenuItem interpreterToolStripMenuItem;
        private ContextMenuStrip contextMenuStrip1;
        private IContainer components;
        int autoCaptureCount;
        private Label label1;
        private Button btnAddNewSign;
        private Button btnDeleteGesture;
        TrainModule mTrain;

        #region Init View

        public FrameDataForm()
        {
            InitializeComponent();
            // We will create new AppSetting.txt if this file doesnot exist.
            AppSetting.CheckAndCreateIfHaveNoAppSetting();
            JObject settObj = AppSetting.GetAppSetting();

            LMSingleton.Instance.compareDownPercent = (float)settObj[AppSetting.settingKeyCompareDownPercent];
            LMSingleton.Instance.compareTopPercent = (float)settObj[AppSetting.settingKeyCompareTopPercent];

            LMSingleton.Instance.currentForm = LMSingleton.FormName.Training;

            float cDownPercent = LMSingleton.Instance.compareDownPercent;
            float cTopPercent = LMSingleton.Instance.compareTopPercent;

            dictHumanSign = new Dictionary<string, Model.LMFrame>();
            this.controller = new Controller();
            this.listener = new LeapEventListener(this);
            controller.AddListener(listener);
            mCaptureStatus = CaptureStatus.PreviewSign;
            updateUIByCaptureStatus();
            Debug.WriteLine("Load Done");

            mTrain = new TrainModule();


            string[] signFolders = FileHelper.CheckSignFolders();
            // We will create alphabet folders if there is no folder found.
            if (signFolders.Length == 0)
            {
                FileHelper.createABCFolderForFirstTime();

                signFolders = FileHelper.CheckSignFolders();
            }
            LMSingleton.Instance.signFolders = signFolders;

            for (int i = 0; i < signFolders.Length; i++)
            {
                TreeNode node = new TreeNode(signFolders[i]);
                trvHumanSign.Nodes.Add(node);
            }

            reloadListHumanSign();

            trvHumanSign.SelectedNode = trvHumanSign.Nodes[0];
            choosedSign = clearNodeName( trvHumanSign.SelectedNode.Text);
            lbSelectedSign.Text = "Selected Gesture :" + choosedSign;

            initView();
        }

        void initView()
        {
            FirstLoadAutoCaptureSettings();
        }

        delegate void LeapEventDelegate(string EventName);

        /// <summary>
        /// Leap Motion event handling
        /// </summary>
        /// <param name="EventName">Name of event</param>
        public void LeapEventNotification(string EventName)
        {
            if (!this.InvokeRequired)
            {
                switch (EventName)
                {
                    case "onInit":
                        Debug.WriteLine("Init");
                        break;
                    case "onConnect":
                        Debug.WriteLine("Connected");
                        this.connectHandler();
                        break;
                    case "onFrame":
                        Debug.WriteLine("onFrame");
                        if (!this.Disposing)
                            this.newFrameHandler(this.controller.Frame());
                        break;
                }
            }
            else
            {
                BeginInvoke(new LeapEventDelegate(LeapEventNotification), new object[] { EventName });
            }
        }

        void connectHandler()
        {
            this.controller.EnableGesture(Gesture.GestureType.TYPE_CIRCLE);
            this.controller.Config.SetFloat("Gesture.Circle.MinRadius", 40.0f);
            this.controller.EnableGesture(Gesture.GestureType.TYPE_SWIPE);
            controller.SetPolicy(Controller.PolicyFlag.POLICY_IMAGES);
        }
       
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    this.controller.RemoveListener(this.listener);
                    this.controller.Dispose();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnReloadData = new System.Windows.Forms.Button();
            this.lbListHumanSign = new System.Windows.Forms.Label();
            this.trvHumanSign = new System.Windows.Forms.TreeView();
            this.lbSelectedSign = new System.Windows.Forms.Label();
            this.btnAutoCapture = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.txbStartIndex = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txbFileCount = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txbTimeOut = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txbHumanSign = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.btnStartAutCapture = new System.Windows.Forms.Button();
            this.lbCaptureText = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txbDebugConsole = new System.Windows.Forms.TextBox();
            this.btnTrain = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.leapMotionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.trainingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.interpreterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.btnAddNewSign = new System.Windows.Forms.Button();
            this.btnDeleteGesture = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.pictureBox1.Location = new System.Drawing.Point(294, 52);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(400, 250);
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(291, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(129, 25);
            this.label2.TabIndex = 4;
            this.label2.Text = "Left Camera";
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.SystemColors.ControlDark;
            this.pictureBox2.Location = new System.Drawing.Point(733, 52);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(400, 250);
            this.pictureBox2.TabIndex = 5;
            this.pictureBox2.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(730, 37);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(143, 25);
            this.label3.TabIndex = 6;
            this.label3.Text = "Right Camera";
            // 
            // btnReloadData
            // 
            this.btnReloadData.Location = new System.Drawing.Point(199, 532);
            this.btnReloadData.Name = "btnReloadData";
            this.btnReloadData.Size = new System.Drawing.Size(75, 23);
            this.btnReloadData.TabIndex = 20;
            this.btnReloadData.Text = "Reload";
            this.btnReloadData.UseVisualStyleBackColor = true;
            this.btnReloadData.Click += new System.EventHandler(this.btnReloadData_Click);
            // 
            // lbListHumanSign
            // 
            this.lbListHumanSign.AutoSize = true;
            this.lbListHumanSign.Location = new System.Drawing.Point(9, 36);
            this.lbListHumanSign.Name = "lbListHumanSign";
            this.lbListHumanSign.Size = new System.Drawing.Size(128, 25);
            this.lbListHumanSign.TabIndex = 21;
            this.lbListHumanSign.Text = "Gesture List";
            // 
            // trvHumanSign
            // 
            this.trvHumanSign.Location = new System.Drawing.Point(9, 52);
            this.trvHumanSign.Name = "trvHumanSign";
            this.trvHumanSign.ShowPlusMinus = false;
            this.trvHumanSign.Size = new System.Drawing.Size(262, 265);
            this.trvHumanSign.TabIndex = 23;
            this.trvHumanSign.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeViewMouseDoubleClick);
            // 
            // lbSelectedSign
            // 
            this.lbSelectedSign.AutoSize = true;
            this.lbSelectedSign.Location = new System.Drawing.Point(9, 323);
            this.lbSelectedSign.Name = "lbSelectedSign";
            this.lbSelectedSign.Size = new System.Drawing.Size(200, 25);
            this.lbSelectedSign.TabIndex = 24;
            this.lbSelectedSign.Text = "Selected gesture: A";
            // 
            // btnAutoCapture
            // 
            this.btnAutoCapture.Location = new System.Drawing.Point(12, 532);
            this.btnAutoCapture.Name = "btnAutoCapture";
            this.btnAutoCapture.Size = new System.Drawing.Size(84, 23);
            this.btnAutoCapture.TabIndex = 25;
            this.btnAutoCapture.Text = "Auto Capture";
            this.btnAutoCapture.UseVisualStyleBackColor = true;
            this.btnAutoCapture.Click += new System.EventHandler(this.btnAutoCapture_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 497);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(115, 25);
            this.label7.TabIndex = 34;
            this.label7.Text = "Start Index";
            // 
            // txbStartIndex
            // 
            this.txbStartIndex.Location = new System.Drawing.Point(113, 494);
            this.txbStartIndex.Name = "txbStartIndex";
            this.txbStartIndex.Size = new System.Drawing.Size(143, 31);
            this.txbStartIndex.TabIndex = 33;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 471);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(110, 25);
            this.label6.TabIndex = 32;
            this.label6.Text = "File Count";
            // 
            // txbFileCount
            // 
            this.txbFileCount.Location = new System.Drawing.Point(113, 468);
            this.txbFileCount.Name = "txbFileCount";
            this.txbFileCount.Size = new System.Drawing.Size(143, 31);
            this.txbFileCount.TabIndex = 31;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 445);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(95, 25);
            this.label4.TabIndex = 30;
            this.label4.Text = "Time out";
            // 
            // txbTimeOut
            // 
            this.txbTimeOut.Location = new System.Drawing.Point(113, 442);
            this.txbTimeOut.Name = "txbTimeOut";
            this.txbTimeOut.Size = new System.Drawing.Size(143, 31);
            this.txbTimeOut.TabIndex = 29;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 419);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(88, 25);
            this.label8.TabIndex = 28;
            this.label8.Text = "Gesture";
            // 
            // txbHumanSign
            // 
            this.txbHumanSign.Enabled = false;
            this.txbHumanSign.Location = new System.Drawing.Point(113, 416);
            this.txbHumanSign.Name = "txbHumanSign";
            this.txbHumanSign.Size = new System.Drawing.Size(143, 31);
            this.txbHumanSign.TabIndex = 27;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 400);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(228, 25);
            this.label9.TabIndex = 35;
            this.label9.Text = "Auto Capture Settings:";
            // 
            // btnStartAutCapture
            // 
            this.btnStartAutCapture.Location = new System.Drawing.Point(113, 532);
            this.btnStartAutCapture.Name = "btnStartAutCapture";
            this.btnStartAutCapture.Size = new System.Drawing.Size(75, 23);
            this.btnStartAutCapture.TabIndex = 36;
            this.btnStartAutCapture.Text = "Start";
            this.btnStartAutCapture.UseVisualStyleBackColor = true;
            this.btnStartAutCapture.Click += new System.EventHandler(this.btnStartAutCapture_Click);
            // 
            // lbCaptureText
            // 
            this.lbCaptureText.AutoSize = true;
            this.lbCaptureText.Location = new System.Drawing.Point(682, 305);
            this.lbCaptureText.Name = "lbCaptureText";
            this.lbCaptureText.Size = new System.Drawing.Size(123, 25);
            this.lbCaptureText.TabIndex = 37;
            this.lbCaptureText.Text = "Capure in 3";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(262, 445);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(40, 25);
            this.label5.TabIndex = 38;
            this.label5.Text = "ms";
            // 
            // txbDebugConsole
            // 
            this.txbDebugConsole.BackColor = System.Drawing.SystemColors.MenuText;
            this.txbDebugConsole.ForeColor = System.Drawing.SystemColors.Window;
            this.txbDebugConsole.Location = new System.Drawing.Point(294, 329);
            this.txbDebugConsole.Multiline = true;
            this.txbDebugConsole.Name = "txbDebugConsole";
            this.txbDebugConsole.ReadOnly = true;
            this.txbDebugConsole.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txbDebugConsole.Size = new System.Drawing.Size(842, 311);
            this.txbDebugConsole.TabIndex = 39;
            this.txbDebugConsole.Text = "Timestamp:             \r\nFrames Per Second:\r\nDisplay is Valid:\r\nHand Count :\r\nGes" +
    "ture Count :\r\n\r\n\r\n\r\n";
            // 
            // btnTrain
            // 
            this.btnTrain.Location = new System.Drawing.Point(9, 599);
            this.btnTrain.Name = "btnTrain";
            this.btnTrain.Size = new System.Drawing.Size(75, 23);
            this.btnTrain.TabIndex = 41;
            this.btnTrain.Text = "TRAIN";
            this.btnTrain.UseVisualStyleBackColor = true;
            this.btnTrain.Click += new System.EventHandler(this.btnTrain_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.leapMotionToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.menuStrip1.Size = new System.Drawing.Size(1148, 40);
            this.menuStrip1.TabIndex = 42;
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
            this.trainingToolStripMenuItem.Size = new System.Drawing.Size(227, 38);
            this.trainingToolStripMenuItem.Text = "Training";
            // 
            // testingToolStripMenuItem
            // 
            this.testingToolStripMenuItem.Name = "testingToolStripMenuItem";
            this.testingToolStripMenuItem.Size = new System.Drawing.Size(227, 38);
            this.testingToolStripMenuItem.Text = "Testing";
            this.testingToolStripMenuItem.Click += new System.EventHandler(this.testingToolStripMenuItem_Click);
            // 
            // interpreterToolStripMenuItem
            // 
            this.interpreterToolStripMenuItem.Name = "interpreterToolStripMenuItem";
            this.interpreterToolStripMenuItem.Size = new System.Drawing.Size(227, 38);
            this.interpreterToolStripMenuItem.Text = "Interpreter";
            this.interpreterToolStripMenuItem.Click += new System.EventHandler(this.interpreterToolStripMenuItem_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 583);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(375, 25);
            this.label1.TabIndex = 43;
            this.label1.Text = "Click to train post capturing a gesture.";
            // 
            // btnAddNewSign
            // 
            this.btnAddNewSign.Location = new System.Drawing.Point(176, 323);
            this.btnAddNewSign.Name = "btnAddNewSign";
            this.btnAddNewSign.Size = new System.Drawing.Size(97, 23);
            this.btnAddNewSign.TabIndex = 44;
            this.btnAddNewSign.Text = "Add Gesture";
            this.btnAddNewSign.UseVisualStyleBackColor = true;
            this.btnAddNewSign.Click += new System.EventHandler(this.btnAddNewSign_Click);
            // 
            // btnDeleteGesture
            // 
            this.btnDeleteGesture.Location = new System.Drawing.Point(176, 352);
            this.btnDeleteGesture.Name = "btnDeleteGesture";
            this.btnDeleteGesture.Size = new System.Drawing.Size(97, 23);
            this.btnDeleteGesture.TabIndex = 45;
            this.btnDeleteGesture.Text = "Delete Gesture";
            this.btnDeleteGesture.UseVisualStyleBackColor = true;
            this.btnDeleteGesture.Click += new System.EventHandler(this.btnDeleteGesture_Click);
            // 
            // FrameDataForm
            // 
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1148, 641);
            this.Controls.Add(this.btnDeleteGesture);
            this.Controls.Add(this.btnAddNewSign);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnTrain);
            this.Controls.Add(this.txbDebugConsole);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lbCaptureText);
            this.Controls.Add(this.btnStartAutCapture);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txbStartIndex);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txbFileCount);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txbTimeOut);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txbHumanSign);
            this.Controls.Add(this.btnAutoCapture);
            this.Controls.Add(this.lbSelectedSign);
            this.Controls.Add(this.trvHumanSign);
            this.Controls.Add(this.lbListHumanSign);
            this.Controls.Add(this.btnReloadData);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FrameDataForm";
            this.Text = "Leap Motion Gesture Trainer";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        #region Delegate 

        /// <summary>
        /// Leap Motion frame handling
        /// </summary>
        /// <param name="frame">A frame get from leap motion</param>
        void newFrameHandler(Frame frame)
        {

            long currentMillis = Helper.TimeHelper.currentTime();
            // capture only 1 frame for 1  humnan sign
            
            if (mCaptureStatus == CaptureStatus.PreAutoCapture)
            {
                if (currentMillis - lastMillisecond > delayMillisecondToRecord)
                {
                    Debug.WriteLine("Show image");
                    lastMillisecond = currentMillis;
                    showFrameInUIByFrame(frame);
                }

                return;
            }

            if (mCaptureStatus == CaptureStatus.PreCapture)
            {

                // auto capture each time. status change to capture 
                long autoCaptureCountdown = currentMillis - lastAutocaptureTime;
                if (autoCaptureCountdown > autoCaptureTimeOut)
                {
                    lastAutocaptureTime = currentMillis;
                    mCaptureStatus = CaptureStatus.Capture;
                    lbCaptureText.Text = "Capturing ....";
                }
                else
                {
                    lbCaptureText.Text = "Capture in " + autoCaptureCountdown + " milliseconds";

                    if (currentMillis - lastMillisecond > delayMillisecondToRecord)
                    {
                        Debug.WriteLine("Show image");
                        lastMillisecond = currentMillis;
                        showFrameInUIByFrame(frame);
                    }

                    return;
                }

                
            }
            if (mCaptureStatus != CaptureStatus.Capture)
            {
                return;
            }
            mCaptureStatus = CaptureStatus.FinishCapture;

            // Leap Motion Frame will be convert to LMFrame
            LMFrame lmFrame;
            try
            {
                lmFrame = new LMFrame(frame, choosedSign);
            }
            catch (Exception e)
            {
                FileHelper.saveDebugString("LMFrame create : " + "frame data: " +frame.ToString() + "\\n" + e.Data.ToString());
                return;
            }

            if (lmFrame.LeftCamImg == null)
            {
                return;
            }
            

            // Save test data
            currentLeapMotionFrame = frame;

            showFrameInUI(lmFrame);

            saveJSONData(lmFrame);

            if (autoCaptureCount + 2 > autoCaptureFileCount)
            {
                lbCaptureText.Text = "Auto capture finished.";
                mCaptureStatus = CaptureStatus.PreviewSign;
                reloadListHumanSign();
                updateUIByCaptureStatus();
            }
            else
            {
                autoCaptureCount++;
            }
        }

        /// <summary>
        /// Update UI with frame information
        /// </summary>
        /// <param name="frame">Selected frame</param>
        void showFrameInUIByFrame(Frame frame)
        {
            string leapDesc = LMFrame.DebugStringFromFrame(frame);

            if (this.txbDebugConsole.InvokeRequired)
            {
                txbDebugConsole.Invoke(new dlgUpdateFrameInfo(updateFrameInfo), new object[] { leapDesc });
            }
            else
            {
                updateFrameInfo(leapDesc);
            }

            pictureBox1.Image = Helper.ImageHelper.generateBitmapFromLeapImage(frame.Images[0]);
            pictureBox2.Image = Helper.ImageHelper.generateBitmapFromLeapImage(frame.Images[1]);
        }

        /// <summary>
        /// Update UI with LMFrame data
        /// </summary>
        /// <param name="lmFrame">Selected LMFrame</param>
        void showFrameInUI(LMFrame lmFrame)
        {
            currentFrame = lmFrame ;

            string leapDesc = "";

            if (this.txbDebugConsole.InvokeRequired)
            {
                txbDebugConsole.Invoke(new dlgUpdateFrameInfo(updateFrameInfo), new object[] { leapDesc });
            }
            else
            {
                updateFrameInfo(leapDesc);
            }

            LeapGenImageThenShow(lmFrame);
        }

        private delegate void dlgUpdateFrameInfo(string aLeapDesc);
     
        void updateFrameInfo(string aLeapDesc)
        {

            if (aLeapDesc.Length > 0)
            {
                // preview capture
                txbDebugConsole.Text = aLeapDesc;
                Debug.WriteLine(aLeapDesc);
            }
            else
            {
                txbDebugConsole.Text = currentFrame.DebugString();
            }
        }


        private async Task<int> LeapGenImageThenShow(LMFrame frame)
        {
            return await Task<int>.Run(() =>
            {
                if (isGenerateImage)
                {
                    return 0;
                }
                isGenerateImage = true;
                Debug.WriteLine("Processing Image");

                pictureBox1.Image = frame.LeftCamImg;
                pictureBox2.Image = frame.RightCamImg;

                isGenerateImage = false;
                return 0;
            });
        }

        #endregion

        #region Action

        private void btnCapture_Click(object sender, EventArgs e)
        {
            mCaptureStatus = CaptureStatus.Capture;
            updateUIByCaptureStatus();
        }

        private void btnReloadData_Click(object sender, EventArgs e)
        {
            reloadListHumanSign();
        }

        private void btnAutoCapture_Click(object sender, EventArgs e)
        {
            if (checkAutoCaptureSettings())
            {
                mCaptureStatus = CaptureStatus.PreAutoCapture;
                updateUIByCaptureStatus();
            }

        }

        private void btnStartAutCapture_Click(object sender, EventArgs e)
        {
            string btnText = ((Button)sender).Text;
            if (btnText == "Stop")
            {
                mCaptureStatus = CaptureStatus.PreviewSign;
                updateUIByCaptureStatus();
            }
            else
            {
                lastAutocaptureTime = Helper.TimeHelper.currentTime();
                autoCaptureCount = 0;
                mCaptureStatus = CaptureStatus.PreCapture;

                if (checkAutoCaptureSettings())
                {
                    autoCaptureTimeOut = int.Parse(txbTimeOut.Text);
                    autoCaptureFileCount = int.Parse(txbFileCount.Text);
                    autoCaptureStartIndex = int.Parse(txbStartIndex.Text);
                    updateUIByCaptureStatus();
                }
            }
        }

        private void btnTrain_Click(object sender, EventArgs e)
        {
            List<string> selectedSign = Promt.ShowCheckedListBox(FileHelper.CheckSignFolders(), "Select gesture to be trained");

            if (selectedSign != null)
            {
                mTrain.ConvertSelectedSignDataToAngle(selectedSign);
            }
        }

        private void btnAddNewSign_Click(object sender, EventArgs e)
        {
            ShowPopUpEnterCustomSignName();
        }

        private void btnDeleteGesture_Click(object sender, EventArgs e)
        {
            List<string> selectedSign = Promt.ShowCheckedListBox(FileHelper.CheckSignFolders(), "Select gesture to be deleted");

            if (selectedSign != null)
            {
                mTrain.DeleteSelectedSign(selectedSign);
                reloadListHumanSign();
            }
        }

        public void testingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!LMSingleton.Instance.currentForm.Equals(LMSingleton.FormName.Testing))
            {
                TestForm test = new TestForm();
                test.Show();
                this.Hide();
            }
        }

        public void interpreterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!LMSingleton.Instance.currentForm.Equals(LMSingleton.FormName.Interpreter))
            {
                InterpreterForm inter = new InterpreterForm();
                inter.Show();
                this.Hide();
            }
        }

        #endregion

        #region Add new custom sign
        /// <summary>
        /// Show Dialog to enter new gesture name.
        /// </summary>
        void ShowPopUpEnterCustomSignName()
        {
            string newCustomSign = Promt.ShowDialog("Enter a name", "Add a new gesture");

            if (newCustomSign.Trim().Length > 0)
            {
                CreateCustomSignName(newCustomSign);
            }
        }

        /// <summary>
        /// Craete a new gesture folder
        /// </summary>
        /// <param name="signName">Gesture name</param>
        void CreateCustomSignName(string signName)
        {
            FileHelper.CreateCustomSignFolder(signName);
            TreeNode node = new TreeNode(signName);
            trvHumanSign.Nodes.Add(node);
            reloadListHumanSign();
        }
        #endregion

        #region TreeView Handler
        private void treeViewMouseDoubleClick(object sender, MouseEventArgs e)
        {
            //
            // Get the selected node.
            //
            TreeNode node = trvHumanSign.SelectedNode;
            //
            // Render message box.
            //
            string nodeStr = node.Text;

            if (nodeStr.Contains("("))
            {
                nodeStr = clearNodeName(nodeStr);
                // selected Sign
                choosedSign = nodeStr;
                lbSelectedSign.Text = "Selected Sign :" + nodeStr;
                txbHumanSign.Text = nodeStr;
                isGenerateImage = false;
                mCaptureStatus = CaptureStatus.PreviewSign;
                updateUIByCaptureStatus();
            }
            else
            {
                mCaptureStatus = CaptureStatus.PreviewSign;
                updateUIByCaptureStatus();
                // load json - nodeStr = fileName
                loadCaptureData(nodeStr);
            }
        }
        #endregion

        #region Helper

        /// <summary>
        /// Save captured LMFrame data to .txt file
        /// </summary>
        /// <param name="lmFrame">Captured LMFrame</param>
        void saveJSONData(LMFrame lmFrame)
        {
            string jsonStr = lmFrame.toJSON().ToString();
            Debug.WriteLine(jsonStr);
            // save .txt and .zip file
            saveFile(jsonStr, choosedSign);
        }

        /// <summary>
        /// AngleBetween - the angle between 2 vectors
        /// </summary>
        /// <returns>
        /// Returns the the angle in degrees between vector1 and vector2
        /// </returns>
        /// <param name="vector1"> The first Vector </param>
        /// <param name="vector2"> The second Vector </param>
        public double AngleBetween(Vector v1, Vector v2)
        {
            float ab = v1.x * v2.x + v1.y * v2.y + v1.z * v2.z;
            double aLeng = Math.Sqrt(v1.x * v1.x + v1.y * v1.y + v1.z * v1.z);
            double bLeng = Math.Sqrt(v2.x * v2.x + v2.y * v2.y + v2.z * v2.z);
            double abLeng = aLeng * bLeng;
            double radian = Math.Acos(ab / abLeng);
            double degress = radian * (180 / Math.PI);

            return radian;
        }

        /// <summary>
        /// Clean node name of listview. A(200) -> A
        /// </summary>
        /// <param name="nodeStr">Node name</param>
        /// <returns></returns>
        string clearNodeName(string nodeStr)
        {
            if (nodeStr.Contains('('))
            {
                string[] splitStr = nodeStr.Split('(');
                nodeStr = splitStr.First<string>();
            }

            return nodeStr;
        }

        /// <summary>
        /// Default auto capture data setttings.
        /// </summary>
        void FirstLoadAutoCaptureSettings()
        {
            txbHumanSign.Text = LMSingleton.Instance.signFolders[0];
            txbFileCount.Text = 40 +"";
            txbStartIndex.Text = 1 + "";
            txbTimeOut.Text = 500 + "";
        }

        /// <summary>
        /// Check auto capture data setting is correct or not.
        /// </summary>
        /// <returns></returns>
        bool checkAutoCaptureSettings()
        {
            string timeOutStr = txbTimeOut.Text;
            string fileCountStr = txbFileCount.Text;
            string startIndexStr = txbStartIndex.Text;

            if (choosedSign.Length == 0)
            {
                MessageBox.Show("Please select a choose sign");
                return false;
            }
            else if (timeOutStr.Length == 0 || fileCountStr.Length == 0 || startIndexStr.Length == 0)
            {
                MessageBox.Show("Please fill auto capture settings.");
                return false;
            }

            autoCaptureSign = choosedSign;
            autoCaptureTimeOut = long.Parse(timeOutStr);
            autoCaptureFileCount = int.Parse(fileCountStr);
            autoCaptureStartIndex = int.Parse(startIndexStr);

            return true;
        }

        /// <summary>
        /// Update UI by capture status
        /// </summary>
        void updateUIByCaptureStatus()
        {
            // update ui follow capture status
            trvHumanSign.Enabled = false;
            txbTimeOut.ReadOnly = false;
            txbFileCount.ReadOnly = false;
            txbStartIndex.ReadOnly = false;

            if (mCaptureStatus == CaptureStatus.PreviewSign)
            {
                trvHumanSign.Enabled = true;
                btnAutoCapture.Show();
                btnStartAutCapture.Hide();
            }
            else if (mCaptureStatus == CaptureStatus.PreAutoCapture)
            {
                btnAutoCapture.Hide();
                btnStartAutCapture.Show();
                btnStartAutCapture.Text = "Start";
            }
            else if (mCaptureStatus == CaptureStatus.PreCapture)
            {
                txbTimeOut.ReadOnly = true;
                txbFileCount.ReadOnly = true;
                txbStartIndex.ReadOnly = true;
                btnStartAutCapture.Text = "Stop";
            }     
        }

        /// <summary>
        /// Save capture LMFrame JSON data to .txt file
        /// </summary>
        /// <param name="content">LMFrame JSON data</param>
        /// <param name="humanSign">Gesture name</param>
        void saveFile(string content, string humanSign)
        { 
            string folderPath = FileHelper.captureDataFolderPath(humanSign);
            string fileName = captureDataFileNameGetNext(folderPath);
            string txtFilePath = folderPath + "\\" + fileName + ".txt";

            Debug.WriteLine("Begin save");
            // pass in true to cause writes to be appended
            StreamWriter sw = new StreamWriter(txtFilePath, true);

            // use writeline so we get a newline character written
            sw.WriteLine(content);

            // Ensure data is written to disk
            sw.Close();

            string rawFile = folderPath + "\\" + fileName + ".raw";
            byte[] rawData = currentLeapMotionFrame.Serialize;
            bool isSuccessSaveFile = ByteArrayToFile(rawFile, rawData);

            //UNCOMMENT TO ENABLE COMPRESSION
            //Helper.FileHelper.zipFileSaveInFolder(txtFilePath);

            mCaptureStatus = CaptureStatus.PreCapture;
            updateUIByCaptureStatus();
            Debug.WriteLine("Save successful");
        }

        /// <summary>
        /// Save byte array (raw data of Leap Motion frame) to .raw file
        /// </summary>
        /// <param name="fileName">File path</param>
        /// <param name="byteArray">Byte array</param>
        /// <returns></returns>
        public bool ByteArrayToFile(string fileName, byte[] byteArray)
        {
            try
            {
                using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(byteArray, 0, byteArray.Length);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught in process: {0}", ex);
                return false;
            }
        }
        
        /// <summary>
        /// Generate next file name by file count in current gesture folder. 
        /// </summary>
        /// <param name="folderPath">Current gesture folder path</param>
        /// <returns></returns>
        string captureDataFileNameGetNext(string folderPath)
        {
            string filePath = "";
            string[] filenames = Directory.GetFiles(folderPath, "*.txt");
            filePath = "" + filenames.Length.ToString("000");

            return filePath;
        }

        /// <summary>
        /// Load saved gesture data and show it's information in UI.
        /// </summary>
        /// <param name="fileName">Gesture data file path</param>
        void loadCaptureData(string fileName)
        {
            string captureFolder = FileHelper.captureDataFolderPath(choosedSign);
            string filePath = captureFolder + "\\" + fileName;

            if (fileName.Contains(".zip"))
            {
                filePath = Helper.FileHelper.txtFilePathFromUnzip(filePath);
            }

            string text = System.IO.File.ReadAllText(filePath);
            JObject jObj = JObject.Parse(text);
            LMFrame frame = new LMFrame(jObj);
            currentFrame = frame;
            showFrameInUI(frame);
        }

        /// <summary>
        /// Reload gesture list
        /// </summary>
        void reloadListHumanSign()
        {
            trvHumanSign.BeginUpdate();
            trvHumanSign.Nodes.Clear();
            LMSingleton.Instance.signFolders = FileHelper.CheckSignFolders();

            foreach (string sign in LMSingleton.Instance.signFolders)
            {
                TreeNode node = new TreeNode(sign);
                string signFolderPath = FileHelper.captureDataFolderPath(sign);
                var allowedExtensions = new[] { ".txt", ".zip" };
                string[] arrFileNames = Directory.GetFiles(signFolderPath)
                                        .Where(file => allowedExtensions.Any(file.ToLower().EndsWith))
                                        .ToArray();

                foreach (string filePath in arrFileNames)
                {
                    string[] names = filePath.Split('\\');
                    string fileName = names.Last<string>();
                    TreeNode newSign = new TreeNode(fileName);
                    node.Nodes.Add(newSign);
                }

                node.Text = clearNodeName(node.Text) + "(" + arrFileNames.Length + ")";

                trvHumanSign.Nodes.Add(node);
            }

            trvHumanSign.EndUpdate();
        }
        #endregion
    }
}