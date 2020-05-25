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
    public partial class ListChannel_Follow_Child_Category : PhoneApplicationPage
    {

        /** Dieu khien scoll View cua ung dung **/
        ScrollViewer scrollViewer;


        /** Tham so chuyen dong cua ung dung **/
        private bool fixCacheImage = false;

        /** Properties **/
        ObservableCollection<LiveChannelClass> ds;
        private List<LiveChannelClass> listLiveChannel = new List<LiveChannelClass>();
        private LiveChannelClass liveChannelObj = new LiveChannelClass();
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
        private int total_channel = 0;

        public ListChannel_Follow_Child_Category()
        {
            InitializeComponent();
            imgCache = new ImageCache();
            tvodUltility = new Ultility();
            setUpApplicationBar();
            enableProgressBar();
            this.lstLiveChannel_Follow_Category.Loaded += new RoutedEventHandler(lstLiveChannel_Follow_Category_Loaded);
        }

        private void enableProgressBar()
        {
            if (ds == null)
            {
                ds = new ObservableCollection<LiveChannelClass>();
            }
            ds.Add(new LiveChannelClass() { video_picture_path = "http", live_channel_title = "Loading..............", live_channel_id = "", live_channel_folder = "", live_channel_url = "" });
            this.lstLiveChannel_Follow_Category.ItemsSource = ds;
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

            BindingDataLiveChannel_ServerData("newest", "1");
        }

        /**Request server to know got new data in API or not : No new Data : Get Data From IsolatedStorage, Co data moi : Get From APIs */
        private bool isGetDataFromFile()
        {
            /** Request to Server to get data new or old */
            return false;
        }


        public void BindingDataLiveChannel_ServerData(string filter, string page)
        {
            string cmsURL = SystemParameter.REQUEST_VIDEO_FOLLOW_CATEGORY_PAGE;
            cmsURL = cmsURL.Replace("%s", this.child_category_id);
            cmsURL = cmsURL.Replace("%d", filter);
            cmsURL = cmsURL.Replace("%p", page);
            this.current_filter = filter;
            this.current_page = 1;

            WebClient client = new WebClient();
            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client_1_DownloadStringCompleted);
            client.DownloadStringAsync(new Uri(cmsURL));
        }

        void client_1_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            string data = e.Result;
           
            this.responseResult = data;
           
            //bool result = tvodUltility.writeDataToFile(this.responseResult, this.folder, this.childCategoryFile);

            parseJSONLiveChannelList(this.responseResult);
            ds = new ObservableCollection<LiveChannelClass>();
            ds.Clear();
            if (this.numberVideo > 0)
            {
                for (int i = 0; i < numberVideoPerPage; i++)
                {
                    liveChannelObj = new LiveChannelClass();
                    liveChannelObj = listLiveChannel[i];
                    //string source_image = imgCache.getImage(parentCategoryObj.category_image);
                    string source_image = "";
                    if (fixCacheImage == true)
                    {
                        source_image = imgCache.getImage_2(liveChannelObj.live_channel_id);
                    }
                    else //Day la hoan thien nhat tuy nhien can thoi gian de fix data ( cache Image)
                    {
                        source_image = imgCache.getImage(liveChannelObj.video_picture_path);
                    }

                    ds.Add(new LiveChannelClass() { video_picture_path = source_image, live_channel_title = liveChannelObj.live_channel_title, live_channel_id = liveChannelObj.live_channel_id,live_channel_folder=liveChannelObj.live_channel_folder,live_channel_url = liveChannelObj.live_channel_url });
                }
                this.lstLiveChannel_Follow_Category.ItemsSource = ds;
            }
            else
            {
                ds.Add(new LiveChannelClass() { video_picture_path = "http", live_channel_title = "Không có dữ liệu", live_channel_id = "" });
                this.lstLiveChannel_Follow_Category.ItemsSource = ds;
            }
        }

        private void parseJSONLiveChannelList(String jsonLiveChannelList)
        {
            try
            {
                var liveChannelListJSONObj = JsonConvert.DeserializeObject<RootLiveChannelClass>(jsonLiveChannelList);
                if (liveChannelListJSONObj.success == true || liveChannelListJSONObj.type == "live_channel")
                {
                    this.total_channel = Int32.Parse(liveChannelListJSONObj.total_quantity);
                    listLiveChannel = new List<LiveChannelClass>();

                    foreach (var obj in liveChannelListJSONObj.items)
                    {
                        liveChannelObj = new LiveChannelClass();
                        liveChannelObj.live_channel_id = obj.live_channel_id;
                        liveChannelObj.live_channel_folder = obj.live_channel_folder;
                        liveChannelObj.live_channel_number_view = obj.live_channel_number_view;
                        liveChannelObj.live_channel_title = obj.live_channel_title;
                        liveChannelObj.live_channel_url = obj.live_channel_url;
                        liveChannelObj.video_picture_path = obj.video_picture_path;
                        listLiveChannel.Add(liveChannelObj);

                    }
                    this.numberVideoPerPage = Int16.Parse(liveChannelListJSONObj.quantity);
                    this.numberVideo = Int16.Parse(liveChannelListJSONObj.total_quantity);
                }
                else
                {
                    liveChannelObj = new LiveChannelClass();
                    liveChannelObj.live_channel_title = "Không có dữ liệu !";
                    this.numberVideoPerPage = 0;
                    this.numberVideo = 0;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }

        private void lstLiveChannel_Follow_Category_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                LiveChannelClass liveChannelObj = (sender as ListBox).SelectedItem as LiveChannelClass;
                NavigationService.Navigate(new Uri("/Live_Channel_Player.xaml?live_channel_id=" + liveChannelObj.live_channel_id + "&live_channel_url=" + liveChannelObj.live_channel_url + "&live_channel_folder=" + liveChannelObj.live_channel_folder, UriKind.Relative));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        /**************************************************************************************************/
        /** Giai quyet van de phan trang **/

        public void BindingDataLiveChannel_ServerData_Paging(string filter, string page)
        {
            try
            {
                string cmsURL = SystemParameter.REQUEST_VIDEO_FOLLOW_CATEGORY_PAGE;
                cmsURL = cmsURL.Replace("%s", this.child_category_id);
                cmsURL = cmsURL.Replace("%d", filter);
                cmsURL = cmsURL.Replace("%p", page);
                this.current_page = Int32.Parse(page);
                if ((this.current_page > 1) && ((this.current_page - 1) * 25 < this.total_channel))
                {

                    WebClient client = new WebClient();
                    client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client_paging_DownloadStringCompleted);
                    client.DownloadStringAsync(new Uri(cmsURL));
                }
                else
                {
                    ds.Add(new LiveChannelClass() { video_picture_path = "http", live_channel_title = "         Không có dữ liệu", live_channel_id = "" });
                    this.lstLiveChannel_Follow_Category.ItemsSource = ds;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        void client_paging_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            string data = e.Result;

            this.responseResult = data;

            //bool result = tvodUltility.writeDataToFile(this.responseResult, this.folder, this.childCategoryFile);

            parseJSONLiveChannelList(this.responseResult);
            if (this.numberVideo > 0)
            {
                for (int i = 0; i < numberVideoPerPage; i++)
                {
                    liveChannelObj = new LiveChannelClass();
                    liveChannelObj = listLiveChannel[i];
                    //string source_image = imgCache.getImage(parentCategoryObj.category_image);
                    string source_image = "";
                    if (fixCacheImage == true)
                    {
                        source_image = imgCache.getImage_2(liveChannelObj.live_channel_id);
                    }
                    else //Day la hoan thien nhat tuy nhien can thoi gian de fix data ( cache Image)
                    {
                        source_image = imgCache.getImage(liveChannelObj.video_picture_path);
                    }

                    ds.Add(new LiveChannelClass() { video_picture_path = source_image, live_channel_title = liveChannelObj.live_channel_title, live_channel_id = liveChannelObj.live_channel_id, live_channel_folder = liveChannelObj.live_channel_folder, live_channel_url = liveChannelObj.live_channel_url });
                }
                this.lstLiveChannel_Follow_Category.ItemsSource = ds;
            }
            else
            {
                ds.Add(new LiveChannelClass() { video_picture_path = "http", live_channel_title = "Không có dữ liệu", live_channel_id = "" });
                this.lstLiveChannel_Follow_Category.ItemsSource = ds;
            }
        }

        void lstLiveChannel_Follow_Category_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                FrameworkElement element = (FrameworkElement)sender;
                element.Loaded -= lstLiveChannel_Follow_Category_Loaded;
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
            ListChannel_Follow_Child_Category page = obj as ListChannel_Follow_Child_Category;
            ScrollViewer viewer = page.scrollViewer;

            //Checks if the Scroll has reached the last item based on the ScrollableHeight
            bool atBottom = viewer.VerticalOffset >= viewer.ScrollableHeight;

            if (atBottom)
            {
                MessageBox.Show("Đang tải dữ liệu trang " + (page.current_page + 1).ToString());
                page.BindingDataLiveChannel_ServerData_Paging(page.current_filter, (page.current_page + 1).ToString());
            }
        }

        public readonly DependencyProperty ListVerticalOffsetProperty = DependencyProperty.Register("ListVerticalOffset", typeof(double), typeof(ListChannel_Follow_Child_Category),
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