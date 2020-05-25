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
using Microsoft.Phone.Tasks;

namespace iTVOD_WindowPhone7
{
    public partial class Video_Detail_Player_V2 : PhoneApplicationPage
    {
        /** Private properties **/
        private string video_id;
        private string video_english_title;
        private string video_vietnamese_title;
        private string video_picture_path;
        private string resultGetURL;
        private string video_url;


        //URL for Image File in Internet
        private string cacheCookieFile = "tvod_cookie.txt";
        private string ImageFileName = null;
        private string folder = "TVOD";


        //HTTP Client is used to request
        WebClient webClient;
        WebClient clientURL;

        private Ultility tvodUltility;

        //Cookie
        private string cookie;

        public Video_Detail_Player_V2()
        {
            InitializeComponent();
            disableProgressBar();
            setUpApplicationBar();

            SupportedOrientations = SupportedPageOrientation.Portrait | SupportedPageOrientation.Portrait;

            webClient = new WebClient();
            

            //Request to server to get Image Material
            webClient.OpenReadCompleted += (s1, e1) =>
            {
                if (e1.Error == null)
                {
                    try
                    {
                        bool isSpaceAvailable = IsSpaceIsAvailable(e1.Result.Length);
                        if (isSpaceAvailable)
                        {
                            //Save File To Isolated Storage
                            using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                            {
                                if (!myIsolatedStorage.DirectoryExists(folder))
                                {
                                    myIsolatedStorage.CreateDirectory(folder);
                                }

                                using (IsolatedStorageFileStream isfs = new IsolatedStorageFileStream(folder + "\\" + ImageFileName, FileMode.Create, FileAccess.Write, myIsolatedStorage))
                                {

                                    long imgLen = e1.Result.Length;
                                    byte[] b = new byte[imgLen];
                                    e1.Result.Read(b, 0, b.Length);
                                    isfs.Write(b, 0, b.Length);
                                    isfs.Flush();
                                    isfs.Close();


                                }
                            }

                            LoadImageFromIsolatedStorage(folder + "\\" + ImageFileName);

                        }
                        else
                        {
                            BitmapImage bmpImg = new BitmapImage();
                            bmpImg.SetSource(e1.Result);
                            imgVideo.Source = bmpImg;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }

            };
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

            btnBack.Click += new EventHandler(btnBack_Click);
            btnHome.Click += new EventHandler(btnHome_Click);
            btnUserProfile.Click += new EventHandler(btnUserProfile_Click);
            btnSearch.Click += new EventHandler(btnSearch_Click);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/SearchActivity.xaml", UriKind.Relative));
        }

        private void btnUserProfile_Click(object sender, EventArgs e)
        {
            try
            {
                tvodUltility = new Ultility();
                bool isLogin = tvodUltility.checkLogin();
                if (isLogin == true)
                {

                    NavigationService.Navigate(new Uri("/UserProfileActivity_V2.xaml", UriKind.Relative));
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


        private void btnBack_Click(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            string video_id = "";
            string video_english_title = "";
            string video_vietnamese_title = "";
           // string video_picture_path;
           
            string msg = "";
            if (NavigationContext.QueryString.TryGetValue("video_id", out msg))
            {
                video_id = msg;
            }
            if (NavigationContext.QueryString.TryGetValue("video_english_title", out msg))
            {
                video_english_title = msg;
            }
            if (NavigationContext.QueryString.TryGetValue("video_vietnamese_title", out msg))
            {
                video_vietnamese_title = msg;
            }
            if (NavigationContext.QueryString.TryGetValue("video_picture_path", out msg))
            {
                this.video_picture_path = msg;
            }

            this.video_id = video_id;
            this.video_english_title = video_english_title;
            this.video_vietnamese_title = video_vietnamese_title;

            lblVideoTitle.Text = this.video_english_title;
            lblEnglishTitle.Text = this.video_english_title;
            lblVietnameseTitle.Text = this.video_vietnamese_title;

            Uri uri = new Uri(this.video_picture_path);
            ImageFileName = uri.AbsolutePath.Replace("/", "_");
            ImageFileName = ImageFileName.Replace("%20", "_");
            ImageFileName = ImageFileName.Replace(" ", "_");


            disableProgressBar();

            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                string fullFileName = folder + "\\" + ImageFileName;
                bool fileExist = isf.FileExists(fullFileName);
                if (fileExist)
                {
                    LoadImageFromIsolatedStorage(fullFileName);
                }
                else
                {
                    if (!string.IsNullOrEmpty(this.video_picture_path))
                    {
                        webClient.OpenReadAsync(new Uri(this.video_picture_path));
                    }
                }
            }

        }


        private bool IsSpaceIsAvailable(long spaceSeq)
        {
            using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                long spaceAvail = store.AvailableFreeSpace;
                if (spaceAvail < spaceSeq)
                {
                    return false;
                }
                return true;
            }
        }

        private void LoadImageFromIsolatedStorage(string imageFileName)
        {
            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (IsolatedStorageFileStream isoStream = isf.OpenFile(imageFileName, FileMode.Open))
                {
                    BitmapImage bmpImg = new BitmapImage();
                    bmpImg.SetSource(isoStream);
                    imgVideo.Source = bmpImg;
                }
            }
        }

        private void btnViewVideo_Click(object sender, RoutedEventArgs e)
        {

            getURL();

        }

        private void getURL()
        {
            enableProgressBar();
            /** URL **/
            string cmsURl = SystemParameter.REQUEST_VIDEO_MOBILE_URL;
            cmsURl = cmsURl.Replace("%s", this.video_id);

            clientURL = new WebClient();

            if (this.cookie != null && !this.cookie.Equals(""))
            {
                clientURL.Headers["Cookie"] = this.cookie;
            }
            else
            {
                if ((App.Current as App).cookie != null && !(App.Current as App).cookie.Equals(""))
                {
                    clientURL.Headers["Cookie"] = (App.Current as App).cookie;
                }
                else
                {
                    string old_cookie = getCookieFromIsolatedStorage();
                    if (old_cookie != null && old_cookie != "" && old_cookie != "\r\n")
                    {
                        clientURL.Headers["Cookie"] = old_cookie;
                    }
                }

            }
            
            clientURL.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client_getURL_DownloadStringCompleted);
            clientURL.DownloadStringAsync(new Uri(cmsURl.ToString()));
            
        }

        void client_getURL_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                string data = e.Result;
                this.resultGetURL = data;
                parseJSONGetURL(this.resultGetURL);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }


        private void parseJSONGetURL(string jsonGetURL)
        {
            try
            {
                var URLJSONObj = JsonConvert.DeserializeObject<URLObject>(jsonGetURL);
                if (URLJSONObj.success == true)
                {
                    this.video_url = URLJSONObj.url;
                    if (this.video_url != null && !this.video_url.Equals(""))
                    {
                       
                        MediaPlayerLauncher player = new MediaPlayerLauncher();
                        player.Media = new Uri(this.video_url);
                        player.Show();
                        
                    }
                    else
                    {
                        MessageBox.Show("File MP4 được gửi từ Server bị lỗi !");
                        
                    }
                    //disableProgressBar();
                }
                else
                {
                    disableProgressBar();
                    if (URLJSONObj.type != null && URLJSONObj.type.Equals(SystemParameter.ERROR_CODE_FROM_SERVER_NO_AUTHORIZED))
                    {
                        //MessageBox.Show("Chưa đăng nhập. Bạn có muốn đăng nhập vào hệ thống ?");
                        if (MessageBox.Show("Chưa đăng nhập. Bạn có muốn đăng nhập vào hệ thống ?", "Thông Báo", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                        {
                            (App.Current as App).navigation_tvod = "DETAIL_VIDEO";
                            NavigationService.Navigate(new Uri("/LoginActivity.xaml", UriKind.Relative));
                        }
                    }
                    else
                    {
                        MessageBox.Show(URLJSONObj.type + ":" + URLJSONObj.reason);
                    }

                    if ((App.Current as App).user_name != null && (App.Current as App).password != null)
                    {
                        string cmsLogin = SystemParameter.REQUEST_LOGIN;
                        cmsLogin = cmsLogin.Replace("%u", (App.Current as App).user_name);
                        cmsLogin = cmsLogin.Replace("%p", (App.Current as App).password);
                        clientURL = new WebClient();
                        clientURL.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client_Login_getURL_DownloadStringCompleted);
                        clientURL.DownloadStringAsync(new Uri(cmsLogin.ToString()));
                    }
                     
                    
                    
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }


        }

        void client_Login_getURL_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
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
                }
                else
                {
                    MessageBox.Show("Không thể login vào hệ thống. Sai username hoặc password");

                }
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


        private void enableProgressBar()
        {
            pBar.Visibility = Visibility.Visible;
            btnViewVideo.IsEnabled = false;
            btnSendGift.IsEnabled = false;
        }

        
        private void disableProgressBar()
        {
            pBar.Visibility = Visibility.Collapsed;
            btnViewVideo.IsEnabled = true;
            btnSendGift.IsEnabled = true;
            
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

        private void btnViewVideo_Click_1(object sender, RoutedEventArgs e)
        {
            getURL();
        }

    }
    
}