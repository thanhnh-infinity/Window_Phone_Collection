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
    public partial class List_Drama_Activity : PhoneApplicationPage
    {
        /** Dieu khien scoll View cua ung dung **/
        ScrollViewer scrollViewer;

        /** Tham so chuyen dong cua ung dung **/
        private bool fixCacheImage = false;

        /** Properties **/
        ObservableCollection<DramaClass> ds;
        private List<DramaClass> listDrama = new List<DramaClass>();
        private DramaClass dramaObj = new DramaClass();
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

        public List_Drama_Activity()
        {
            InitializeComponent();
            imgCache = new ImageCache();
            tvodUltility = new Ultility();
            setUpApplicationBar();
            enableProgressBar();

            btnNewest.Background = new SolidColorBrush(Colors.White);
            btnNewest.Foreground = new SolidColorBrush(Colors.Black);
            this.lstDrama.Loaded += new RoutedEventHandler(lstDrama_Loaded);
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

            BindingDataDrama_ServerData("newest", "1");
        }

        public void BindingDataDrama_ServerData(string filter, string page)
        {
            string cmsURL = SystemParameter.REQUEST_LIST_DRAMA;
            cmsURL = cmsURL.Replace("%f", filter);
            cmsURL = cmsURL.Replace("%p", page);
            this.current_filter = filter;
            this.current_page = 1;
            WebClient client = new WebClient();
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

                parseJSONDramaList(this.responseResult);
                ds = new ObservableCollection<DramaClass>();
                if (this.current_page == 1)
                    ds.Clear();
                if (this.numberVideo > 0)
                {
                    for (int i = 0; i < numberVideoPerPage; i++)
                    {
                        dramaObj = new DramaClass();
                        dramaObj = listDrama[i];
                        //string source_image = imgCache.getImage(parentCategoryObj.category_image);
                        string source_image = "";
                        if (fixCacheImage == true)
                        {
                            //source_image = imgCache.getImage_2(childCategoryObj.category_id);
                        }
                        else //Day la hoan thien nhat tuy nhien can thoi gian de fix data ( cache Image)
                        {
                            source_image = imgCache.getImage(dramaObj.drama_image_path);
                            if (source_image == null || source_image == string.Empty || source_image == "")
                            {
                                source_image = "http";
                            }
                        }

                        ds.Add(new DramaClass() { drama_image_path = source_image, drama_english_title = dramaObj.drama_english_title, drama_vietnamese_title = dramaObj.drama_vietnamese_title, drama_quantity = dramaObj.drama_quantity, drama_id = dramaObj.drama_id });
                    }
                    this.lstDrama.ItemsSource = ds;
                    disableProgressBar();
                }
                else
                {
                    ds.Add(new DramaClass() { drama_image_path = "http", drama_english_title = "Không có dữ liệu", drama_vietnamese_title = "", drama_quantity = "", drama_id = "" });
                    this.lstDrama.ItemsSource = ds;
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


        private void parseJSONDramaList(String jsonDramaList)
        {
            try
            {
                var dramaListJSONObj = JsonConvert.DeserializeObject<RootDramaClass>(jsonDramaList);
                if (dramaListJSONObj.success == true || dramaListJSONObj.type == "drama")
                {
                    this.total_video = Int32.Parse(dramaListJSONObj.total_quantity);
                    listDrama = new List<DramaClass>();
                    foreach (var obj in dramaListJSONObj.items)
                    {
                        dramaObj = new DramaClass();
                        dramaObj.drama_id = obj.drama_id;
                        dramaObj.drama_english_title = obj.drama_english_title;
                        dramaObj.drama_image_path = obj.drama_image_path;
                        dramaObj.drama_quantity = "Số tập trong bộ : " + obj.drama_quantity;
                        dramaObj.drama_vietnamese_title = obj.drama_vietnamese_title;
                        listDrama.Add(dramaObj);

                    }
                    this.numberVideoPerPage = Int16.Parse(dramaListJSONObj.quantity);
                    this.numberVideo = Int16.Parse(dramaListJSONObj.total_quantity);
                }
                else
                {
                    dramaObj = new DramaClass();
                    dramaObj.drama_english_title = "Không có dữ liệu !";
                    this.numberVideoPerPage = 0;
                    this.numberVideo = 0;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }

        /**Request server to know got new data in API or not : No new Data : Get Data From IsolatedStorage, Co data moi : Get From APIs */
        private bool isGetDataFromFile()
        {
            /** Request to Server to get data new or old */
            return false;
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
                    //NavigationService.Navigate(new Uri("/LoginActivity.xaml", UriKind.Relative));
                    LoginActivity_V2 loginWindow = new LoginActivity_V2();
                    //loginWindow.Login = Username;
                    //loginWindow.Closed += new EventHandler(OnLoginChildWindowShow);
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

        private void enableProgressBar()
        {
            if (ds == null)
            {
                ds = new ObservableCollection<DramaClass>();

            }
            ds.Add(new DramaClass() { drama_image_path = "http", drama_english_title = "Loading.............", drama_vietnamese_title = "", drama_quantity = "", drama_id = "" });
            this.lstDrama.ItemsSource = ds;
            btnMostView.IsEnabled = false;
            btnNewest.IsEnabled = false;
        }

        private void disableProgressBar()
        {
            btnMostView.IsEnabled = true;
            btnNewest.IsEnabled = true;
        }

        private void btnNewest_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                listDrama = new List<DramaClass>();
                ds.Clear();
                enableProgressBar();
                BindingDataDrama_ServerData("newest", "1");
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
                listDrama = new List<DramaClass>();
                ds.Clear();
                enableProgressBar();
                BindingDataDrama_ServerData("most_view", "1");
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

        /*****************************************************************************************************/
        /*************Phan trang nao**************************************************************************/
        void lstDrama_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                FrameworkElement element = (FrameworkElement)sender;
                element.Loaded -= lstDrama_Loaded;
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
        public void BindingDataDrama_ServerData_Paging(string filter, string page)
        {
            try
            {
                string cmsURL = SystemParameter.REQUEST_LIST_DRAMA;
                cmsURL = cmsURL.Replace("%f", filter);
                cmsURL = cmsURL.Replace("%p", page);
                this.current_page = Int32.Parse(page);
                if ((this.current_page > 1) && ((this.current_page - 1) * 25 < this.total_video))
                {
                    WebClient client = new WebClient();
                    client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client_2_DownloadStringCompleted);
                    client.DownloadStringAsync(new Uri(cmsURL));
                }
                else
                {
                    ds.Add(new DramaClass() { drama_image_path = "http", drama_english_title = "Không có dữ liệu", drama_vietnamese_title = "", drama_quantity = "", drama_id = "" });
                    this.lstDrama.ItemsSource = ds;

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        void client_2_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                string data = e.Result;
                // if (this.responseResult.Equals("") || this.responseResult == null)
                // {
                this.responseResult = data;
                // }
                //bool result = tvodUltility.writeDataToFile(this.responseResult, this.folder, this.childCategoryFile);

                parseJSONDramaList(this.responseResult);
                if (this.numberVideo > 0)
                {
                    for (int i = 0; i < numberVideoPerPage; i++)
                    {
                        dramaObj = new DramaClass();
                        dramaObj = listDrama[i];
                        //string source_image = imgCache.getImage(parentCategoryObj.category_image);
                        string source_image = "";
                        if (fixCacheImage == true)
                        {
                            //source_image = imgCache.getImage_2(childCategoryObj.category_id);
                        }
                        else //Day la hoan thien nhat tuy nhien can thoi gian de fix data ( cache Image)
                        {
                            source_image = imgCache.getImage(dramaObj.drama_image_path);
                            if (source_image == null || source_image == string.Empty || source_image == "")
                            {
                                source_image = "http";
                            }
                        }

                        ds.Add(new DramaClass() { drama_image_path = source_image, drama_english_title = dramaObj.drama_english_title, drama_vietnamese_title = dramaObj.drama_vietnamese_title, drama_quantity = dramaObj.drama_quantity, drama_id = dramaObj.drama_id });
                    }
                    this.lstDrama.ItemsSource = ds;
                    disableProgressBar();
                }
                else
                {
                    ds.Add(new DramaClass() { drama_image_path = "http", drama_english_title = "Không có dữ liệu", drama_vietnamese_title = "", drama_quantity = "", drama_id = "" });
                    this.lstDrama.ItemsSource = ds;
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

        public static void OnListVerticalOffsetChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            List_Drama_Activity page = obj as List_Drama_Activity;
            ScrollViewer viewer = page.scrollViewer;

            //Checks if the Scroll has reached the last item based on the ScrollableHeight
            bool atBottom = viewer.VerticalOffset >= viewer.ScrollableHeight;

            if (atBottom)
            {
                MessageBox.Show("Đang tải dữ liệu trang " + (page.current_page + 1).ToString());
                page.BindingDataDrama_ServerData_Paging(page.current_filter, (page.current_page + 1).ToString());
            }
        }

        public readonly DependencyProperty ListVerticalOffsetProperty = DependencyProperty.Register("ListVerticalOffset", typeof(double), typeof(List_Drama_Activity),
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

        private void lstDrama_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DramaClass dramaObjTemp = (sender as ListBox).SelectedItem as DramaClass;
                NavigationService.Navigate(new Uri("/List_Video_Follow_Drama.xaml?drama_id=" + dramaObjTemp.drama_id + "&drama_english_title=" + dramaObjTemp.drama_english_title + "&drama_vietnamese_title=" + dramaObjTemp.drama_vietnamese_title + "&drama_image_path=" + dramaObjTemp.drama_image_path + "&drama_quantity=" +  dramaObjTemp.drama_quantity, UriKind.Relative));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}