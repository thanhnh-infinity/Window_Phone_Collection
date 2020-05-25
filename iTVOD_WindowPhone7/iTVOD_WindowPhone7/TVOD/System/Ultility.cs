using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO;
using System.IO.IsolatedStorage;
using System.Diagnostics;

namespace iTVOD_WindowPhone7.TVOD.System
{
    public class Ultility
    {
        public bool checkLogin()
        {
            string cookie = (App.Current as App).cookie;

            if (cookie != null && !cookie.Equals("") && !cookie.Equals("\r\n"))
            {
                return true;
            }
            else
            {
                cookie = getCookieFromIsolatedStorage();
                if (cookie != null && !cookie.Equals("") && !cookie.Equals("\r\n"))
                {
                    return true;
                }
                return false;
            }
            //return false;
        }

        public string getCookieFromIsolatedStorage()
        {
            try
            {
                IsolatedStorageFile File = IsolatedStorageFile.GetUserStoreForApplication();
                string sFile = "TVOD\\tvod_cookie.txt";
                bool fileExist = File.FileExists(sFile);
                string rawData = "";
                if (fileExist == true)
                {
                    StreamReader reader = new StreamReader(new IsolatedStorageFileStream(sFile, FileMode.Open, File));
                    rawData = reader.ReadToEnd();
                    reader.Close();
                    return rawData;
                }
                else
                {
                    return "";
                }

            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex.Message);
                return "";
            }
        }

        public bool writeDataToFile(string data, string folder, string fileName)
        {
            try
            {
                IsolatedStorageFile ISF = IsolatedStorageFile.GetUserStoreForApplication();

                if (!ISF.DirectoryExists(folder))
                {
                    ISF.CreateDirectory(folder);
                }
                using (StreamWriter SW = new StreamWriter(new IsolatedStorageFileStream(folder + "\\" + fileName, FileMode.Create, FileAccess.Write, ISF)))
                {
                    SW.WriteLine(data);
                    SW.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }

        }
    }
}
