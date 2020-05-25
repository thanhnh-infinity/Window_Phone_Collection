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
    public partial class List_Child_Category_Level_2 : PhoneApplicationPage
    {
        /** Tham so chuyen dong cua ung dung **/
        private bool fixCacheImage = true;

        /** Properties **/
        ObservableCollection<ChildCategoryClass> ds = new ObservableCollection<ChildCategoryClass>();
        private List<ChildCategoryClass> listChildCategory = new List<ChildCategoryClass>();
        private ChildCategoryClass childCategoryObj = new ChildCategoryClass();
        private int numberChildCategories = 0;
        private string folder = "TVOD";
        private String childCategoryFile = "";
        private string responseResult = "";
        private ImageCache imgCache;

        /** Properties get from previous activity **/
        private String parent_category_id;
        private String parent_category_name;
        private Ultility tvodUltility;


        /** Constructor **/
        public List_Child_Category_Level_2()
        {
            InitializeComponent();
            imgCache = new ImageCache();
            tvodUltility = new Ultility();
            setUpApplicationBar();
            //Cho doi thong tin nhan duoc tu server
              
            
        }


        public void BindingDataChildCategory_ServerData()
        {
            string cmsURL = SystemParameter.REQUEST_CHILD_CATEGORY;
            cmsURL += this.parent_category_id;

            WebClient client = new WebClient();
            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client_1_DownloadStringCompleted);
            client.DownloadStringAsync(new Uri(cmsURL));
        }

        void client_1_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            string data = e.Result;
            if (this.responseResult.Equals("") || this.responseResult == null)
            {
                this.responseResult = data;
            }

            /* Ghi du lieu vao File lan dau tien */
            if (this.parent_category_id.Equals("13"))
            {
                this.childCategoryFile = "music_child_category.json";
            }
            else if (this.parent_category_id.Equals("14"))
            {
                this.childCategoryFile = "phim_child_category.json";
            }
            else if (this.parent_category_id.Equals("15"))
            {
                this.childCategoryFile = "clip_child_category.json";
            }
            else if (this.parent_category_id.Equals("16"))
            {
                this.childCategoryFile = "live_child_category.json";
            }
            else if (this.parent_category_id.Equals("552"))
            {
                this.childCategoryFile = "radio_child_category.json";
            }
            else if (this.parent_category_id.Equals("685"))
            {
                this.childCategoryFile = "news_child_category.json";
            }
            else
            {
                this.childCategoryFile = "child_category.json";
            }

            bool result = tvodUltility.writeDataToFile(this.responseResult,this.folder, this.childCategoryFile);

            parseJSONChildCategory(this.responseResult);
            ds.Clear();
            for (int i = 0; i < numberChildCategories; i++)
            {
                childCategoryObj = new ChildCategoryClass();
                childCategoryObj = listChildCategory[i];
                //string source_image = imgCache.getImage(parentCategoryObj.category_image);
                string source_image = "";
                if (fixCacheImage == true)
                {
                    source_image = imgCache.getImage_2(childCategoryObj.category_id);
                }
                else //Day la hoan thien nhat tuy nhien can thoi gian de fix data ( cache Image)
                {
                    source_image = imgCache.getImage(childCategoryObj.category_image);
                }

                ds.Add(new ChildCategoryClass() { category_image = source_image, category_name = childCategoryObj.category_name, number_video_category= childCategoryObj.number_video_category, category_id = childCategoryObj.category_id });
            }
            this.lstChildCategory.ItemsSource = ds;

        }

        private void parseJSONChildCategory(String jsonChildCategory)
        {
            try
            {
                var childCatJSONObj = JsonConvert.DeserializeObject<RootChildCategoryClass>(jsonChildCategory);
                if (childCatJSONObj.success == true)
                {
                    foreach (var obj in childCatJSONObj.items)
                    {
                        childCategoryObj = new ChildCategoryClass();
                        childCategoryObj.category_id = obj.category_id;
                        childCategoryObj.category_name = obj.category_name;
                        childCategoryObj.category_image = obj.category_image;
                        childCategoryObj.number_video_category = "Số lượng :" + obj.number_video_category;
                        listChildCategory.Add(childCategoryObj);
                    }

                    this.numberChildCategories = Int16.Parse(childCatJSONObj.quantity);
                }
                else
                {
                    childCategoryObj = new ChildCategoryClass();
                    childCategoryObj.category_name = "Không có dữ liệu !";
                    listChildCategory.Add(childCategoryObj);
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
            string parent_category_id = "";
            string parent_category_name = "";
            string msg = "";
            if (NavigationContext.QueryString.TryGetValue("category_id", out msg))
            {
                parent_category_id = msg;
                this.parent_category_id = parent_category_id;
            }
            if (NavigationContext.QueryString.TryGetValue("category_name", out msg))
            {
                parent_category_name = msg;
                this.parent_category_name = parent_category_name;
            }

            lblChildCategory_Name.Text = parent_category_name;


            if (!isGetDataFromFile()) //API Response de xem lay du lieu tu Server hay lay du lieu tu Storage
            {
                BindingDataChildCategory_ServerData();
            }
            else
            {
                if (this.parent_category_id.Equals("13"))
                {
                    this.childCategoryFile = "music_child_category.json";
                }
                else if (this.parent_category_id.Equals("14"))
                {
                    this.childCategoryFile = "phim_child_category.json";
                }
                else if (this.parent_category_id.Equals("15"))
                {
                    this.childCategoryFile = "clip_child_category.json";
                }
                else if (this.parent_category_id.Equals("16"))
                {
                    this.childCategoryFile = "live_child_category.json";
                }
                else if (this.parent_category_id.Equals("552"))
                {
                    this.childCategoryFile = "radio_child_category.json";
                }
                else if (this.parent_category_id.Equals("685"))
                {
                    this.childCategoryFile = "news_child_category.json";
                }
                else
                {
                    this.childCategoryFile = "child_category.json";
                }
                BindingDataParentCategory_FixData(folder, childCategoryFile);
            }
        }

        //Phuc vu cho viec Cache Data APIs //
        private void BindingDataParentCategory_FixData(string subFolder, string subFileName)
        {
            try
            {
                IsolatedStorageFile File = IsolatedStorageFile.GetUserStoreForApplication();
                string sFile = subFolder + "\\" + subFileName;
                bool fileExist = File.FileExists(sFile);
                string rawData = "";
                if (fileExist == true)
                {
                    StreamReader reader = new StreamReader(new IsolatedStorageFileStream(sFile, FileMode.Open, File));
                    rawData = reader.ReadToEnd();
                    reader.Close();
                }

                parseJSONChildCategory(rawData);
                ds = new ObservableCollection<ChildCategoryClass>();
                ds.Clear();
                for (int i = 0; i < numberChildCategories; i++)
                {
                    childCategoryObj = new ChildCategoryClass();
                    childCategoryObj = listChildCategory[i];
                    //string source_image = imgCache.getImage(parentCategoryObj.category_image);
                    string source_image = "";
                    if (fixCacheImage == true)
                    {
                        source_image = imgCache.getImage_2(childCategoryObj.category_id);
                    }
                    else //Day la hoan thien nhat tuy nhien can thoi gian de fix data
                    {
                        source_image = imgCache.getImage(childCategoryObj.category_image);
                    }

                    ds.Add(new ChildCategoryClass() { category_image = source_image, category_name = childCategoryObj.category_name, number_video_category = childCategoryObj.number_video_category, category_id = childCategoryObj.category_id });
                }
                this.lstChildCategory.ItemsSource = ds;

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
            return SystemParameter.ENABLE_GET_DATA_FROM_ISOLATED_STORE;
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

      

        private void lstChildCategory_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ChildCategoryClass childCategory = (sender as ListBox).SelectedItem as ChildCategoryClass;
                if (childCategory.category_id == SystemParameter.ID_HD_LIVE_CHILD_CATEGORY || childCategory.category_id == SystemParameter.ID_SD_LIVE_CHILD_CATEGORY || childCategory.category_id == SystemParameter.ID_MOBILE_LIVE_CHILD_CATEGORY)
                {
                    NavigationService.Navigate(new Uri("/ListChannel_Follow_Child_Category.xaml?child_category_id=" + childCategory.category_id + "&child_category_name=" + childCategory.category_name, UriKind.Relative));
                }
                else if (childCategory.category_id == SystemParameter.ID_DRAMA_CATEGORY)
                {
                    NavigationService.Navigate(new Uri("/List_Drama_Activity.xaml?child_category_id=" + childCategory.category_id + "&child_category_name=" + childCategory.category_name, UriKind.Relative));
                }
                else
                {
                    NavigationService.Navigate(new Uri("/List_Video_Follow_Child_Category.xaml?child_category_id=" + childCategory.category_id + "&child_category_name=" + childCategory.category_name, UriKind.Relative));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            
        }
    }
}