using LeapMotionGestureTraining.Helper;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeapMotionGestureTraining.AppSettings
{
    class AppSetting
    {
        public static string appSettingFileName = "AppSetting.txt";
        public static string settingKeyCompareDownPercent = "compare_down_percent";
        public static string settingKeyCompareTopPercent = "compare_top_percent";

        public static JObject GetAppSetting()
        {
            string appSettingPath = FileHelper.appSettingFolderPath();
            string appSettingFilePath = appSettingPath + "\\"+ appSettingFileName; 

            if (File.Exists(appSettingFilePath))
            {
                string text = File.ReadAllText(appSettingFilePath);
                JObject settingObj = JObject.Parse(text);

                return settingObj;
            }
            else
            {
                CheckAndCreateIfHaveNoAppSetting();

                return GetAppSetting();
            }          

        }

        public static void CheckAndCreateIfHaveNoAppSetting()
        {
            string appSettingPath = FileHelper.appSettingFolderPath();
            string appSettingFilePath = appSettingPath + "\\" + appSettingFileName;

            if (!File.Exists(appSettingFilePath))
            {
                JObject settObj = new JObject();
                settObj.Add(settingKeyCompareDownPercent, 75);
                settObj.Add(settingKeyCompareTopPercent, 125);
                FileHelper.saveFile(appSettingFilePath, settObj.ToString());
            }
        }      
    }
}
