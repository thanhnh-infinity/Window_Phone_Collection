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
    
    public partial class MainPage : PhoneApplicationPage
    {
        private bool fixCacheImage = true;

        ObservableCollection<ParentCategoryClass> ds = new ObservableCollection<ParentCategoryClass>();
        private List<ParentCategoryClass> listParentCategory = new List<ParentCategoryClass>();
        private ParentCategoryClass parentCategoryObj = new ParentCategoryClass();
        private int numberParentCategories = 0;
        private string folder = "TVOD";
        private String ParentCategoryFile = "parent_category.json";
        private string responseResult = "";
        private ImageCache imgCache;

        private Ultility tvodUltility;


        ProgressIndicator prog;
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            enableProgressBar();

            imgCache = new ImageCache();
            if (!isGetDataFromFile()) //API Response de xem lay du lieu tu Server hay lay du lieu tu Storage
            {
                BindingDataParentCategory_ServerData();
            }
            else
            {
                BindingDataParentCategory_FixData(folder, ParentCategoryFile);
            }

            setUpApplicationBar();
            //disableProgressBar();
            BackKeyPress += OnBackKeyPressed;
        }

        void OnBackKeyPressed(object sender,System.ComponentModel.CancelEventArgs  e)
        {
            var result = MessageBox.Show("Bạn thực sự muốn thoát khỏi ứng dụng ?", "Thoát",
                                MessageBoxButton.OKCancel);

            if (result == MessageBoxResult.OK)
            {
                return;
            }
            else
            {
                e.Cancel = true;
            }
        }

        public void enableProgressBar()
        {
            prog = new ProgressIndicator();
            prog.IsVisible = true;
            prog.IsIndeterminate = true;
            prog.Text = "Xin hãy đợi trong giây lát";
            SystemTray.SetProgressIndicator(this, prog);
        }

        public void disableProgressBar()
        {
            prog.IsVisible = false ;
        }


        public void setUpApplicationBar()
        {
            ApplicationBar = new ApplicationBar();
            ApplicationBar.IsVisible = true;

            ApplicationBarIconButton btnHome = new ApplicationBarIconButton(new Uri("/icon/home.png", UriKind.Relative));
            btnHome.Text = "Home";
            ApplicationBarIconButton btnUserProfile = new ApplicationBarIconButton(new Uri("/icon/user_profile.png", UriKind.Relative));
            btnUserProfile.Text = "Người dùng";
            ApplicationBarIconButton btnSearch = new ApplicationBarIconButton(new Uri("/icon/search.png", UriKind.Relative));
            btnSearch.Text = "Tìm kiếm";

            ApplicationBar.Buttons.Add(btnHome);
            ApplicationBar.Buttons.Add(btnUserProfile);
            ApplicationBar.Buttons.Add(btnSearch);

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


        public void BindingDataParentCategory_ServerData()
        {
            WebClient client = new WebClient();
            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client_1_DownloadStringCompleted);       
            client.DownloadStringAsync(new Uri(SystemParameter.REQUEST_PARENT_CATEGORY));
        }

        void client_1_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            string data = e.Result;
            if (this.responseResult.Equals("") || this.responseResult == null)
            {
                this.responseResult = data;
            }

            /* Ghi du lieu vao File lan dau tien */
            bool result = writeDataToFile(this.responseResult);

            parseJSONParentCategory(this.responseResult);
            ds.Clear();
            for (int i = 0; i < numberParentCategories; i++)
            {
                parentCategoryObj = new ParentCategoryClass();
                parentCategoryObj = listParentCategory[i];
                //string source_image = imgCache.getImage(parentCategoryObj.category_image);
                string source_image = "";
                if (fixCacheImage == true)
                {
                    source_image = imgCache.getImage_2(parentCategoryObj.category_id);
                }
                else //Day la hoan thien nhat tuy nhien can thoi gian de fix data
                {
                    source_image = imgCache.getImage(parentCategoryObj.category_image);
                }
                
               ds.Add(new ParentCategoryClass() { category_image = source_image, category_name = parentCategoryObj.category_name, category_id = parentCategoryObj.category_id });
            }
            this.lstParentCategory.ItemsSource = ds;
            disableProgressBar();
        }


        private void parseJSONParentCategory(String jsonParentCategory)
        {
            try
            {
                var parentCatJSONObj = JsonConvert.DeserializeObject<RootParentCategoryClass>(jsonParentCategory);
                if (parentCatJSONObj.success == true)
                {
                    foreach (var obj in parentCatJSONObj.items)
                    {
                        parentCategoryObj = new ParentCategoryClass();
                        parentCategoryObj.category_id = obj.category_id;
                        parentCategoryObj.category_name = obj.category_name;
                        parentCategoryObj.category_image = obj.category_image;
                        listParentCategory.Add(parentCategoryObj);
                    }

                    this.numberParentCategories = Int16.Parse(parentCatJSONObj.quantity);
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

                parseJSONParentCategory(rawData);

                ds.Clear();
                for (int i = 0; i < numberParentCategories; i++)
                {
                    parentCategoryObj = new ParentCategoryClass();
                    parentCategoryObj = listParentCategory[i];
                    //string source_image = imgCache.getImage(parentCategoryObj.category_image);
                    string source_image = "";
                    if (fixCacheImage == true)
                    {
                        source_image = imgCache.getImage_2(parentCategoryObj.category_id);
                    }
                    else //Day la hoan thien nhat tuy nhien can thoi gian de fix data
                    {
                        source_image = imgCache.getImage(parentCategoryObj.category_image);
                    }

                    ds.Add(new ParentCategoryClass() { category_image = source_image, category_name = parentCategoryObj.category_name, category_id = parentCategoryObj.category_id });
                }
                this.lstParentCategory.ItemsSource = ds; 
                
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private bool writeDataToFile(string data)
        {
            try
            {
                IsolatedStorageFile ISF = IsolatedStorageFile.GetUserStoreForApplication();

                if (!ISF.DirectoryExists(folder))
                {
                    ISF.CreateDirectory(folder);
                }
                using (StreamWriter SW = new StreamWriter(new IsolatedStorageFileStream(folder + "\\" + ParentCategoryFile, FileMode.Create, FileAccess.Write, ISF)))
                {
                    SW.WriteLine(data);
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

        /**Request server to know got new data in API or not : No new Data : Get Data From IsolatedStorage, Co data moi : Get From APIs */
        private bool isGetDataFromFile()
        {
            /** Request to Server to get data new or old */
            return SystemParameter.ENABLE_GET_DATA_FROM_ISOLATED_STORE;
        }

        private void lstParentCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                (App.Current as App).loadData = true;
                ParentCategoryClass parentCategory = (sender as ListBox).SelectedItem as ParentCategoryClass;
                NavigationService.Navigate(new Uri("/List_Child_Category_Level_2.xaml?category_id=" + parentCategory.category_id + "&category_name=" + parentCategory.category_name, UriKind.Relative));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }


    }
}