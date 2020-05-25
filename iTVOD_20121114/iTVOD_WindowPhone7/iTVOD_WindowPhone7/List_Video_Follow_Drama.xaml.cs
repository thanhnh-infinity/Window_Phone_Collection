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
using System.Windows.Data;

namespace iTVOD_WindowPhone7
{
    public partial class List_Video_Follow_Drama : PhoneApplicationPage
    {

        private String drama_image_path;
        private String drama_id;
        private String drama_english_title;
        private String drama_vietnamese_title;
        private String drama_quantity;
        //HTTP Client is used to request
        WebClient webClient;
        WebClient clientURL;
        private string ImageFileName = null;
        private string folder = "TVOD";

        /** Dieu khien scoll View cua ung dung **/
        ScrollViewer scrollViewer;

        /** Tham so chuyen dong cua ung dung **/
        private bool fixCacheImage = false;

        /** Properties **/
        ObservableCollection<VideoClass> ds = new ObservableCollection<VideoClass>();
        private List<VideoClass> listVideo = new List<VideoClass>();
        private VideoClass videoObj = new VideoClass();
        private int numberVideo = 0;
        private int numberVideoPerPage = 0;

    
        private string responseResult = "";
        private ImageCache imgCache;

        /** Properties get from previous activity **/
        private int current_page = 1;
        private String current_filter = "oldest";
        private int total_video = 0;
     

        private Ultility tvodUltility;

        public List_Video_Follow_Drama()
        {
            InitializeComponent();

            //disableProgressBar();
            setUpApplicationBar();
            imgCache = new ImageCache();

            SupportedOrientations = SupportedPageOrientation.Portrait | SupportedPageOrientation.Portrait;

            webClient = new WebClient();
            btnViewFromBegin.Background = new SolidColorBrush(Colors.White);
            btnViewFromBegin.Foreground = new SolidColorBrush(Colors.Black);

            this.lstVideoFollowDrama.Loaded +=new RoutedEventHandler(lstVideoFollowDrama_Loaded);

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
                            imgDrama.Source = bmpImg;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }

            };
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
                    imgDrama.Source = bmpImg;
                }
            }
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
            string drama_id = "";
            string drama_english_title = "";
            string drama_vietnamese_title = "";
            enableProgressBar();

            string msg = "";
            if (NavigationContext.QueryString.TryGetValue("drama_id", out msg))
            {
                drama_id = msg;
            }
            if (NavigationContext.QueryString.TryGetValue("drama_english_title", out msg))
            {
                drama_english_title = msg;
            }
            if (NavigationContext.QueryString.TryGetValue("drama_vietnamese_title", out msg))
            {
                drama_vietnamese_title = msg;
            }
            if (NavigationContext.QueryString.TryGetValue("drama_image_path", out msg))
            {
                this.drama_image_path = msg;
            }
            if (NavigationContext.QueryString.TryGetValue("drama_quantity", out msg))
            {
                this.drama_quantity = msg;
            }

            this.drama_id = drama_id;
            this.drama_english_title = drama_english_title;
            this.drama_vietnamese_title = drama_vietnamese_title;

            lblDramaTitle.Text = this.drama_english_title;
            lblDramaTitle_Vietnam.Text = this.drama_vietnamese_title;
            lblQuantity.Text = this.drama_quantity;
           // tvodBrowser.Source =  new Uri("http://tvod.vn/node/" + drama_id);

            Uri uri = new Uri(this.drama_image_path);
            ImageFileName = uri.AbsolutePath.Replace("/", "_");
            ImageFileName = ImageFileName.Replace("%20", "_");
            ImageFileName = ImageFileName.Replace(" ", "_");


            //disableProgressBar();

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
                    if (!string.IsNullOrEmpty(this.drama_image_path))
                    {
                        webClient.OpenReadAsync(new Uri(this.drama_image_path));
                    }
                }
            }


            // Display video inside Drama
            BindingDataVideo_ServerData_FollowDrama(this.drama_id, "oldest", "1");

        }


        public void BindingDataVideo_ServerData_FollowDrama(string drama_id,string filter, string page)
        {
            try
            {
                string cmsURL = SystemParameter.REQUEST_VIDEO_FOLLOW_DRAMA;
                cmsURL = cmsURL.Replace("%p", page);
                cmsURL = cmsURL.Replace("%f", filter);
                cmsURL = cmsURL.Replace("%d", drama_id);
                this.current_page = Int32.Parse(page);
                this.current_filter = filter;
                WebClient client = new WebClient();
                client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client_list_video_follow_drama_DownloadStringCompleted);
                client.DownloadStringAsync(new Uri(cmsURL));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        void client_list_video_follow_drama_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                string data = e.Result;
                this.responseResult = data;

                parseJSONVideoList(this.responseResult);
                //ds = new ObservableCollection<VideoClass>();
                if (this.current_page == 1)
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

                        ds.Add(new VideoClass() { video_picture_path = source_image, video_english_title = videoObj.video_english_title, video_id = videoObj.video_id, video_vietnamese_title = videoObj.video_vietnamese_title, video_number_views = videoObj.video_number_views });
                    }
                    this.lstVideoFollowDrama.ItemsSource = ds;
                    disableProgressBar();
                }
                else
                {
                    ds.Add(new VideoClass() {video_picture_path = "http", video_english_title = "Không có dữ liệu !", video_id = "" });
                    this.lstVideoFollowDrama.ItemsSource = ds;
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

        private void lstVideoFollowDrama_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                VideoClass videoObjTemp = (sender as ListBox).SelectedItem as VideoClass;
                NavigationService.Navigate(new Uri("/Video_Detail_Player_V2.xaml?video_id=" + videoObjTemp.video_id + "&video_english_title=" + videoObjTemp.video_english_title + "&video_vietnamese_title=" + videoObjTemp.video_vietnamese_title + "&video_picture_path=" + videoObjTemp.video_picture_path, UriKind.Relative));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void btnViewFromBegin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                listVideo = new List<VideoClass>();
                ds.Clear();
                enableProgressBar();
                BindingDataVideo_ServerData_FollowDrama(this.drama_id,"oldest", "1");
                btnViewFromBegin.Background = new SolidColorBrush(Colors.White);
                btnViewFromBegin.Foreground = new SolidColorBrush(Colors.Black);
                btnViewFromEnd.Background = new SolidColorBrush(Colors.Black);
                btnViewFromEnd.Foreground = new SolidColorBrush(Colors.White);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void enableProgressBar()
        {
            if (ds == null)
            {
                ds = new ObservableCollection<VideoClass>();

            }
            ds.Add(new VideoClass() { video_picture_path = "http", video_english_title = "Loading.............", video_vietnamese_title = "", video_number_views = "", video_id = "" });
            this.lstVideoFollowDrama.ItemsSource = ds;
            btnViewFromBegin.IsEnabled = false;
            btnViewFromEnd.IsEnabled = false;
        }

        private void disableProgressBar()
        {
            btnViewFromBegin.IsEnabled = true;
            btnViewFromEnd.IsEnabled = true;
        }

        private void btnViewFromEnd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                listVideo = new List<VideoClass>();
                ds.Clear();
                enableProgressBar();
                BindingDataVideo_ServerData_FollowDrama(this.drama_id, "newest", "1");
                btnViewFromBegin.Background = new SolidColorBrush(Colors.Black);
                btnViewFromBegin.Foreground = new SolidColorBrush(Colors.White);
                btnViewFromEnd.Background = new SolidColorBrush(Colors.White);
                btnViewFromEnd.Foreground = new SolidColorBrush(Colors.Black);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        /**************************************************************************************************/
        /** Giai quyet van de phan trang **/
        void lstVideoFollowDrama_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                FrameworkElement element = (FrameworkElement)sender;
                element.Loaded -= lstVideoFollowDrama_Loaded;
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
            List_Video_Follow_Drama page = obj as List_Video_Follow_Drama;
            ScrollViewer viewer = page.scrollViewer;

            //Checks if the Scroll has reached the last item based on the ScrollableHeight
            bool atBottom = viewer.VerticalOffset >= viewer.ScrollableHeight;

            if (atBottom)
            {
                MessageBox.Show("Đang tải dữ liệu trang " + (page.current_page + 1).ToString());
                page.BindingDataVideo_ServerData_FollowDrama(page.drama_id,page.current_filter, (page.current_page + 1).ToString());
                //BindingDataVideo_ServerData_Paging("newest", "2");
            }
        }

        public readonly DependencyProperty ListVerticalOffsetProperty = DependencyProperty.Register("ListVerticalOffset", typeof(double), typeof(List_Video_Follow_Drama),
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