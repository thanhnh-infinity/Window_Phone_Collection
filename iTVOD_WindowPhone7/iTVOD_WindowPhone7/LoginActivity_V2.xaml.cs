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
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using System.Diagnostics;
using iTVOD_WindowPhone7.TVOD.TVODClass;
using Newtonsoft.Json;
using System.IO;
using System.IO.IsolatedStorage;
using iTVOD_WindowPhone7.TVOD.System;
using Microsoft.Phone.Shell;


namespace iTVOD_WindowPhone7
{
    public partial class LoginActivity_V2 : ChildWindow
    {

        private WebClient clienLogin;

        /** Properties **/
        private string user_name;
        private string password;
        private bool success;
        // private string sessionID;


        /** Cookie **/
        private string cookie;


        //URL for Image File in Internet
        private string cacheCookieFile = "tvod_cookie.txt";
        //private string ImageFileName = null;
        private string folder = "TVOD";

        public LoginActivity_V2()
        {
            InitializeComponent();
            pBar.Visibility = Visibility.Collapsed;
            
            if ((App.Current as App).user_name != null && !(App.Current as App).user_name.Equals(""))
            {
                txtUserName.Text = (App.Current as App).user_name;
            }

            if ((App.Current as App).password != null && !(App.Current as App).password.Equals(""))
            {
                txtPassword.Password = (App.Current as App).password;
            }

            String username = getUserNameFromIsolatedStorage();
            if (username != null && !username.Equals(""))
            {
                txtUserName.Text = username.Trim().ToString();
            }
            String password = getPassFromIsolatedStorage();
            if (password != null && !password.Equals(""))
            {
                txtPassword.Password = password.Trim().ToString();
            }
        }

        protected override void OnOpened()
        {
            base.OnOpened();
            if (txtUserName.Text == "" && txtPassword.Password == "")
            {
                txtUserName.Focus();
            }

            if (txtUserName.Text != "" && txtPassword.Password != "")
            {
                btnLogin.Focus();
            }
        }

        private void LoginAction(string user_name, string password)
        {
            clienLogin = new WebClient();
            enableProgressBar();
            string cmsLogin = SystemParameter.REQUEST_LOGIN;
            cmsLogin = cmsLogin.Replace("%u", user_name);
            cmsLogin = cmsLogin.Replace("%p", password);
            clienLogin = new WebClient();
            clienLogin.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client_Login_DownloadStringCompleted);
            clienLogin.DownloadStringAsync(new Uri(cmsLogin.ToString()));
        }


        void client_Login_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            string data = e.Result;
            parseJSONLogin(data);
        }

        private void parseJSONLogin(string jsonLogin)
        {
            try
            {
                var URLJSONObj = JsonConvert.DeserializeObject<LoginClass>(jsonLogin);
                if (URLJSONObj.success == true)
                {
                    this.cookie = "SESS9ff1de06eab787efbcbfae72f0c03323=" + URLJSONObj.sessionID;
                    (App.Current as App).cookie = this.cookie;
                    saveCookieToIsolatedStorage(this.cookie);
                    this.success = true;
                    (App.Current as App).user_name = this.user_name;
                    (App.Current as App).password = this.password;

                    /** Save user name , password and Cookie **/
                    SaveCredentials(this.user_name, this.password);
                    disableProgressBar();
                    //NavigationService.GoBack();
                    base.Close();
                }
                else
                {
                    MessageBox.Show("Không thể login vào hệ thống. Sai username hoặc password");
                    disableProgressBar();
                    txtUserName.Focus();
                    return;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }


        }

        public string getPassFromIsolatedStorage()
        {
            try
            {
                IsolatedStorageFile File = IsolatedStorageFile.GetUserStoreForApplication();
                string sFile = folder + "\\pass.txt";
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

        public string getUserNameFromIsolatedStorage()
        {
            try
            {
                IsolatedStorageFile File = IsolatedStorageFile.GetUserStoreForApplication();
                string sFile = folder + "\\user.txt";
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

        public bool SaveCredentials(String username, String password)
        {
            try
            {
                IsolatedStorageFile ISF = IsolatedStorageFile.GetUserStoreForApplication();

                if (!ISF.DirectoryExists(folder))
                {
                    ISF.CreateDirectory(folder);
                }
                using (StreamWriter SW = new StreamWriter(new IsolatedStorageFileStream(folder + "\\user.txt", FileMode.Create, FileAccess.Write, ISF)))
                {
                    SW.WriteLine(username);
                    SW.Close();
                }
                using (StreamWriter SW = new StreamWriter(new IsolatedStorageFileStream(folder + "\\pass.txt", FileMode.Create, FileAccess.Write, ISF)))
                {
                    SW.WriteLine(password);
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


        public bool saveCookieToIsolatedStorage(string cookie)
        {
            try
            {
                IsolatedStorageFile ISF = IsolatedStorageFile.GetUserStoreForApplication();

                if (!ISF.DirectoryExists(folder))
                {
                    ISF.CreateDirectory(folder);
                }
                using (StreamWriter SW = new StreamWriter(new IsolatedStorageFileStream(folder + "\\" + this.cacheCookieFile, FileMode.Create, FileAccess.Write, ISF)))
                {
                    SW.WriteLine(cookie);
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

        private void enableProgressBar()
        {
            btnLogin.IsEnabled = false;
            pBar.Visibility = Visibility.Visible;
            txtPassword.IsEnabled = false;
            txtUserName.IsEnabled = false;
        }

        private void disableProgressBar()
        {
            btnLogin.IsEnabled = true;
            pBar.Visibility = Visibility.Collapsed;
            txtPassword.IsEnabled = true;
            txtUserName.IsEnabled = true;
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (txtUserName.Text == "")
            {
                MessageBox.Show("Xin hãy nhập tên đăng nhập !");
                txtUserName.Focus();
                return;
            }
            if (txtPassword.Password == "")
            {
                MessageBox.Show("Xin hãy nhập mật khẩu !");
                txtPassword.Focus();
                return;
            }

            string user_name = txtUserName.Text.Trim();
            string password = txtPassword.Password.Trim();
            this.user_name = user_name;
            this.password = password;
            LoginAction(user_name, password);
            //this.DialogResult = true;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {

            base.Close();
        }
    }
}