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

    class LMBone
    {

        public string BoneType { get; set; }
        public Vector Start { get; set; } // prevJoint
        public Vector End { get; set; } // nextJoint
        public Vector Direction { get; set; }

        public LMBone(string boneType, Vector start, Vector end, Vector direction)
        {
            BoneType = boneType;
            Start = start;
            End = end;
            Direction = direction;
        }

        public LMBone (JObject obj)
        {
            BoneType = (string )obj.GetValue("boneType");
            Start = JSONHelper.vectorFromJArray((JArray)obj["start"]);
            End = JSONHelper.vectorFromJArray((JArray)obj["end"]);
            Direction = JSONHelper.vectorFromJArray((JArray)obj["direction"]);
        }

        public JObject ToJSON()
        {
            JObject obj = new JObject();
            obj.Add("boneType", BoneType);
            obj.Add("start", JSONHelper.arrayFromVector(Start));
            obj.Add("end", JSONHelper.arrayFromVector(End));
            obj.Add("direction", JSONHelper.arrayFromVector(Direction));

            return obj;
        }
    }
}
