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
    public partial class UserProfileActivity : PhoneApplicationPage
    {

        private Ultility tvodUltility;

        private String responseResult;
        private Boolean success;

        private Boolean logout_success;

        private LogoutClass logoutObject;

        private AccountClass account;

        private string cacheCookieFile = "tvod_cookie.txt";
        private string folder = "TVOD";


        public UserProfileActivity()
        {
            InitializeComponent();
            setUpApplicationBar();
            enableProgressBar();
        }

        private void enableProgressBar()
        {
            pBar.Visibility = Visibility.Visible;
            btnGiaHan.IsEnabled = false;
            btnNapTien.IsEnabled = false;
            btnLogout.IsEnabled = false;
        }

        private void disableProgressBar()
        {
            pBar.Visibility = Visibility.Collapsed;
            btnGiaHan.IsEnabled = true;
            btnNapTien.IsEnabled = true;
            btnLogout.IsEnabled = true;
        }

        public void setUpApplicationBar()
        {
            ApplicationBar = new ApplicationBar();
            ApplicationBar.IsVisible = true;

            ApplicationBarIconButton btnBack = new ApplicationBarIconButton(new Uri("/icon/back.png", UriKind.Relative));
            btnBack.Text = "Back";
            ApplicationBarIconButton btnHome = new ApplicationBarIconButton(new Uri("/icon/home.png", UriKind.Relative));
            btnHome.Text = "Home";
            ApplicationBarIconButton btnUserProfile = new ApplicationBarIconButton(new Uri("/icon/user_profile.png", UriKind.Relative));
            btnUserProfile.Text = "Người dùng";
            ApplicationBarIconButton btnSearch = new ApplicationBarIconButton(new Uri("/icon/search.png", UriKind.Relative));
            btnSearch.Text = "Tìm kiếm";

            ApplicationBar.Buttons.Add(btnBack);
            ApplicationBar.Buttons.Add(btnHome);
            ApplicationBar.Buttons.Add(btnUserProfile);
            ApplicationBar.Buttons.Add(btnSearch);

            btnUserProfile.Click += new EventHandler(btnUserProfile_Click);
            btnHome.Click += new EventHandler(btnHome_Click);
            btnBack.Click += new EventHandler(btnBack_Click);

        }


        private void btnBack_Click(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        private void btnUserProfile_Click(object sender, EventArgs e)
        {
            try
            {
                tvodUltility = new Ultility();
                bool isLogin = tvodUltility.checkLogin();
                if (isLogin == true)
                {

                    NavigationService.Navigate(new Uri("/UserProfileActivity.xaml", UriKind.Relative));
                }
                else
                {
                    (App.Current as App).navigation_tvod = "MAIN_PAGE";
                    NavigationService.Navigate(new Uri("/LoginActivity.xaml", UriKind.Relative));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            BindingUserProfile_ServerData();
        }

        public void parseJSONUserProfile(String jsonUserProfile){
             try
            {
                var userProfileJSONObj = JsonConvert.DeserializeObject<AccountClass>(jsonUserProfile);
                if (userProfileJSONObj.success == true)
                {
                    account = new AccountClass();
                    this.success = userProfileJSONObj.success ;
                    account.uid = userProfileJSONObj.uid;
                    account.name = userProfileJSONObj.name;
                    account.email = userProfileJSONObj.email;
                    account.balance = userProfileJSONObj.balance;
                    account.subcriber = userProfileJSONObj.subcriber;
                    account.success = userProfileJSONObj.success;
                    account.group_user = userProfileJSONObj.group_user;
                }
                else
                {
                    if (MessageBox.Show("Thông tin người dùng không hợp lệ!","Thông báo", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    {
                        NavigationService.GoBack();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }


        public void BindingUserProfile_ServerData()
        {
            string cmsURL = SystemParameter.REQUEST_USER_PROFILE;
         
            WebClient client = new WebClient();
            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client_user_profile_DownloadStringCompleted);

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
                else
                {

                }
            }

            client.DownloadStringAsync(new Uri(cmsURL));
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

        void client_user_profile_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                string data = e.Result;
                this.responseResult = data;
                parseJSONUserProfile(this.responseResult);
                if (this.success)
                {
                    lblAccountName.Text = account.name;
                    lblBalance.Text = account.balance;
                    lblExpiredDate.Text = account.subcriber;
                    lblPriceSet.Text = account.group_user;
                }
                else
                {
                    if (MessageBox.Show("Thông tin người dùng không hợp lệ!", "Thông báo", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    {
                        NavigationService.GoBack();
                    }
                }
                disableProgressBar();
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

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            
            Logout_Action();
        }

        void Logout_Action()
        {
            string cmsURL = SystemParameter.REQUEST_LOGOUT;
            enableProgressBar();
            WebClient client = new WebClient();
            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client_logout_DownloadStringCompleted);

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

            client.DownloadStringAsync(new Uri(cmsURL));
        }

        void client_logout_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                string data = e.Result;
                Boolean delete_cookie = false;
                this.responseResult = data;
                parseJSONLogout(this.responseResult);
                if (this.logout_success)
                {
                    /* Xoa du lieu session */
                    if ((App.Current as App).cookie != null && !(App.Current as App).cookie.Equals(""))
                    {
                        (App.Current as App).cookie = null; ;
                    }
                    else
                    {
                        delete_cookie = deleteCookieFromIsolatedStorage();
                    }
                    if (delete_cookie)
                    {
                        if (MessageBox.Show("Đã đăng xuất thành công khỏi hệ thống", "Thông báo", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                        {
                            this.lblAccountName.Text = "";
                            this.lblBalance.Text = "";
                            this.lblExpiredDate.Text = "";
                            this.lblPriceSet.Text = "";
                            
                            NavigationService.GoBack();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Ứng dụng hoạt động không hợp lệ! Xin hãy thực hiện lại");
                    }
                }
                else
                {
                    MessageBox.Show("Ứng dụng hoạt động không hợp lệ! Xin hãy thực hiện lại");
                }
                disableProgressBar();
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

        Boolean deleteCookieFromIsolatedStorage()
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
                    SW.WriteLine("");
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

        void parseJSONLogout(String jsonLogout)
        {
            try
            {
                var userProfileJSONObj = JsonConvert.DeserializeObject<LogoutClass>(jsonLogout);
                if (userProfileJSONObj.success == true)
                {
                    logoutObject = new LogoutClass();
                    logoutObject.success = userProfileJSONObj.success;

                    this.logout_success = true;
                }
                else
                {
                    if (MessageBox.Show("Thông tin người dùng không hợp lệ!", "Thông báo", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    {
                        NavigationService.GoBack();
                    }
                    this.logout_success = false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

    }
}