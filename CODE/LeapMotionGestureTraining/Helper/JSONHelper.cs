using System;
using System.Text;
using Leap;
using Newtonsoft.Json.Linq;
using System.Drawing;
using System.IO;
using LeapMotionGestureTraining.Model;

namespace LeapMotionGestureTraining.Helper
{
    class JSONHelper
    {

        /// <summary>
        /// Generate vector from JSON Array
        /// </summary>
        /// <param name="arrVector">JSON Array</param>
        /// <returns></returns>
        public static Vector vectorFromJArray(JArray arrVector)
        {
            Vector vector = new Vector(float.Parse((string)arrVector[0]),
                                        float.Parse((string)arrVector[1]),
                                        float.Parse((string)arrVector[2]));

            return vector;
        }

        /// <summary>
        /// Generate JSON array from vector
        /// </summary>
        /// <param name="vector">Vector</param>
        /// <returns></returns>
        public static JArray arrayFromVector(Vector vector)
        {
            string arrStr = stringFromVector(vector);
            return JArray.Parse(arrStr);
        }

        /// <summary>
        /// Change vector.ToString() to JSON Array tyle
        /// </summary>
        /// <param name="vector">Vector</param>
        /// <returns></returns>
        public static string stringFromVector(Vector vector)
        {
            string vectorStr = vector.ToString();
            vectorStr = vectorStr.Replace("(", "[");
            vectorStr = vectorStr.Replace(")", "]");
            return vectorStr;
        }

    }
}
