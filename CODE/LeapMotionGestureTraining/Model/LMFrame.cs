using Leap;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeapMotionGestureTraining.Model;
using LeapMotionGestureTraining.Helper;

namespace LeapMotionGestureTraining.Model
{
    class LMFrame
    {
        public long Id { get; set; }
        public long TimeStamp { get; set; }
        public string HumanSign { get; set; }
        public System.Drawing.Image LeftCamImg { get; set; }
        public System.Drawing.Image RightCamImg { get; set; }
        public List<LMHand> Hands { get; set; }

        public static string DebugStringFromFrame(Frame frame)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("Frame ID : " + frame.Id
                            + ", timestamp : " + frame.Timestamp
                            + ", hands : " + frame.Hands.Count
                            + ", finger : " + (frame.Hands.Count * 5));
            foreach (Hand hand in frame.Hands)
            {
                string handType = hand.IsLeft ? "left hand" : "right hand";
                builder.AppendLine("  ");
                builder.AppendLine(" Hand ID : " + hand.Id
                                    + ", hand type : " + handType
                                    + ", palm position: " + hand.PalmPosition.ToString());
                builder.AppendLine(" Hand pitch : " + (hand.Direction.Pitch * LeapConstant.RAD_TO_DEG) + " degress"
                                    + ", roll : " + (hand.Direction.Roll * LeapConstant.RAD_TO_DEG) + " degress"
                                    + ", yaw : " + (hand.Direction.Yaw * LeapConstant.RAD_TO_DEG) + " degress");

                builder.AppendLine(" Arm direction: " + hand.Arm.Direction.ToString()
                                    + ", wrist position : " + hand.Arm.WristPosition.ToString()
                                    + ", elbow position : " + hand.Arm.ElbowPosition.ToString());

                foreach (Finger finger in hand.Fingers)
                {
                    builder.AppendLine("  Finger Id : " + finger.Id
                                    + ", " + finger.Type
                                    + ", length " + finger.Length
                                    + ", width " + finger.Width);

                    foreach (Bone.BoneType boneType in (Bone.BoneType[])Enum.GetValues(typeof(Bone.BoneType)))
                    {
                        Bone bone = finger.Bone(boneType);
                        builder.AppendLine("    Bone : " + bone.Type
                                        + ", start: " + bone.PrevJoint.ToString()
                                        + ", end : " + bone.NextJoint.ToString()
                                        + ", direction : " + bone.Direction.ToString());
                    }
                }
            }
            return builder.ToString();
        } 

        public string DebugString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("Frame ID : " + Id 
                            + ", timestamp : " + TimeStamp 
                            + ", hands : " + Hands.Count
                            + ", finger : " + (Hands.Count * 5));
            foreach (LMHand hand in Hands)
            {
                string handType = hand.IsLeft ? "left hand" : "right hand";
                builder.AppendLine("  ");
                builder.AppendLine(" Hand ID : " + hand.HandId
                                    + ", hand type : " + handType
                                    + ", palm position: " + hand.PalmPosition.ToString());
                builder.AppendLine(" Hand pitch : " + (hand.Direction.Pitch * LeapConstant.RAD_TO_DEG) + " degress"
                                    + ", roll : " + (hand.Direction.Roll * LeapConstant.RAD_TO_DEG) + " degress"
                                    + ", yaw : " + (hand.Direction.Yaw * LeapConstant.RAD_TO_DEG) + " degress");

                builder.AppendLine(" Arm direction: " + hand.ArmDirection.ToString()
                                    + ", wrist position : " + hand.ArmWristPosition.ToString()
                                    + ", elbow position : " + hand.ArmElbowPosition.ToString());

                foreach (LMFinger finger in hand.Fingers)
                {
                    //builder.AppendLine("==");
                    builder.AppendLine("  Finger Id : " + finger.FingerID 
                                    + ", " + finger.FingerType 
                                    + ", length " + finger.Length
                                    + ", width " + finger.Width);

                    foreach (LMBone bone in finger.Bones)
                    {
                        //builder.AppendLine("----");
                        builder.AppendLine("    Bone : " + bone.BoneType
                                        + ", start: " + bone.Start.ToString()
                                        + ", end : " + bone.End.ToString()
                                        + ", direction : " + bone.Direction.ToString());
                    }
                }
            }
            
            return builder.ToString();
        }

        public LMFrame(Frame aFrame, string humanSign)
        {
            //frame = aFrame;
            Id = aFrame.Id;
            TimeStamp = aFrame.Timestamp;
            HumanSign = humanSign;

            try
            {
                LeftCamImg = Helper.ImageHelper.generateBitmapFromLeapImage(aFrame.Images[0]);
                RightCamImg = Helper.ImageHelper.generateBitmapFromLeapImage(aFrame.Images[1]);
            }
            catch (Exception e)
            {
                FileHelper.saveDebugString("Image creatation" + e.Data.ToString());
            }

            if (LeftCamImg == null)
            {
                FileHelper.saveDebugString("LMFRame creation : " + aFrame.Images[0].ToString());
            }
            

            Hands = new List<LMHand>();
            foreach (Hand aHand in aFrame.Hands)
            {

                List<LMFinger> fingers = new List<LMFinger>();
                foreach (Finger afinger in aHand.Fingers)
                {
                    List<LMBone> bones = new List<LMBone>();
                    foreach (Bone.BoneType boneType in (Bone.BoneType[])Enum.GetValues(typeof(Bone.BoneType)))
                    {
                        Bone bone = afinger.Bone(boneType);
                        LMBone lmBone = new LMBone(boneType.ToString(), bone.PrevJoint, bone.NextJoint, bone.Direction);
                        bones.Add(lmBone);
                    }

                    LMFinger lmFinger = new LMFinger(afinger.Id, afinger.Type.ToString(), afinger.Width, afinger.Length, bones);
                    fingers.Add(lmFinger);
                }

                LMHand lmHand = new LMHand(aHand.IsLeft, aHand.Id, aHand.GetType().ToString(), aHand.PalmPosition, 
                                            aHand.PalmNormal, aHand.Direction, aHand.Arm.Direction, aHand.Arm.WristPosition,
                                            aHand.Arm.ElbowPosition, fingers);

                Hands.Add(lmHand);
            }

        }

        public LMFrame(JObject jObject)
        {
            //JObject jObject = JObject.Parse(jsonData);

            Id = (long)jObject.GetValue("id");
            TimeStamp = (long)jObject.GetValue("timeStamp");
            string leftBitmapBase64Str = (string)jObject.GetValue("leftCamImg");
            string rightBitmapBase64Str = (string)jObject.GetValue("rightCamImg");

            LeftCamImg = Helper.ImageHelper.imageFromString(leftBitmapBase64Str);
            RightCamImg = Helper.ImageHelper.imageFromString(rightBitmapBase64Str);

            Hands = new List<LMHand>();
            foreach (JObject obj in (JArray)jObject["hands"])
            {
                LMHand hand = new LMHand(obj);
                Hands.Add(hand);
            }
        }


        public JObject toJSON()
        {
            string leftBitmapBase64Str = Helper.ImageHelper.stringFromImage(LeftCamImg);
            string rightBitmapBase64Str = Helper.ImageHelper.stringFromImage(RightCamImg);

            JObject jObject = new JObject();
            jObject.Add("id", Id);
            jObject.Add("timeStamp", TimeStamp);
            jObject.Add("humanSign", HumanSign);
            jObject.Add("leftCamImg", leftBitmapBase64Str);
            jObject.Add("rightCamImg", rightBitmapBase64Str);

            JArray arrHand = new JArray();

            foreach (LMHand hand in Hands)
            {
                arrHand.Add(hand.ToJSON());
            }

            jObject.Add("hands", arrHand);

            string jObjTest = jObject.ToString();

            return jObject;

        }

        public static string debugString(Frame aFrame)
        {
            return "";
        }
    }
}
