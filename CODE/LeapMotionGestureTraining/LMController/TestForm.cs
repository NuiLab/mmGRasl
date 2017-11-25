using Leap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LeapMotionGestureTraining.LMController.LMModule;
using LeapMotionGestureTraining.Model;
using LeapMotionGestureTraining.Helper;
using System.IO;
using Newtonsoft.Json.Linq;

namespace LeapMotionGestureTraining.LMController
{
    public partial class TestForm : Form, ILeapEventDelegate
    {
        enum TestStatus { Training, Testing, Capturing};

        TestStatus mStatus;

        LMFrame currentFrame;

        private Controller controller;
        private LeapEventListener listener;
        TrainModule mTrain;


        long lastMillisecond = 0;
        long delayMillisecondToRecord = 100;
        private bool isCapture;

        public TestForm()
        {
            LMSingleton.Instance.currentForm = LMSingleton.FormName.Testing;
            InitializeComponent();
            isCapture = false;
            this.controller = new Controller();
            this.listener = new LeapEventListener(this);
            controller.AddListener(listener);

            mStatus = TestStatus.Training;
            ReloadUIByStatus();
            mTrain = new TrainModule();

            ReloadListTestData();
        }

        #region Init Form 
        delegate void LeapEventDelegate(string EventName);
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

        #endregion

        #region Leap Handler
        // get_frame_from_leap_motion_device
        void newFrameHandler(Frame frame)
        {
            if (mStatus == TestStatus.Training)
            {
                return;
            }
            
            if (mStatus == TestStatus.Testing)
            {
                long currentMillis = Helper.TimeHelper.currentTime();
                // capture only 1 frame for 1  humnan sign


                if (currentMillis - lastMillisecond > delayMillisecondToRecord)
                {
                    Debug.WriteLine("Show image");
                    lastMillisecond = currentMillis;
                    showFrameInUIByFrame(frame);
                }
            }


            if (mStatus == TestStatus.Capturing)
            {
                if (isCapture)
                {
                    return;
                }
                isCapture = true;
                LMFrame lmFrame = new LMFrame(frame, "0");

                LMFrame testFrame = new LMFrame(JObject.Parse(lmFrame.toJSON().ToString()));

                currentFrame = testFrame;

                JObject angleObj = mTrain.angleJSONObjectFromLMFrame(testFrame);

                StringBuilder debugTxt = new StringBuilder();
                debugTxt.AppendLine(testFrame.DebugString());
                debugTxt.AppendLine("-------------------------------");
                debugTxt.AppendLine(angleObj.ToString());

                txbDebugConsole.Text = debugTxt.ToString();

                string signName = mTrain.SignNameFromLMFrameByListData(testFrame);

                txbTrainResult.Text = signName;

                string folderPath = FileHelper.testDataFolderPath();
                string fileName = captureDataFileNameGetNext(folderPath);

                string txtFilePath = folderPath + "\\" + fileName + ".txt";

                Debug.WriteLine("Begin save");
                // pass in true to cause writes to be appended
                StreamWriter sw = new StreamWriter(txtFilePath, true);

                // use writeline so we get a newline character written
                sw.WriteLine(lmFrame.toJSON().ToString());

                // Ensure data is written to disk
                sw.Close();

                ReloadListTestData();
            }
        }

        string captureDataFileNameGetNext(string folderPath)
        {
            string filePath = "";
            string[] filenames = Directory.GetFiles(folderPath, "*.txt");

            filePath = "" + (filenames.Length + 1).ToString("000");

            return filePath;
        }
        #endregion

        #region UI Handler

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

        #endregion

        #region Helper
        void ReloadListTestData ()
        {
            trvTestData.BeginUpdate();

            trvTestData.Nodes.Clear();
           
            string signFolderPath = FileHelper.testDataFolderPath();

            var allowedExtensions = new[] { ".txt" };
            string[] arrFileNames = Directory.GetFiles(signFolderPath)
                                    .Where(file => allowedExtensions.Any(file.ToLower().EndsWith))
                                    .ToArray();

            foreach (string filePath in arrFileNames)
            {
                string fileName = Path.GetFileName(filePath);

                TreeNode newSign = new TreeNode(fileName);
                trvTestData.Nodes.Add(newSign);
            }

            trvTestData.EndUpdate();
        }

        void RemoveAllFileInTestData()
        {
            string signFolderPath = FileHelper.testDataFolderPath();

            var allowedExtensions = new[] { ".txt" };
            string[] arrFileNames = Directory.GetFiles(signFolderPath)
                                    .Where(file => allowedExtensions.Any(file.ToLower().EndsWith))
                                    .ToArray();

            foreach (string filePath in arrFileNames)
            {
                File.Delete(filePath);
            }

            ReloadListTestData();
        }

        void ReloadUIByStatus()
        {
            if (mStatus == TestStatus.Training)
            {
            }
        }

        #endregion

        private void treeViewMouseDoubleClick(object sender, MouseEventArgs e)
        {
            //
            // Get the selected node.
            //
            TreeNode node = trvTestData.SelectedNode;
            //
            // Render message box.
            //

            string nodeStr = node.Text;

            string testFolderPath = FileHelper.testDataFolderPath();

            string filePath = testFolderPath + "\\" + nodeStr;

            string frameStr = File.ReadAllText(filePath);
            JObject jFrameObj = JObject.Parse(frameStr);
            LMFrame aFrame = new LMFrame(jFrameObj);

            JObject angleObj = mTrain.angleJSONObjectFromLMFrame(aFrame);

            StringBuilder debugTxt = new StringBuilder();
            debugTxt.AppendLine(aFrame.DebugString());
            debugTxt.AppendLine("-------------------------------");
            debugTxt.AppendLine(angleObj.ToString());

            txbDebugConsole.Text = debugTxt.ToString();
            pictureBox1.Image = aFrame.LeftCamImg;
            pictureBox2.Image = aFrame.RightCamImg;

            string signName = mTrain.SignNameFromLMFrame(aFrame);

            txbTrainResult.Text = signName;
        }


        private void btnTrain_Click(object sender, EventArgs e)
        {
            mTrain.ConvertSignDataToAngle();
        }

        private void btnRemoveAllTestData_Click(object sender, EventArgs e)
        {
            RemoveAllFileInTestData();
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            mStatus = TestStatus.Testing;
            ReloadUIByStatus();
            mTrain.ReloadTrainResult();
        }

        private void btnCapture_Click(object sender, EventArgs e)
        {
            if (btnCapture.Text.Contains("Re-capture"))
            {
                btnCapture.Text = "Capture";
                mStatus = TestStatus.Testing;
                isCapture = false;
            }
            else
            {
                btnCapture.Text = "Re-capture";
                mStatus = TestStatus.Capturing;
                isCapture = false;
            }        
        }

        private void btnBackToTrain_Click(object sender, EventArgs e)
        {
            FrameDataForm form = new FrameDataForm();
            form.Show();
            this.Hide();
        }

        public void trainingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!LMSingleton.Instance.currentForm.Equals(LMSingleton.FormName.Training))
            {
                FrameDataForm train = new FrameDataForm();
                train.Show();
                this.Hide();
            }

        }

        public void testingToolStripMenuItem_Click(object sender, EventArgs e)
        {

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
    }
}
