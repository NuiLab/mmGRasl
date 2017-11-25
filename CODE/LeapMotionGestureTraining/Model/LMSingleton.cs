using LeapMotionGestureTraining.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeapMotionGestureTraining.Model
{
    class LMSingleton
    {
        public enum FormName { Training, Testing, Interpreter}
        public float compareTopPercent { get; set; }
        public float compareDownPercent { get; set; }
        public FormName currentForm { get; set; }
        public string[] signFolders { get; set; }
        private static LMSingleton instance;
        private LMSingleton() { }

        public static LMSingleton Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new LMSingleton();
                }
                return instance;
            }
        }

        public void UpdateSignFolders()
        {
            signFolders = FileHelper.CheckSignFolders();
        }

    }
}
