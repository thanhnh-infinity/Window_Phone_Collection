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
using System.Windows.Data;

namespace iTVOD_WindowPhone7
{
    public partial class List_Video_Follow_Child_Category : PhoneApplicationPage
    {
        /** Dieu khien scoll View cua ung dung **/
        ScrollViewer scrollViewer;

        /** Tham so chuyen dong cua ung dung **/
        private bool fixCacheImage = false;

        /** Properties **/
        ObservableCollection<VideoClass> ds ;
        private List<VideoClass> listVideo = new List<VideoClass>();
        private VideoClass videoObj = new VideoClass();
        private int numberVideo = 0;
        private int numberVideoPerPage = 0;
        
        //private string folder = "TVOD";
        //private String childCategoryFile = "";
        private string responseResult = "";
        private ImageCache imgCache;

        /** Properties get from previous activity **/
        private String child_category_id;
        private String child_category_name;
        private Ultility tvodUltility;
        private int current_page = 1;
        private String current_filter = "newest";
        private int total_video = 0;

        /** Progress Bar Indicator **/
        ProgressIndicator prog;


        //Cache Cookie File
        private string cacheCookieFile = "tvod_cookie.txt";
        private string folder = "TVOD";

        public List_Video_Follow_Child_Category()
        {
            InitializeComponent();
            imgCache = new ImageCache();
            tvodUltility = new Ultility();
            setUpApplicationBar();
            enableProgressBar();

            btnNewest.Background = new SolidColorBrush(Colors.White);
            btnNewest.Foreground = new SolidColorBrush(Colors.Black);

            this.lstVideo_Follow_Category.Loaded += new RoutedEventHandler(lstVideo_Follow_Category_Loaded);
            
        }

        public void enableProgressIndicator()
        {
            prog = new ProgressIndicator();
            prog.IsVisible = true;
            prog.IsIndeterminate = true;
            prog.Text = "Xin hãy đợi trong giây lát";
            btnMostView.IsEnabled = false;
            btnNewest.IsEnabled = false;
            SystemTray.SetProgressIndicator(this, prog);
        }

        public void disableProgressIndicator()
        {
            prog.IsVisible = false;
            btnMostView.IsEnabled = true;
            btnNewest.IsEnabled = true;
        }
      

        private void enableProgressBar()
        {
            if (ds == null)
            {
                ds = new ObservableCollection<VideoClass>();
                
            }
            ds.Add(new VideoClass() { video_picture_path = "http", video_english_title = "Loading.............", video_vietnamese_title = "", video_number_views = "", video_id = "", video_price="0.00"});
            this.lstVideo_Follow_Category.ItemsSource = ds;
            btnMostView.IsEnabled = false;
            btnNewest.IsEnabled = false;
        }

        private void disableProgressBar()
        {
            btnMostView.IsEnabled = true;
            btnNewest.IsEnabled = true;
        }

        public void BindingDataVideo_ServerData_Paging(string filter, string page)
        {
            try
            {
                string cmsURL = SystemParameter.REQUEST_VIDEO_FOLLOW_CATEGORY_PAGE;
                cmsURL = cmsURL.Replace("%s", this.child_category_id);
                cmsURL = cmsURL.Replace("%d", filter);
                cmsURL = cmsURL.Replace("%p", page);
                this.current_page = Int32.Parse(page);
                if ((this.current_page > 1) && ((this.current_page - 1) * SystemParameter.NUMBER_ITEMS_PER_PAGE < this.total_video))
                {
                    WebClient client = new WebClient();

                    if ((App.Current as App).cookie != null && !(App.Current as App).cookie.Equals(""))
                    {
                        client.Headers["Cookie"] = (App.Current as App).cookie;
                    }
                    else
                    {
                        string old_cookie = getCookieFromIsolatedStorage();
                        if (old_cookie != null && old_cookie != "" && old_cookie != "\r\n")
                        {
                            client.Headers["Cookie"] = old_cookie;
                        }
                    }

                    client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client_2_DownloadStringCompleted);
                    client.DownloadStringAsync(new Uri(cmsURL));
                }
                else
                {
                    ds.Add(new VideoClass() { video_picture_path = "http", video_english_title = "Đã hết dữ liệu", video_vietnamese_title = "", video_number_views = "", video_id = "", video_price="0.00"});
                    this.lstVideo_Follow_Category.ItemsSource = ds;
                    disableProgressIndicator();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        void client_2_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            string data = e.Result;
           // if (this.responseResult.Equals("") || this.responseResult == null)
           // {
                this.responseResult = data;
           // }
            //bool result = tvodUltility.writeDataToFile(this.responseResult, this.folder, this.childCategoryFile);

            parseJSONVideoList(this.responseResult);
            for (int i = 0; i < numberVideoPerPage; i++)
            {
                videoObj = new VideoClass();
                videoObj = listVideo[i];
                
                //string source_image = imgCache.getImage(parentCategoryObj.category_image);
                string source_image = "";
                if (fixCacheImage == true)
                {
                    //source_image = imgCache.getImage_2(childCategoryObj.category_id);
                }
                else //Day la hoan thien nhat tuy nhien can thoi gian de fix data ( cache Image)
                {
                    source_image = imgCache.getImage(videoObj.video_picture_path);
                    if (source_image == null || source_image == string.Empty || source_image == "")
                    {
                        source_image = "http";
                    }
                }

                ds.Add(new VideoClass() { video_picture_path = source_image, video_english_title = videoObj.video_english_title, video_vietnamese_title = videoObj.video_vietnamese_title, video_number_views = videoObj.video_number_views, video_id = videoObj.video_id, video_price = videoObj.video_price});
            }

            this.lstVideo_Follow_Category.ItemsSource = ds;
            disableProgressIndicator();
           
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


        public void BindingDataVideo_ServerData(string filter, string page)
        {
            string cmsURL = SystemParameter.REQUEST_VIDEO_FOLLOW_CATEGORY_PAGE;
            cmsURL = cmsURL.Replace("%s",this.child_category_id);
            cmsURL = cmsURL.Replace("%d", filter);
            cmsURL = cmsURL.Replace("%p", page);
            this.current_filter = filter;
            this.current_page = 1;
            WebClient client = new WebClient();

            if ((App.Current as App).cookie != null && !(App.Current as App).cookie.Equals(""))
            {
                client.Headers["Cookie"] = (App.Current as App).cookie;
            }
            else
            {
                string old_cookie = getCookieFromIsolatedStorage();
                if (old_cookie != null && old_cookie != "" && old_cookie != "\r\n")
                {
                    client.Headers["Cookie"] = old_cookie;
                }
            }

            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client_1_DownloadStringCompleted);
            client.DownloadStringAsync(new Uri(cmsURL));
        }

        void client_1_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                string data = e.Result;
                //if (this.responseResult.Equals("") || this.responseResult == null)
                //{
                this.responseResult = data;
                //}
                //bool result = tvodUltility.writeDataToFile(this.responseResult, this.folder, this.childCategoryFile);

                parseJSONVideoList(this.responseResult);
                ds = new ObservableCollection<VideoClass>();
                ds.Clear();
                if (this.numberVideo > 0)
                {
                    for (int i = 0; i < numberVideoPerPage; i++)
                    {
                        videoObj = new VideoClass();
                        videoObj = listVideo[i];
                        //string source_image = imgCache.getImage(parentCategoryObj.category_image);
                        string source_image = "";
                        if (fixCacheImage == true)
                        {
                            //source_image = imgCache.getImage_2(childCategoryObj.category_id);
                        }
                        else //Day la hoan thien nhat tuy nhien can thoi gian de fix data ( cache Image)
                        {
                            source_image = imgCache.getImage(videoObj.video_picture_path);
                        }

                        ds.Add(new VideoClass() { video_picture_path = source_image, video_english_title = videoObj.video_english_title, video_vietnamese_title = videoObj.video_vietnamese_title, video_number_views = videoObj.video_number_views, video_id = videoObj.video_id, video_price = videoObj.video_price});
                    }
                    this.lstVideo_Follow_Category.ItemsSource = ds;
                    disableProgressBar();
                }
                else
                {
                    ds.Add(new VideoClass() { video_picture_path = "http", video_english_title = "Không có dữ liệu", video_vietnamese_title = "", video_number_views = "", video_id = "", video_price = "0.00" });
                    this.lstVideo_Follow_Category.ItemsSource = ds;
                    disableProgressBar();
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

        private void parseJSONVideoList(String jsonVideoList)
        {
            try
            {
                var videoListJSONObj = JsonConvert.DeserializeObject<RootVideoClass>(jsonVideoList);
                if (videoListJSONObj.success == true || videoListJSONObj.type == "video")
                {
                    this.total_video = Int32.Parse(videoListJSONObj.total_quantity);
                    listVideo = new List<VideoClass>();
                    foreach (var obj in videoListJSONObj.items)
                    {
                        videoObj = new VideoClass();
                        videoObj.video_id = obj.video_id;
                        videoObj.video_description = obj.video_description;
                        videoObj.video_duration = obj.video_duration;
                        videoObj.video_english_title = obj.video_english_title;
                        videoObj.video_number_views = "Số lượt xem : " + obj.video_number_views;
                        videoObj.video_picture_path = obj.video_picture_path;
                        videoObj.video_price = obj.video_price;
                        videoObj.video_vietnamese_title = obj.video_vietnamese_title;
                        listVideo.Add(videoObj);
                       
                    }
                    this.numberVideoPerPage = Int16.Parse(videoListJSONObj.quantity);
                    this.numberVideo = Int16.Parse(videoListJSONObj.total_quantity);
                }
                else
                {
                    videoObj = new VideoClass();
                    videoObj.video_english_title = "Không có dữ liệu !";
                    this.numberVideoPerPage = 0;
                    this.numberVideo = 0;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }

        /** Set Up Application Bar **/
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
                    LoginActivity_V2 loginWindow = new LoginActivity_V2();
                    loginWindow.Show();
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

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            (App.Current as App).loadData = false;
            base.OnNavigatedFrom(e);
        }
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            if ((App.Current as App).loadData == true)
            {
                base.OnNavigatedTo(e);
                string child_category_id = "";
                string child_category_name = "";
                string msg = "";
                if (NavigationContext.QueryString.TryGetValue("child_category_id", out msg))
                {
                    child_category_id = msg;
                    this.child_category_id = child_category_id;
                }
                if (NavigationContext.QueryString.TryGetValue("child_category_name", out msg))
                {
                    child_category_name = msg;
                    this.child_category_name = child_category_name;
                }

                lblCategory_Name.Text = child_category_name;
           
                BindingDataVideo_ServerData("newest","1");
            } 
            else 
            {
                (App.Current as App).loadData = true;
                base.OnNavigatedTo(e);
            }
        }

        /**Request server to know got new data in API or not : No new Data : Get Data From IsolatedStorage, Co data moi : Get From APIs */
        private bool isGetDataFromFile()
        {
            /** Request to Server to get data new or old */
            return false;
        }

        private void btnNewest_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                listVideo = new List<VideoClass>();
                ds.Clear();
                enableProgressBar();
                BindingDataVideo_ServerData("newest", "1");
                btnMostView.Background = new SolidColorBrush(Colors.Black);
                btnMostView.Foreground = new SolidColorBrush(Colors.White);
                btnNewest.Background = new SolidColorBrush(Colors.White);
                btnNewest.Foreground = new SolidColorBrush(Colors.Black);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void btnMostView_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                listVideo = new List<VideoClass>();
                ds.Clear();
                enableProgressBar();
                BindingDataVideo_ServerData("most_view", "1");
                btnMostView.Background = new SolidColorBrush(Colors.White);
                btnMostView.Foreground = new SolidColorBrush(Colors.Black);
                btnNewest.Background = new SolidColorBrush(Colors.Black);
                btnNewest.Foreground = new SolidColorBrush(Colors.White);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void lstVideo_Follow_Category_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                VideoClass videoObjTemp = (sender as ListBox).SelectedItem as VideoClass;
                NavigationService.Navigate(new Uri("/Video_Detail_Player_V2.xaml?video_id=" + videoObjTemp.video_id + "&video_english_title=" + videoObjTemp.video_english_title + "&video_vietnamese_title=" + videoObjTemp.video_vietnamese_title + "&video_picture_path=" + videoObjTemp.video_picture_path + "&video_price=" + videoObjTemp.video_price + "&video_description=" + videoObjTemp.video_english_title, UriKind.Relative));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        /**************************************************************************************************/
        /** Giai quyet van de phan trang **/
        void lstVideo_Follow_Category_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                FrameworkElement element = (FrameworkElement)sender;
                element.Loaded -= lstVideo_Follow_Category_Loaded;
                scrollViewer = FindChildOfType<ScrollViewer>(element);
                if (scrollViewer == null)
                {
                    throw new InvalidOperationException("ScrollViewer not found.");
                }

                Binding binding = new Binding();
                binding.Source = scrollViewer;
                binding.Path = new PropertyPath("VerticalOffset");
                binding.Mode = BindingMode.OneWay;
                this.SetBinding(ListVerticalOffsetProperty, binding);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public static void OnListVerticalOffsetChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            List_Video_Follow_Child_Category page = obj as List_Video_Follow_Child_Category;
            ScrollViewer viewer = page.scrollViewer;

            //Checks if the Scroll has reached the last item based on the ScrollableHeight
            bool atBottom = viewer.VerticalOffset >= viewer.ScrollableHeight;

            if (atBottom)
            {
                page.enableProgressIndicator();
                //MessageBox.Show("Đang tải dữ liệu trang " + (page.current_page + 1).ToString());
                page.BindingDataVideo_ServerData_Paging(page.current_filter, (page.current_page+1).ToString());
                //BindingDataVideo_ServerData_Paging("newest", "2");
            }
        }

        public readonly DependencyProperty ListVerticalOffsetProperty = DependencyProperty.Register("ListVerticalOffset", typeof(double), typeof(List_Video_Follow_Child_Category),
           new PropertyMetadata(new PropertyChangedCallback(OnListVerticalOffsetChanged)));

        public double ListVerticalOffset
        {
            get { return (double)this.GetValue(ListVerticalOffsetProperty); }
            set { this.SetValue(ListVerticalOffsetProperty, value); }
        }

        /// <summary>
        /// Finding the ScrollViewer
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="root"></param>
        /// <returns></returns>
        static T FindChildOfType<T>(DependencyObject root) where T : class
        {
            var queue = new Queue<DependencyObject>();
            queue.Enqueue(root);

            while (queue.Count > 0)
            {
                DependencyObject current = queue.Dequeue();
                for (int i = VisualTreeHelper.GetChildrenCount(current) - 1; 0 <= i; i--)
                {
                    var child = VisualTreeHelper.GetChild(current, i);
                    var typedChild = child as T;
                    if (typedChild != null)
                    {
                        return typedChild;
                    }
                    queue.Enqueue(child);
                }
            }
            return null;
        }
    }
}