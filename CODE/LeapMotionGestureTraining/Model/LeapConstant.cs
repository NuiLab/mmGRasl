using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeapMotionGestureTraining.Model
{
    class LeapConstant
    {

        /**
 * The constant pi as a single precision floating point number.
 * @since 1.0
 */
        public static float PI = 3.1415926536f;
        /**
         * The constant ratio to convert an angle measure from degrees to radians.
         * Multiply a value in degrees by this constant to convert to radians.
         * @since 1.0
         */
        public static float DEG_TO_RAD = 0.0174532925f;
        /**
         * The constant ratio to convert an angle measure from radians to degrees.
         * Multiply a value in radians by this constant to convert to degrees.
         * @since 1.0
         */
        public static float RAD_TO_DEG = 57.295779513f;

        /**
        * The difference between 1 and the least value greater than 1 that is
        * representable as a float.
        * @since 2.0
*/
        public static float EPSILON = 1.192092896e-07f;


        public static string abcStr = "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";

    }
}
