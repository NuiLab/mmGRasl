using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeapMotionGestureTraining.Model
{
    class LMFinger
    {
        public int FingerID { get; set; }
        public string FingerType { get; set; }
        public float Width { get; set; }
        public float Length { get; set; }
        public List<LMBone> Bones {get; set;}

        public LMFinger(int fingerId, string fingerType, float width, float length, List<LMBone> bones)
        {
            FingerID = fingerId;
            FingerType = fingerType;
            Width = width;
            Length = length;
            Bones = bones;
        }

        public LMFinger (JObject obj)
        {
            FingerID = (int) obj.GetValue("fingerId");
            FingerType = (string)obj.GetValue("fingerType");
            Width = (float)obj.GetValue("width");
            Length = (float)obj.GetValue("length");

            Bones = new List<LMBone>();
            foreach (JObject boneObj in (JArray)obj["bones"] )
            {
                LMBone bone = new LMBone(boneObj);
                Bones.Add(bone);
            }
        }

        public JObject ToJSON()
        {
            JObject obj = new JObject();
            obj.Add("fingerId", FingerID);
            obj.Add("fingerType", FingerType);
            obj.Add("width", Width);
            obj.Add("length", Length);

            JArray arrBones = new JArray();
            foreach (LMBone bone in Bones)
            {
                arrBones.Add(bone.ToJSON());
            }
            obj.Add("bones", arrBones);

            return obj;
        }
    }
}
