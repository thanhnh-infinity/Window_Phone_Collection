using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.IO.IsolatedStorage;
using iTVOD_WindowPhone7.TVOD.System;
using System.IO;
using System.Diagnostics;
using Newtonsoft.Json;
using iTVOD_WindowPhone7.TVOD.TVODClass;
using System.Windows.Navigation;

namespace iTVOD_WindowPhone7
{
    public partial class PurchaseExpiredDateActivity : ChildWindow
    {
        private string cacheCookieFile = "tvod_cookie.txt";
        private string folder = "TVOD";
        private Ultility tvodUltility;
        private String responseResult;
        private Boolean success;
        private PackageClass packageClass;
        private String packageName;

        public PurchaseExpiredDateActivity()
        {
            InitializeComponent();
            disabelProgressBar();
        }

        private void enableProgressBar()
        {
            pBar.Visibility = Visibility.Visible;
            btnPurchase10Days.IsEnabled = false;
            btnPurchase30Days.IsEnabled = false;
            btnPurchase3Days.IsEnabled = false;
        }

        private void disabelProgressBar()
        {
            pBar.Visibility = Visibility.Collapsed;
            btnPurchase10Days.IsEnabled = true;
            btnPurchase30Days.IsEnabled = true;
            btnPurchase3Days.IsEnabled = true;
        }

        private void btnPurchase3Days_Click(object sender, RoutedEventArgs e)
        {
            enableProgressBar();
            Request_Purchase_Package_Service(1);

        }

        public void Request_Purchase_Package_Service(int type)
        {
            string cmsURL = SystemParameter.REQUEST_PURCHASE_PACKAGE_SERVICE;
            cmsURL = cmsURL.Replace("%t", type.ToString());
            cmsURL = cmsURL + "&time=" + DateTime.Now;
            if (type == 1)
            {
                this.packageName = "Basic 1";
            }
            if (type == 2)
            {
                this.packageName = "Basic 2";
            }
            if (type == 3)
            {
                this.packageName = "Basic 3";
            }

            WebClient client = new WebClient();
            if ((App.Current as App).cookie != null && !(App.Current as App).cookie.Equals(""))
            {
                client.Headers["Cookie"] = (App.Current as App).cookie;
            }
            else
            {
                string old_cookie = getCookieFromIsolatedStorage();
                if (old_cookie != null && !old_cookie.Equals("") && old_cookie != "\r\n")
                {
                    client.Headers["Cookie"] = old_cookie;
                }
            }

            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client_purchase_package_service_DownloadStringCompleted);
            client.DownloadStringAsync(new Uri(cmsURL));
        }

        void client_purchase_package_service_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                string data = e.Result;
                this.responseResult = data;
                parseJSONPurchasePackage(this.responseResult);
                if (this.success)
                {
                    MessageBox.Show("Bạn đã mua gói dịch vụ " + this.packageName + " thành công");
                }
                else
                {
                    MessageBox.Show(packageClass.type + "---" + packageClass.reason);
                }
            
            }
            catch (WebException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }


        }

        public void parseJSONPurchasePackage(String jsonPackage)
        {
            try
            {
                var packageJSONObj = JsonConvert.DeserializeObject<PackageClass>(jsonPackage);
                if (packageJSONObj.success == true)
                {
                    packageClass = new PackageClass();
                    this.success = true;
                    packageClass.success = packageJSONObj.success;
                    packageClass.content = packageJSONObj.content;
                    packageClass.reason = packageJSONObj.reason;
                    packageClass.type = packageJSONObj.type;
                    
                }
                else
                {
                    MessageBox.Show(packageJSONObj.type + "----" + packageJSONObj.reason);
                }
                disabelProgressBar();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public string getCookieFromIsolatedStorage()
        {
            try
            {
                IsolatedStorageFile File = IsolatedStorageFile.GetUserStoreForApplication();
                string sFile = folder + "\\" + cacheCookieFile;
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

        private void btnPurchase10Days_Click(object sender, RoutedEventArgs e)
        {
            enableProgressBar();
            Request_Purchase_Package_Service(2);
        }

        private void btnPurchase30Days_Click(object sender, RoutedEventArgs e)
        {
            enableProgressBar();
            Request_Purchase_Package_Service(3);
        }

    }
}