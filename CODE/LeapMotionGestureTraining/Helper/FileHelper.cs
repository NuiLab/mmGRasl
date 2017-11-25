using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using Ionic.Zip;
using LeapMotionGestureTraining.Model;
using System.Diagnostics;

namespace LeapMotionGestureTraining.Helper
{
    class FileHelper
    {
        public static void zipFile(string txtPath, string zipPath)
        {
            using (ZipFile zip = new ZipFile())
            {
                zip.AddFile(txtPath);
                zip.Save(zipPath);
            }
        }

        public static void zipFileSaveInFolder(string txtPath)
        {
            string extractPath = Path.GetDirectoryName(txtPath);
            string fileName = Path.GetFileNameWithoutExtension(txtPath);
            zipFile(txtPath, extractPath + "\\" + fileName + ".zip");
        }

        public static string txtFilePathFromUnzip(string zipToUnpack)
        {
            string zipFolderPath = Path.GetDirectoryName(zipToUnpack);
            string zipName = Path.GetFileNameWithoutExtension(zipToUnpack);
            string txtFileExtractFolderPath = zipFolderPath + "\\Unzip";

            unzipFile(zipToUnpack, txtFileExtractFolderPath);

            return txtFileExtractFolderPath + "\\" + zipName + ".txt";
        }

        public static void unzipFile(string zipToUnpack, string unpackDirectory)
        {
            using (ZipFile zip1 = ZipFile.Read(zipToUnpack))
            {
                // here, we extract every entry, but we could extract conditionally
                // based on entry name, size, date, checkbox status, etc.  
                foreach (ZipEntry e in zip1.ToList<ZipEntry>())
                {
                    e.FileName = System.IO.Path.GetFileName(e.FileName);
                    e.Extract(unpackDirectory, ExtractExistingFileAction.OverwriteSilently);
                }
            }
        }


        #region Folder app creation
        public static string[] CheckSignFolders()
        {
            string startupPath = captureFolderPath();
            string[] subDirectories;

            try
            {
                subDirectories = Directory.GetDirectories(startupPath);
            }
            catch (Exception e)
            {
                FileHelper.saveDebugString(e.Message);
                subDirectories = new string[0];
            }

            for (int i = 0; i < subDirectories.Length; i++)
            {
                subDirectories[i] = subDirectories[i].Remove(0, startupPath.Length + 1);
            }

            return subDirectories;
        }

        public static void createABCFolderForFirstTime()
        {
            string startupPath = captureFolderPath();
            string[] abcArr = LeapConstant.abcStr.Split(',');

            for (int i = 0; i < abcArr.Length; i++)
            {
                string folderName = startupPath + "\\" + abcArr[i];
                if (!Directory.Exists(folderName))
                {
                    Directory.CreateDirectory(folderName);
                }
            }
        }

        public static bool CreateCustomSignFolder(string folderName)
        {
            bool isCreate = false;
            string startupPath = captureFolderPath();
            string customFolderPath = startupPath + "\\" + folderName;

            if (!Directory.Exists(customFolderPath))
            {
                isCreate = true;
                Directory.CreateDirectory(customFolderPath);
                LMSingleton.Instance.UpdateSignFolders();
            }

            return isCreate;
        }

        #endregion

        #region File Path
        public static string captureFolderPath()
        {
            string startupPath = Directory.GetCurrentDirectory();
            startupPath = startupPath + "\\Capture";

            return startupPath;
        }

        public static string captureDataFolderPath(string humanSign)
        {
            string startupPath = System.IO.Directory.GetCurrentDirectory();
            startupPath += @"\Capture\" + humanSign;
            if (!Directory.Exists(startupPath))
            {
                Directory.CreateDirectory(startupPath);
            }

            return startupPath;
        }


        public static void saveFileToTestFolder(string fileName, string fileSource)
        {
            string startupPath = TestFolderPath();

            string txtFilePath = startupPath + @"\" + fileName + ".txt";
            StreamWriter sw = new StreamWriter(txtFilePath, true);

            // use writeline so we get a newline character written
            sw.WriteLine(fileSource);

            // Ensure data is written to disk
            sw.Close();
        }

        public static void saveFile(string filePath, string fileSource)
        {
            StreamWriter sw = new StreamWriter(filePath, true);

            // use writeline so we get a newline character written
            sw.WriteLine(fileSource);

            // Ensure data is written to disk
            sw.Close();
        }

        public static void saveDebugString(string debugStr)
        {
            string debugPath = DebugFolderPath();
            debugPath = debugPath + "\\debug.txt";
            StreamWriter sw = new StreamWriter(debugPath, true);

            // use writeline so we get a newline character written  
            debugStr += "-----------------------------\\n";
            sw.WriteLine(debugStr);

            // Ensure data is written to disk
            sw.Close();
        }

        public static string TestFolderPath(string sign)
        {
            string startupPath = TestFolderPath();
            startupPath = startupPath + "\\" + sign;

            if (!Directory.Exists(startupPath))
            {
                Directory.CreateDirectory(startupPath);
            }

            return startupPath;
        }

        public static string TestFolderPath()
        {
            string startupPath = System.IO.Directory.GetCurrentDirectory();
            startupPath += @"\TestFolder";
            if (!Directory.Exists(startupPath))
            {
                Directory.CreateDirectory(startupPath);
            }

            return startupPath;
        }

        public static string testDataFolderPath()
        {
            string startupPath = System.IO.Directory.GetCurrentDirectory();
            startupPath += @"\TestData";
            if (!Directory.Exists(startupPath))
            {
                Directory.CreateDirectory(startupPath);
            }

            return startupPath;
        }

        public static string appSettingFolderPath()
        {
            string startupPath = System.IO.Directory.GetCurrentDirectory();
            startupPath += @"\AppSetting";
            if (!Directory.Exists(startupPath))
            {
                Directory.CreateDirectory(startupPath);
            }

            return startupPath;
        }

        public static string interpreterFolderPath()
        {
            string startupPath = System.IO.Directory.GetCurrentDirectory();
            startupPath += @"\Interpreter";
            if (!Directory.Exists(startupPath))
            {
                Directory.CreateDirectory(startupPath);
            }

            return startupPath;
        }

        public static string DebugFolderPath()
        {
            string startupPath = System.IO.Directory.GetCurrentDirectory();
            startupPath += @"\Debugs";
            if (!Directory.Exists(startupPath))
            {
                Directory.CreateDirectory(startupPath);
            }

            return startupPath;
        }

        #endregion
    }
}