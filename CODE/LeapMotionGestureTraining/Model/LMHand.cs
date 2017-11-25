using Leap;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeapMotionGestureTraining.Helper;

namespace LeapMotionGestureTraining.Model
{
    class LMHand
    {
        public bool IsLeft { get; set; }
        public int HandId { get; set; }
        public string HandType { get; set; }
        public Vector PalmPosition { get; set; }
        public Vector PalmNormal { get; set; }
        public Vector Direction { get; set; }
        public Vector ArmDirection { get; set; }
        public Vector ArmWristPosition { get; set; }
        public Vector ArmElbowPosition { get; set; }
        public List<LMFinger> Fingers { get; set; }

        public LMHand(bool isLeft, int handId, string handType, Vector palmPosition, Vector palmNormal, Vector direction,
                        Vector armDirection, Vector armWristPosition, Vector armElbowPosition, List<LMFinger> fingers)
        {
            IsLeft = IsLeft;
            HandId = HandId;
            HandType = handType;
            PalmPosition = palmPosition;
            PalmNormal = palmNormal;
            Direction = direction;
            ArmDirection = armDirection;
            ArmWristPosition = armWristPosition;
            ArmElbowPosition = armElbowPosition;
            Fingers = fingers;
        }

        public LMHand(JObject obj)
        {
            IsLeft = (bool) obj.GetValue("isLeft");
            HandId = (int) obj.GetValue("handId");
            HandType = (string) obj.GetValue("handType");
            PalmPosition = JSONHelper.vectorFromJArray((JArray)obj["palmPosition"]); 
            PalmNormal = JSONHelper.vectorFromJArray((JArray)obj["palmNormal"]);
            Direction = JSONHelper.vectorFromJArray((JArray)obj["direction"]);
            ArmDirection = JSONHelper.vectorFromJArray((JArray)obj["armDirection"]);
            ArmWristPosition = JSONHelper.vectorFromJArray((JArray)obj["armWristPosition"]);
            ArmElbowPosition = JSONHelper.vectorFromJArray((JArray)obj["armElbowPosition"]);

            Fingers = new List<LMFinger>();
            foreach (JObject fingerObj in (JArray)obj["fingers"])
            {
                LMFinger finger = new LMFinger(fingerObj);
                Fingers.Add(finger);
            }
        }

        public JObject ToJSON()
        {
            JObject obj = new JObject();
            obj.Add("isLeft", IsLeft);
            obj.Add("handId", HandId);
            obj.Add("handType", HandType);
            obj.Add("palmPosition", JSONHelper.arrayFromVector(PalmPosition));
            obj.Add("palmNormal", JSONHelper.arrayFromVector(PalmNormal));
            obj.Add("direction", JSONHelper.arrayFromVector(Direction));
            obj.Add("armDirection", JSONHelper.arrayFromVector(ArmDirection));
            obj.Add("armWristPosition", JSONHelper.arrayFromVector(ArmWristPosition));
            obj.Add("armElbowPosition", JSONHelper.arrayFromVector(ArmElbowPosition));

            JArray arrFinger = new JArray();
            foreach (LMFinger finger in Fingers)
            {
                arrFinger.Add(finger.ToJSON());
            }
            obj.Add("fingers", arrFinger);

            return obj;
        }

    }
}
