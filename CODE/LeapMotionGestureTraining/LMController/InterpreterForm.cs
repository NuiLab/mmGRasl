using Leap;
using LeapMotionGestureTraining.Helper;
using LeapMotionGestureTraining.LMController.LMModule;
using LeapMotionGestureTraining.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LeapMotionGestureTraining.LMController
{
    public partial class InterpreterForm : Form, ILeapEventDelegate
    {
        enum TestStatus { Training, Testing, Capturing };

        TestStatus mStatus;

        LMFrame currentFrame;

        private Controller controller;
        private LeapEventListener listener;
        TrainModule mTrain;


        long lastMillisecond = 0;
        long delayMillisecondToRecord = 100;
        private bool isCapture;

        public InterpreterForm()
        {
            InitializeComponent();
            Debug.WriteLine("Init Interpreter");
            LMSingleton.Instance.currentForm = LMSingleton.FormName.Interpreter;

            isCapture = false;
            this.controller = new Controller();
            this.listener = new LeapEventListener(this);
            controller.AddListener(listener);

            mStatus = TestStatus.Training;
            ReloadUIByStatus();
            mTrain = new TrainModule();


            // neu co data moi thi se load lai training
            //mTrain.ConvertSignDataToAngle();

            // load Test data
            mStatus = TestStatus.Testing;
            ReloadUIByStatus();
            mTrain.ReloadTrainResult();

        }


        #region Init Form 
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

        #endregion

        #region Action

        private void btnTrain_Click(object sender, EventArgs e)
        {
            mTrain.ConvertSignDataToAngle();
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            mStatus = TestStatus.Testing;
            ReloadUIByStatus();
            mTrain.ReloadTrainResult();
        }

        private void btnCapture_Click(object sender, EventArgs e)
        {
            if (btnCapture.Text.Contains("Re-interpreter"))
            {
                btnCapture.Text = "Interpreter";
                mStatus = TestStatus.Testing;
                isCapture = false;
            }
            else
            {
                btnCapture.Text = "Re-interpreter";
                mStatus = TestStatus.Capturing;
                isCapture = false;
            }

        }

        public void trainingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrameDataForm train = new FrameDataForm();
            train.Show();
            this.Hide();
        }

        public void testingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TestForm test = new TestForm();
            test.Show();
            this.Hide();
        }

        public void interpreterToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        #endregion


        #region Leap Handler
        /// <summary>
        /// Leap Motion frame handling
        /// </summary>
        /// <param name="frame">A frame get from leap motion</param>
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
                    //showFrameInUI(lmFrame);
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

                //string signName = mTrain.SignNameFromLMFrame(testFrame);
                string signName = mTrain.SignNameFromLMFrameByListData(testFrame);

                string[] names = signName.Split('-');

                lbCaptureText.Text = "Gesture Detected: " + names[0];
                txbDebugConsole.Text = names[1];


                string folderPath = FileHelper.interpreterFolderPath();
                string fileName = captureDataFileNameGetNext(folderPath);

                string txtFilePath = folderPath + "\\" + fileName + ".txt";

                Debug.WriteLine("Begin save");
                // pass in true to cause writes to be appended
                StreamWriter sw = new StreamWriter(txtFilePath, true);

                // use writeline so we get a newline character written
                sw.WriteLine(lmFrame.toJSON().ToString());

                // Ensure data is written to disk
                sw.Close();
            }
        }
        #endregion


        #region UI Handler
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
        /// <summary>
        /// Generate next file name by file count in current gesture folder. 
        /// </summary>
        /// <param name="folderPath">Current gesture folder path</param>
        /// <returns></returns>
        string captureDataFileNameGetNext(string folderPath)
        {
            string filePath = "";
            string[] filenames = Directory.GetFiles(folderPath, "*.txt");

            filePath = "" + (filenames.Length + 1).ToString("000");

            return filePath;
        }

        /// <summary>
        /// Reload UI by test status
        /// </summary>
        void ReloadUIByStatus()
        {
            if (mStatus == TestStatus.Training)
            {
            }
        }
        #endregion    
    }
}
