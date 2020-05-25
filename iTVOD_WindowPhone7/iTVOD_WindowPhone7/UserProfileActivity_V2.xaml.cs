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
    public partial class UserProfileActivity_V2 : PhoneApplicationPage
    {

        /** Dieu khien scoll View cua ung dung **/
        ScrollViewer scrollViewer;

        /** Progress Bar Indicator **/
        ProgressIndicator prog;


        private Ultility tvodUltility;

        private String responseResult;
        private Boolean success;

        private Boolean logout_success;

        private LogoutClass logoutObject;

        private AccountClass account;

        private string cacheCookieFile = "tvod_cookie.txt";
        private string folder = "TVOD";


        /** Properties for list transaction **/
        ObservableCollection<TransactionClass> ds = new ObservableCollection<TransactionClass>();
        private List<TransactionClass> listTransaction = new List<TransactionClass>();
        private TransactionClass transactionObj = new TransactionClass();
        private int numberTransaction = 0;
        private int numberTransactionPerPage = 0;
        private int current_page = 1;
        private int total_transaction;


        /** Properties for list favourite contents **/
        ObservableCollection<VideoClass> ds_favourite = new ObservableCollection<VideoClass>();
        private List<VideoClass> listFavouriteContent = new List<VideoClass>();
        private VideoClass favouriteObject = new VideoClass();
        private int numberFavourite = 0;
        private int numberFavouritePerPage = 0;
        private int current_page_favourite = 1;
        private int total_favourite;

        /** Tham so chuyen dong cua ung dung **/
        private bool fixCacheImage = false;

        private ImageCache imgCache = new ImageCache();

        public UserProfileActivity_V2()
        {
            InitializeComponent();
            setUpApplicationBar();
            enableProgressBar();
            tvodUltility = new Ultility();
            this.lstNDDaMua.Loaded += new RoutedEventHandler(lstNDDaMua_Loaded);
           //this.lstFavouriteContent.Loaded += new RoutedEventHandler(lstFavouriteContent_Loaded);
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

        public void enableProgressIndicator_For_Transaction()
        {
            prog = new ProgressIndicator();
            prog.IsVisible = true;
            prog.IsIndeterminate = true;
            prog.Text = "Xin hãy đợi trong giây lát";
            btnViewTransaction.IsEnabled = false;
            SystemTray.SetProgressIndicator(this, prog);
        }

        public void enableProgressIndicator_For_Favourite()
        {
            prog = new ProgressIndicator();
            prog.IsVisible = true;
            prog.IsIndeterminate = true;
            prog.Text = "Xin hãy đợi trong giây lát";
            btnViewFavouriteContentList.IsEnabled = false;
            SystemTray.SetProgressIndicator(this, prog);
        }

        public void disableProgressIndicator()
        {
            prog.IsVisible = false;
           // btnViewTransaction.IsEnabled = true;
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
            btnSearch.Click += new EventHandler(btnSearch_Click);

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/SearchActivity.xaml", UriKind.Relative));
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

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            (App.Current as App).loadData = false;
            base.OnNavigatedFrom(e);
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
                    account.payment_method = userProfileJSONObj.payment_method;
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
            cmsURL = cmsURL + "&time=" + DateTime.Now;
         
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

            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client_user_profile_DownloadStringCompleted);
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
                    if (account.payment_method == null || account.payment_method == "0")
                    {
                        lblPaymentMethod.Text = "Trả Trước";
                    }
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
                //bool clearAppCookie = false;
                parseJSONLogout(this.responseResult);
                if (this.logout_success)
                {
                    /* Xoa du lieu session */
                    if ((App.Current as App).cookie != null && !(App.Current as App).cookie.Equals(""))
                    {
                        (App.Current as App).cookie = "";
                        (App.Current as App).cookie = null;
                        //clearAppCookie = true;
                    }

                    string old_cookie = getCookieFromIsolatedStorage();
                    if (old_cookie != null && old_cookie != "" && old_cookie != "\r\n")
                    {
                        delete_cookie = deleteCookieFromIsolatedStorage();
                    }
                       
                    if (delete_cookie)
                    {
                        if (MessageBox.Show("Đã đăng xuất thành công khỏi hệ thống", "Thông báo", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                        {
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

        private void btnNapTien_Click(object sender, RoutedEventArgs e)
        {
            SmsComposeTask smsComposeTask = new SmsComposeTask();
            smsComposeTask.To = "6780";
            smsComposeTask.Body = "NAP " + account.uid;
            smsComposeTask.Show();          
        }

        private void btnGiaHan_Click(object sender, RoutedEventArgs e)
        {
            PurchaseExpiredDateActivity purchaseForm = new PurchaseExpiredDateActivity();
            purchaseForm.Show();
        }

        private void btnViewTransaction_Click(object sender, RoutedEventArgs e)
        {
            enableProgressIndicator_For_Transaction();
            BindingData_Transaction("1");
        }

        public void BindingData_Transaction(string page)
        {
            try
            {
                //DateTime time = DateTime.Now;
                string cmsURL = SystemParameter.REQUEST_TRANSACTION;
                cmsURL = cmsURL.Replace("%p", page);
                cmsURL = cmsURL.Replace("%t", "1");
                cmsURL = cmsURL + (DateTime.Now);
                
                this.current_page = Int32.Parse(page);
               
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

                this.current_page = Int32.Parse(page);
                if (this.current_page == 1)
                {
                    client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client_list_transaction_DownloadStringCompleted);
                    client.DownloadStringAsync(new Uri(cmsURL));
                }
                else
                {

                    if ((this.current_page > 1) && ((this.current_page - 1) * SystemParameter.NUMBER_ITEMS_PER_PAGE < this.total_transaction))
                    {
                        client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client_list_transaction_DownloadStringCompleted);
                        client.DownloadStringAsync(new Uri(cmsURL));
                    }
                    else
                    {
                        ds.Add(new TransactionClass() { content_picture_path = "http", transaction_id = "Không có dữ liệu", transaction_date = "", transaction_value = "", stop_time = "", content_id = "", content_name = "" });
                        this.lstNDDaMua.ItemsSource = ds;
                        disableProgressIndicator();
                    }
                }

                
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        void client_list_transaction_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                string data = e.Result;
                this.responseResult = data;

                parseJSONTransactionList(this.responseResult);
                //ds = new ObservableCollection<TransactionClass>();
                if (this.current_page == 1)
                    ds.Clear();
                if (this.numberTransaction > 0)
                {
                    for (int i = 0; i < numberTransactionPerPage; i++)
                    {
                        transactionObj = new TransactionClass();
                        transactionObj = listTransaction[i];
                        
                        string source_image = "";
                        if (fixCacheImage == true)
                        {
                            //source_image = imgCache.getImage_2(childCategoryObj.category_id);
                        }
                        else //Day la hoan thien nhat tuy nhien can thoi gian de fix data ( cache Image)
                        {
                            source_image = imgCache.getImage(transactionObj.content_picture_path);
                        }

                        ds.Add(new TransactionClass() { content_picture_path = source_image, transaction_id = transactionObj.transaction_id, transaction_date = transactionObj.transaction_date, transaction_value = transactionObj.transaction_value, stop_time=transactionObj.stop_time, content_id=transactionObj.content_id, content_name = transactionObj.content_name });
                    }
                    this.lstNDDaMua.ItemsSource = ds;
                    //disableProgressBar();
                }
                else
                {
                    ds.Add(new TransactionClass() { content_picture_path = "http", transaction_id = "Không có dữ liệu", transaction_date = "", transaction_value = "", stop_time = "", content_id = "", content_name = "" });
                    this.lstNDDaMua.ItemsSource = ds;
                    //disableProgressBar();
                }
                disableProgressIndicator();
                
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

        private void parseJSONTransactionList(String jsonTransactionList)
        {
            try
            {
                var transactionListJSONObj = JsonConvert.DeserializeObject<RootTransactionClass>(jsonTransactionList);
                if (transactionListJSONObj.success == true || transactionListJSONObj.type == "transaction")
                {
                    this.total_transaction = Int32.Parse(transactionListJSONObj.total_quantity);
                    listTransaction = new List<TransactionClass>();
                    foreach (var obj in transactionListJSONObj.items)
                    {
                        transactionObj = new TransactionClass();
                        transactionObj.transaction_id =    "ID giao dịch   : " + obj.transaction_id;
                        transactionObj.transaction_date =  "Thời gian      : " + obj.transaction_date;
                        transactionObj.transaction_value = "Giá mua        : " + obj.transaction_value;
                        if (obj.stop_time == null || obj.stop_time == "")
                        {
                            transactionObj.stop_time = "Đã xem hết";
                        }
                        else
                        {
                            transactionObj.stop_time = "Thời điểm dừng : " + obj.stop_time;
                        }
                        transactionObj.content_picture_path = obj.content_picture_path;
                        transactionObj.content_id=obj.content_id;
                        transactionObj.content_name =obj.content_name;
                        listTransaction.Add(transactionObj);

                    }
                    this.numberTransactionPerPage = Int16.Parse(transactionListJSONObj.quantity);
                    this.numberTransaction = Int16.Parse(transactionListJSONObj.total_quantity);
                }
                else
                {
                    transactionObj = new TransactionClass();
                    transactionObj.transaction_id = "Không có dữ liệu !";
                    this.numberTransactionPerPage = 0;
                    this.numberTransaction = 0;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }


        private void btnViewFavouriteContentList_Click(object sender, RoutedEventArgs e)
        {
            enableProgressIndicator_For_Favourite();
            BindingData_ListFavouriteContent("1");
        }

        public void BindingData_ListFavouriteContent(string page)
        {
            try
            {
                string cmsURL = SystemParameter.REQUEST_LIST_FAVOURITE_CONTENTS;
                cmsURL = cmsURL.Replace("%p", page);
                cmsURL = cmsURL + (DateTime.Now);
                this.current_page_favourite = Int32.Parse(page);

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

                this.current_page_favourite = Int32.Parse(page);
                if (this.current_page_favourite == 1)
                {
                    client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client_list_favourite_content_DownloadStringCompleted);
                    client.DownloadStringAsync(new Uri(cmsURL));
                }
                else
                {

                    if ((this.current_page_favourite > 1) && ((this.current_page_favourite - 1) * SystemParameter.NUMBER_ITEMS_PER_PAGE < this.total_favourite))
                    {
                        client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client_list_favourite_content_DownloadStringCompleted);
                        client.DownloadStringAsync(new Uri(cmsURL));
                    }
                    else
                    {
                        ds_favourite.Add(new VideoClass() { video_picture_path = "http", video_english_title = "Không có dữ liệu", video_vietnamese_title = "", video_id = "", video_price = "", video_description = "", video_duration = "", video_number_views = "" });
                        this.lstFavouriteContent.ItemsSource = ds_favourite;
                        disableProgressIndicator();
                    }
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        void client_list_favourite_content_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                string data = e.Result;

                parseJSONVideoList(data);
                
                if (this.current_page_favourite == 1)
                    ds_favourite.Clear();
                if (this.numberFavourite > 0)
                {
                    for (int i = 0; i < numberFavouritePerPage ; i++)
                    {
                        favouriteObject = new VideoClass();
                        favouriteObject = listFavouriteContent[i];

                        string source_image = "";
                        if (fixCacheImage == true)
                        {
                            
                        }
                        else
                        {
                            source_image = imgCache.getImage(favouriteObject.video_picture_path);
                        }

                        ds_favourite.Add(new VideoClass() { video_picture_path = source_image, video_id = favouriteObject.video_id, video_description = favouriteObject.video_description, video_vietnamese_title = favouriteObject.video_vietnamese_title , video_english_title = favouriteObject.video_english_title, video_price = favouriteObject.video_price, video_number_views = favouriteObject.video_number_views, video_duration=favouriteObject.video_duration});
                    }
                    this.lstFavouriteContent.ItemsSource = ds_favourite;
                    
                }
                else
                {
                    ds_favourite.Add(new VideoClass() { video_picture_path = "http", video_english_title = "Không có dữ liệu", video_vietnamese_title = "", video_id = "", video_price = "", video_description = "", video_duration = "", video_number_views = "" });
                    this.lstFavouriteContent.ItemsSource = ds_favourite;
                   
                }
                disableProgressIndicator();

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
                    this.total_favourite = Int32.Parse(videoListJSONObj.total_quantity);
                    listFavouriteContent = new List<VideoClass>();
                    foreach (var obj in videoListJSONObj.items)
                    {
                        favouriteObject = new VideoClass();
                        favouriteObject.video_id = obj.video_id;
                        favouriteObject.video_description = obj.video_description;
                        favouriteObject.video_duration = obj.video_duration;
                        favouriteObject.video_english_title = obj.video_english_title;
                        favouriteObject.video_number_views = "Số lượt xem : " + obj.video_number_views;
                        favouriteObject.video_picture_path = obj.video_picture_path;
                        favouriteObject.video_price = obj.video_price;
                        favouriteObject.video_vietnamese_title = obj.video_vietnamese_title;
                        listFavouriteContent.Add(favouriteObject);

                    }
                    this.numberFavouritePerPage = Int16.Parse(videoListJSONObj.quantity);
                    this.numberFavourite = Int16.Parse(videoListJSONObj.total_quantity);
                }
                else
                {
                    favouriteObject = new VideoClass();
                    favouriteObject.video_english_title = "Không có dữ liệu !";
                    this.numberFavouritePerPage = 0;
                    this.numberFavourite = 0;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }

        /*****************************************************************************************************/
        /*************Phan trang nao**************************************************************************/
        /*****************************************************************************************************/
        void lstNDDaMua_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                FrameworkElement element = (FrameworkElement)sender;
                element.Loaded -= lstNDDaMua_Loaded;
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
            UserProfileActivity_V2 page = obj as UserProfileActivity_V2;
            ScrollViewer viewer = page.scrollViewer;

            //Checks if the Scroll has reached the last item based on the ScrollableHeight
            bool atBottom = viewer.VerticalOffset >= viewer.ScrollableHeight - 1;

            if (atBottom)
            {
                page.enableProgressIndicator_For_Transaction();
                page.BindingData_Transaction((page.current_page + 1).ToString());
            }
        }

        public readonly DependencyProperty ListVerticalOffsetProperty = DependencyProperty.Register("ListVerticalOffset", typeof(double), typeof(UserProfileActivity_V2),
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

        /**********************************************************************************/
        /************************ENDING PAGING*********************************************/
        /**********************************************************************************/
        private void lstFavouriteContent_SelectionChanged(object sender, SelectionChangedEventArgs e)
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

        private void lstNDDaMua_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                TransactionClass transactionObjTemp = (sender as ListBox).SelectedItem as TransactionClass;
                String mess = "";
                if (transactionObjTemp.stop_time == null || transactionObjTemp.stop_time == "Đã xem hết")
                {
                    mess = "Bạn có muốn xem lại nội dung này ?";
                }
                else
                {
                    mess = "Bạn có muốn xem tiếp nội dung này ?";
                }
                if (MessageBox.Show(mess, "Thông Báo", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    enableProgressIndicator_For_Transaction();
                    this.getURL(transactionObjTemp.content_id);
                }
                else
                {

                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }


        private void getURL(String video_id)
        {
            //enableProgressBar();
            /** URL **/
            string cmsURl = SystemParameter.REQUEST_VIDEO_MOBILE_URL;
            cmsURl = cmsURl.Replace("%s", video_id);


            WebClient clientURL = new WebClient();

            if (tvodUltility.checkLogin())
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

                clientURL.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client_getURL_DownloadStringCompleted);
                clientURL.DownloadStringAsync(new Uri(cmsURl.ToString()));
            }
            else
            {
                MessageBox.Show("Lỗi hệ thống !");
                disableProgressIndicator();
            }
       
        }

        void client_getURL_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                string data = e.Result;
                parseJSONGetURL(data);
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
                    String video_url = URLJSONObj.url;
                    if (video_url != null && !video_url.Equals(""))
                    {

                        MediaPlayerLauncher player = new MediaPlayerLauncher();
                        player.Media = new Uri(video_url);
                        player.Show();

                    }
                    else
                    {
                        MessageBox.Show("File MP4 được gửi từ Server bị lỗi !");

                    }
                   
                }
                else
                {
                    disableProgressIndicator();
                    MessageBox.Show("Bạn không đủ tiền để xem nội dung này !");
                }

                disableProgressIndicator();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }


        }

       

       
    }
}