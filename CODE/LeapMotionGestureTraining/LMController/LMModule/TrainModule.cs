using Leap;
using LeapMotionGestureTraining.Helper;
using LeapMotionGestureTraining.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace LeapMotionGestureTraining.LMController.LMModule
{
    class TrainModule
    {
        Dictionary<string, JObject> dictTrainResult;

        public TrainModule()
        {
            dictTrainResult = new Dictionary<string, JObject>();
        }

        // 
        /// <summary>
        /// Get sign name by compareing captured data with all training data.
        /// We use compareTopPercent and compareBottomPercent to create space that make
        /// finding more accurate.
        /// </summary>
        /// <param name="lmFrame"></param> Captured data that is converted to LMFrame 
        /// <returns>Return sign math and comparison result as string.</returns>
        public string SignNameFromLMFrameByListData(LMFrame lmFrame)
        {
            StringBuilder builder = new StringBuilder();

            JObject frameAngleObj = angleJSONObjectFromLMFrame(lmFrame);

            string lessFailCountKey = "";
            int currentLessFailCount = 2000;

            foreach (string trainKey in dictTrainResult.Keys)
            {
                int failCount = 0 ;
                int total = 0;

                List<string> hands = frameAngleObj.Properties().Select(p => p.Name).ToList();

                foreach (string hand in  hands)
                {
                    JObject testObj = (JObject)frameAngleObj[hand];

                    List<string> keys = testObj.Properties().Select(p => p.Name).ToList();

                    string[] arrSignTrainData = getListFileFromSign(trainKey);

                    foreach (string signTrainPath in arrSignTrainData)
                    {
                        if (signTrainPath.Contains("_angle"))
                        {
                            Debug.WriteLine("Compare file " + signTrainPath);

                            string signAngleTxt = File.ReadAllText(signTrainPath);
                            JObject preTrainObj = JObject.Parse(signAngleTxt);
                            List<string> handTrainkeys = preTrainObj.Properties().Select(p => p.Name).ToList();

                            foreach (string handTrain in handTrainkeys)
                            {
                                JObject trainObj = (JObject)preTrainObj[handTrain];
                                JObject compareObj = compareTestAndTrainData(testObj, trainObj);

                                bool isSign = (bool)compareObj["isSign"]; 
                                if (!isSign)
                                {
                                    failCount++;
                                }

                                total++;
                            }     
                        }
                    }
                }

                builder.AppendLine("Sign " + trainKey + " fail count : " + failCount + "/" + total);

                if (failCount < currentLessFailCount)
                {
                    lessFailCountKey = trainKey;
                    currentLessFailCount = failCount;
                }

            }

            string returnSignName = lessFailCountKey + "-\n" + builder.ToString();

            return returnSignName;
        }

        // Save the comparison results of captured data and training data. 
        // This method is not used anymore to speed up the finding progress.
        public void saveCompareObj(JObject compareObj, string signName, string signTrainPath)
        {
            string fileName = Path.GetFileNameWithoutExtension(signTrainPath);
            string testSignFolder = FileHelper.TestFolderPath(signName);          
            string filePath = testSignFolder + "\\" + fileName + ".txt";

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            StreamWriter sw = new StreamWriter(filePath, true);

            // use writeline so we get a newline character written
            sw.WriteLine(compareObj.ToString());

            // Ensure data is written to disk
            sw.Close();
        }

        /// <summary>
        /// Compare captured data and trained data
        /// </summary>
        /// <param name="testObj"></param> Captured data that converted to JSON object type
        /// <param name="trainObj"></param> Trained data that ceonverted to JSON object type
        /// <returns>Return JSON object that contain comparative parametters and is trained sign or not. </returns>
        public JObject compareTestAndTrainData(JObject testObj, JObject trainObj)
        {
            List<string> keys = testObj.Properties().Select(p => p.Name).ToList();  
            JObject compareObj = new JObject();
            int count = 0;
            double alpha = 0;
            int countBigger80Percent = 0;
            int countSmaller80Percent = 0;

            foreach (string key in keys)
            {
                double testValue =(double) testObj[key];
                double trainValue = (double)trainObj[key];

                if (count == 0)
                {
                    alpha = testValue - trainValue;
                    compareObj.Add(key, "test : " + testValue + " - train : " + trainValue + " - diff: " + alpha + " - percent : 100%");
                }
                else
                {
                    double testAlpha = trainValue + alpha;
                    double change = testValue / trainValue * 100;
                    compareObj.Add(key, "test : " + testValue + " - train : " + trainValue + " - testAlpha: " + testAlpha + " - percent : " + change + "%");

                    if (change > LMSingleton.Instance.compareDownPercent && change < LMSingleton.Instance.compareTopPercent)
                    {
                        countBigger80Percent++;
                    }
                    else
                    {
                        countSmaller80Percent++;
                    }
                    
                }

                count++;              
            }

            compareObj.Add("range_80_to_120", countBigger80Percent);
            compareObj.Add("out_range_80_to_120", countSmaller80Percent);

            // 36 angle
            if (countBigger80Percent - countSmaller80Percent > 12)
            {
                compareObj.Add("isSign", true);
            }
            else
            {
                compareObj.Add("isSign", false);
            }

            return compareObj;
        }

        /// <summary>
        /// Get list file .txt in chosen sign folder
        /// </summary>
        /// <param name="sign">Chosen sign</param>
        /// <returns>Return array of file path </returns>
        public string[] getListFileFromSign(string sign)
        {
            string signFolderPath = FileHelper.captureDataFolderPath(sign);
            var allowedExtensions = new[] { ".txt"};
            string[] arrFileNames = Directory.GetFiles(signFolderPath)
                                    .Where(file => allowedExtensions.Any(file.ToLower().EndsWith))
                                    .ToArray();

            return arrFileNames;
        }

        /// <summary>
        /// Old training system. Not used anymore.
        /// </summary>
        /// <param name="lmFrame"></param>
        /// <returns></returns>
        public string SignNameFromLMFrame(LMFrame lmFrame)
        {
            StringBuilder builder = new StringBuilder();

            JObject frameAngleObj = angleJSONObjectFromLMFrame(lmFrame);

            foreach (string trainKey in dictTrainResult.Keys)
            {
                JObject trainResult = dictTrainResult[trainKey];
                List<string> hands = frameAngleObj.Properties().Select(p => p.Name).ToList();

                bool isSign = true;

                int lengCount = 0;
                int angleCount = 0;
                float lengTotal = 0;
                float angleTotal = 0;
                foreach (string hand in hands)
                {
                    JObject angleObj = (JObject)frameAngleObj[hand];

                    FileHelper.saveFileToTestFolder(lmFrame.Id.ToString(), angleObj.ToString());

                    int failCount = 0;

                    
                    List<string> keys = angleObj.Properties().Select(p => p.Name).ToList();
                    foreach (string key in keys)
                    {
                        float testValue = (float)angleObj[key];
                        float trainValue = (float)trainResult[key];

                        if (key.Contains("leng"))
                        {
                            lengTotal += testValue / trainValue;
                            lengCount++;
                        }
                        else
                        {
                            angleTotal += testValue / trainValue;
                            angleCount++;
                        }

                    }


                    float lengPercent = lengTotal / lengCount;
                    float anglePercent = angleTotal / angleCount;

                    if (lengPercent < 1 && anglePercent < 1)
                    {
                        isSign = true;
                    }
                    else if (lengPercent > 1 && anglePercent > 1)
                    {
                        isSign = true;
                    }
                    else
                    {
                        isSign = false;
                    }

                    builder.AppendLine(trainKey + " Leng %: " + lengPercent + " - angle %: " + anglePercent + "-> isSign: " + isSign);

                }

            }

            return builder.ToString() ;
        }
       
        /// <summary>
        /// Reload all training result 
        /// </summary>
        public void ReloadTrainResult()
        {
            string[] abcArr = FileHelper.CheckSignFolders();

            foreach (string signName in abcArr)
            {
                string signFolderPath = FileHelper.captureDataFolderPath(signName);
                string trainResultPath = signFolderPath + "\\TrainResult\\trainResult" + signName + ".txt";
                if (File.Exists(trainResultPath))
                {
                    string trainResultText = File.ReadAllText(trainResultPath);

                    JObject trainObj = JObject.Parse(trainResultText);

                    dictTrainResult.Add(signName, trainObj);
                }               
            }
        }

        /// <summary>
        /// A part of training. We will convert captured data to angle with hands and fingers.
        /// </summary>
        /// <param name="signs"> List sign folders will be converted </param>
        public void ConvertSelectedSignDataToAngle(List<string> signs)
        {
            ConvertSignData(signs.ToArray());
        }

        /// <summary>
        /// A part of training. We will convert captured data to angle with hands and fingers.
        /// Convert all sign folders to angle.
        /// </summary>        
        public void ConvertSignDataToAngle()
        {
            string[] abcArr = FileHelper.CheckSignFolders();

            ConvertSignData(abcArr);
        }

        /// <summary>
        /// A part of training. We will convert captured data to angle with hands and fingers.
        /// </summary>
        /// <param name="abcArr"> Array of sign folders will be converted </param>
        private void ConvertSignData(string[] abcArr)
        {
            foreach (string signName in abcArr)
            {
                string signFolderPath = FileHelper.captureDataFolderPath(signName);
                var allowedExtensions = new[] { ".txt" };
                string[] arrFileNames = Directory.GetFiles(signFolderPath)
                                        .Where(file => allowedExtensions.Any(file.ToLower().EndsWith))
                                        .ToArray();

                foreach (string filePath in arrFileNames)
                {
                    if (!filePath.Contains("angle"))
                    {
                        ConvertToAngle(filePath);
                    }
                }

                string trainResultPath = signFolderPath + "\\TrainResult";

                if (!Directory.Exists(trainResultPath))
                {
                    Directory.CreateDirectory(trainResultPath);
                }

                trainResultPath = trainResultPath + "\\trainResult" + signName + ".txt";

                bool isHaveTrainResult = File.Exists(trainResultPath);

                JObject trainObj = null;
                if (isHaveTrainResult)
                {
                    File.Delete(trainResultPath);
                }

                trainObj = new JObject();
                var angleAllowedExtensions = new[] { "angle.txt" };
                string[] arrAngleFileNames = Directory.GetFiles(signFolderPath)
                                        .Where(file => allowedExtensions.Any(file.ToLower().EndsWith))
                                        .ToArray();

                int count = 0;
                foreach (string angleFilePath in arrAngleFileNames)
                {
                    if (angleFilePath.Contains("angle"))
                    {
                        string text = File.ReadAllText(angleFilePath);
                        JObject jObj = JObject.Parse(text);
                        JObject leftHand = (JObject)jObj.GetValue("leftHand");
                        JObject rightHand = (JObject)jObj.GetValue("rightHand");

                        if (!trainObj.HasValues && count == 0)
                        {
                            if (leftHand != null)
                            {
                                TrainFirstTime(leftHand, trainObj);
                                if (rightHand != null)
                                {
                                    TrainAngle(rightHand, trainObj);
                                }
                            }
                            else if (rightHand != null)
                            {
                                TrainFirstTime(rightHand, trainObj);
                                if (leftHand != null)
                                {
                                    TrainAngle(leftHand, trainObj);
                                }
                            }
                        }
                        else
                        {
                            if (leftHand != null)
                            {
                                TrainAngle(leftHand, trainObj);
                            }

                            if (rightHand != null)
                            {
                                TrainAngle(rightHand, trainObj);
                            }
                        }
                        count++;
                    }
                }

                List<string> keys = trainObj.Properties().Select(p => p.Name).ToList();
                foreach (string key in keys)
                {
                    float totalAngle = (float)trainObj[key];
                    trainObj[key] = totalAngle / count;
                }


                if (trainObj.HasValues)
                {
                    Debug.WriteLine("Begin save train rsult");
                    // pass in true to cause writes to be appended
                    StreamWriter sw = new StreamWriter(trainResultPath, true);

                    // use writeline so we get a newline character written
                    sw.Write(trainObj.ToString());

                    // Ensure data is written to disk
                    sw.Close();
                }
            }
        }


        /// <summary>
        /// First time compare angle with trained data to create a result JSON object
        /// </summary>
        /// <param name="angleJSONObj">Angle JSON data </param>
        /// <param name="trainObj">Trained JSON data </param>
        void TrainFirstTime(JObject angleJSONObj, JObject trainObj)
        {
            List<string> keys = angleJSONObj.Properties().Select(p => p.Name).ToList();

            foreach (string key in keys)
            {
                trainObj.Add(key, angleJSONObj.GetValue(key));   
            }
        }

        /// <summary>
        /// Compare angle with trained data to create a result JSON object
        /// </summary>
        /// <param name="angleJSONObj">Angle JSON data </param>
        /// <param name="trainObj">Trained JSON data</param>
        void TrainAngle(JObject angleJSONObj, JObject trainObj)
        {
            List<string> keys = angleJSONObj.Properties().Select(p => p.Name).ToList();

            foreach (string key in keys)
            {
                float trainAngle = (float)trainObj.GetValue(key);
                float dataAngle = (float)angleJSONObj.GetValue(key);

                trainAngle += dataAngle;
                trainObj[key] = trainAngle;
            }

        }

        /// <summary>
        /// Convert a captured data from a file path to angle data.
        /// </summary>
        /// <param name="filePath">Capture data file path</param>
        public void ConvertToAngle(string filePath)
        {

            string folderPath = Path.GetDirectoryName(filePath);
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            string txtFilePath = folderPath + "\\" + fileName + "_angle.txt";

            if (File.Exists(txtFilePath))
            {
                File.Delete(txtFilePath);
            }

            string text = System.IO.File.ReadAllText(filePath);
            JObject jObj = JObject.Parse(text);
            LMFrame frame = new LMFrame(jObj);
            JObject convertObj = angleJSONObjectFromLMFrame(frame);

            Debug.WriteLine("Begin save");
            // pass in true to cause writes to be appended
            StreamWriter sw = new StreamWriter(txtFilePath, true);

            // use writeline so we get a newline character written
            sw.WriteLine(convertObj.ToString());

            // Ensure data is written to disk
            sw.Close();
        }

        /// <summary>
        /// Convert to angle JSON data from a LMFrame object
        /// </summary>
        /// <param name="frame">A LMFrame object</param>
        /// <returns></returns>
        public JObject angleJSONObjectFromLMFrame(LMFrame frame)
        {
            JObject convertObj = new JObject();
            foreach (LMHand hand in frame.Hands)
            {
                JObject handObj = new JObject();
                foreach (LMFinger finger in hand.Fingers)
                {
                    // angle between bone 
                    LMBone metaBone = null;
                    LMBone proBone = null;
                    LMBone interBone = null;
                    LMBone distalBone = null;

                    foreach (LMBone bone in finger.Bones)
                    {
                        if (bone.BoneType.Equals(Bone.BoneType.TYPE_METACARPAL.ToString()))
                        {
                            metaBone = bone;
                        }
                        else if (bone.BoneType.Equals(Bone.BoneType.TYPE_PROXIMAL.ToString()))
                        {
                            proBone = bone;
                        }
                        else if (bone.BoneType.Equals(Bone.BoneType.TYPE_INTERMEDIATE.ToString()))
                        {
                            interBone = bone;
                        }
                        else if (bone.BoneType.Equals(Bone.BoneType.TYPE_DISTAL.ToString()))
                        {
                            distalBone = bone;
                        }
                    }

                    // distance between bone with hand center

                    // angle between point
                    if (!finger.FingerType.Equals(Finger.FingerType.TYPE_THUMB.ToString()))
                    {
                        string metaPointProKey = finger.FingerType + "_" + metaBone.BoneType + "_" + proBone.BoneType + "_point_angle";
                        handObj.Add(metaPointProKey, AngleBpointBetween3Point(metaBone.Start, metaBone.End, proBone.End));
                    }

                    string proPointInterKey = finger.FingerType + "_" + proBone.BoneType + "_" + interBone.BoneType + "_point_angle";
                    handObj.Add(proPointInterKey, AngleBpointBetween3Point(proBone.Start, proBone.End, interBone.End));

                    string interPointDistalKey = finger.FingerType + "_" + interBone.BoneType + "_" + distalBone.BoneType + "_point_ angle";
                    handObj.Add(interPointDistalKey, AngleBpointBetween3Point(interBone.Start, interBone.End, distalBone.End));

                    // angle hand center and bone

                    string handPointProBone = finger.FingerType + "_" + proBone.BoneType + "_HAND_CENTER_angle";
                    handObj.Add(handPointProBone, AngleBpointBetween3Point(proBone.Start, hand.PalmPosition, proBone.End));

                    string handPointInterBone = finger.FingerType + "_" + interBone.BoneType + "_HAND_CENTER_angle";
                    handObj.Add(handPointInterBone, AngleBpointBetween3Point(interBone.Start, hand.PalmPosition, interBone.End));

                    string handPointDistalBone = finger.FingerType + "_" + distalBone.BoneType + "_HAND_CENTER_angle";
                    handObj.Add(handPointDistalBone, AngleBpointBetween3Point(distalBone.Start, hand.PalmPosition, distalBone.End));
                }

                if (hand.IsLeft)
                {
                    convertObj.Add("leftHand", handObj);
                }
                else
                {
                    convertObj.Add("rightHand", handObj);
                }
            }

            return convertObj;
        }



        public void DeleteSelectedSign(List<string> selectedSigns)
        {
            foreach (string signName in selectedSigns)
            {
                string signFolderPath = FileHelper.captureDataFolderPath(signName);
                Directory.Delete(signFolderPath, true);
            }
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

            return degress;
        }

        /// <summary>
        /// The angle between three point: A, B, C.
        /// It's is angle value of BA and BC.
        /// </summary>
        /// <param name="A">A point</param>
        /// <param name="B">B point</param>
        /// <param name="C">C point</param>
        /// <returns>Returns the the angle in degrees between vector BA and vector BC</returns>
        public double AngleBpointBetween3Point(Vector A, Vector B, Vector C)
        {
            // BA
            Vector v1 = new Vector(A.x - B.x, A.y - B.y, A.z - B.z);

            // BC
            Vector v2 = new Vector(C.x - B.x, C.y - B.y, C.z - B.z);

            // angle between BA and BC
            float v1Mag = (float)Math.Sqrt(v1.x * v1.x + v1.y * v1.y + v1.z * v1.z);
            Vector v1Norm = new Vector(v1.x / v1Mag, v1.y / v1Mag, v1.z / v1Mag);

            float v2Mag = (float)Math.Sqrt(v2.x * v2.x + v2.y * v2.y + v2.z * v2.z);
            Vector v2Norm = new Vector(v2.x / v2Mag, v2.y / v2Mag, v2.z / v2Mag);

            float res = v1Norm.x * v2Norm.x + v1Norm.y * v2Norm.y + v1Norm.z * v2Norm.z;

            double radian = Math.Acos(res);

            double degress = radian * (180 / Math.PI);

            return degress;
        }

        /// <summary>
        /// Calculate distance between 2 points
        /// </summary>
        /// <param name="v1">Point 1</param>
        /// <param name="v2">Point 2</param>
        /// <returns>Distance between point 1 and 2</returns>
        public double DistanceBetween(Vector v1, Vector v2)
        {
            double squareValue = (v1.x - v2.x) * (v1.x - v2.x)
                                + (v1.y - v2.y) * (v1.y - v2.y)
                                + (v1.z - v2.z) * (v1.z - v2.z);

            return Math.Sqrt(squareValue);
        }
    }
}