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
using Microsoft.Web.Media.SmoothStreaming;
using iTVOD_WindowPhone7.TVOD.System;
using Newtonsoft.Json;
using iTVOD_WindowPhone7.TVOD.TVODClass;
using System.Diagnostics;
using System.IO.IsolatedStorage;
using System.IO;


namespace iTVOD_WindowPhone7
{
    public partial class Live_Channel_Player : PhoneApplicationPage
    {
        private TVOD.System.Ultility tvodUtil;
        private SessionClass sessionObj;
        private string cacheCookieFile = "tvod_cookie.txt";
        private string folder = "TVOD";
        private string sessionID;
        private string live_channel_url;
        public Live_Channel_Player()
        {
            InitializeComponent();
            //BindingData_To_Get_Session_ID();
            tvodUtil = new TVOD.System.Ultility();
           
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            (App.Current as App).loadData = false;
            base.OnNavigatedFrom(e);
        }

        public void BindingData_To_Get_Session_ID()
        {
            string cmsURL = SystemParameter.REQUEST_SESSION_ID_FOR_LIVE_STREAMING;
            
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
                else
                {

                }
            }

            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client_get_session_id_DownloadStringCompleted);
            client.DownloadStringAsync(new Uri(cmsURL));
        }

        void client_get_session_id_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                string data = e.Result;
                parseJSONGetSessionID(data);
                if (sessionObj != null && sessionObj.success == true)
                {
                    this.sessionID = sessionObj.sessionID;
                }
                else
                {
                    
                }

                live_channel_url += "/manifest?sessionID=" + this.sessionID;
                liveChannelPlayer.SmoothStreamingSource = new Uri(live_channel_url);
                
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

        private void parseJSONGetSessionID(String jsonSession)
        {
            try
            {
                var liveSessionJSONObj = JsonConvert.DeserializeObject<SessionClass>(jsonSession);
                if (liveSessionJSONObj.success == true)
                {
                    sessionObj = new SessionClass();
                    sessionObj.success = liveSessionJSONObj.success;
                    sessionObj.sessionID = liveSessionJSONObj.sessionID;
                    this.sessionID = sessionObj.sessionID;
                }
                else
                {
                    sessionObj = new SessionClass();
                    sessionObj.success = liveSessionJSONObj.success;
                    sessionObj.sessionID = "";
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

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            if (tvodUtil.checkLogin())
            {    
                base.OnNavigatedTo(e);
                BindingData_To_Get_Session_ID();
                string live_channel_id = "";
                //string live_channel_url = "";
                string live_channel_folder = "";
                string msg = "";
                if (NavigationContext.QueryString.TryGetValue("live_channel_id", out msg))
                {
                    live_channel_id = msg;
                }
                if (NavigationContext.QueryString.TryGetValue("live_channel_url", out msg))
                {
                    this.live_channel_url = msg;
                }
                if (NavigationContext.QueryString.TryGetValue("live_channel_folder", out msg))
                {
                    live_channel_folder = msg;
                }

                //live_channel_url += "/manifest?sessionID=" + this.sessionID;
                //liveChannelPlayer.SmoothStreamingSource = new Uri(live_channel_url);

            }
            else
            {
                NavigationService.GoBack();
                if (MessageBox.Show("Chưa đăng nhập. Bạn có muốn đăng nhập vào hệ thống ?", "Thông Báo", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    (App.Current as App).navigation_tvod = "MAIN_PAGE";
                    LoginActivity_V2 loginWindow = new LoginActivity_V2();
                    loginWindow.Show();
                }
            }
        }
        
        
    }
}