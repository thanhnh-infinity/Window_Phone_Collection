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
using Microsoft.Phone.Tasks;
using System.Diagnostics;
using System.IO.IsolatedStorage;
using System.IO;
using System.Threading;

namespace Tasks
{
    public partial class MainPage : PhoneApplicationPage
    {
        private PhoneNumberChooserTask _choosePhoneNumberTask;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            _choosePhoneNumberTask = new PhoneNumberChooserTask();
            _choosePhoneNumberTask.Completed += new EventHandler<PhoneNumberResult>(ChoosePhoneNumberTaskCompleted);
        }

        private void ChoosePhoneNumberTaskCompleted(object sender, PhoneNumberResult e)
        {
            new SmsComposeTask()
            {
                Body = "SMS using Windows Phone 7 Chooser",
                To = e.PhoneNumber
            }.Show();
        }

        private void btnSendMess_Click(object sender, RoutedEventArgs e)
        {
            _choosePhoneNumberTask.Show();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Thanh Nguyen Hai");
        }

        private void btnLauncherBrowser_Click(object sender, RoutedEventArgs e)
        {
            WebBrowserTask webTask = new WebBrowserTask();
            webTask.URL = "http://www.tvod.vn";
            
            webTask.Show();
            
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            Debug.WriteLine("Navigated From MainPage");
            if (State.ContainsKey("TextboxText"))
            {
                State.Remove("TextboxText");
            }
            base.OnNavigatedFrom(e);
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            Debug.WriteLine("Navigated To MainPage");
            if (State.ContainsKey("TextboxText"))
            {
                string strTextBoxText = State["TextboxText"] as string;
                if (strTextBoxText != null)
                {
                    textBox1.Text = strTextBoxText;
                }

            }
            else
            {
                LoadAppStateDataAsync();
            }
            base.OnNavigatedTo(e);
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            base.OnBackKeyPress(e);

            MessageBoxResult res = MessageBox.Show("Do you want to save your works before leaving ?", "You are existing in application", MessageBoxButton.OKCancel);

            if (res == MessageBoxResult.OK)
            {
                Debug.WriteLine("OK - Save to isolated storage");
                SaveString(textBox1.Text, "TextboxText.dat");
            }
            else
            {
                Debug.WriteLine("Cancel - DOnt Save");
            }
        }


        private void SaveString(string strTextToSave, string fileName)
        {
            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                // If user choose save, create new file 
                using (IsolatedStorageFileStream fs = isf.CreateFile(fileName))
                {
                    using (StreamWriter write = new StreamWriter(fs))
                    {
                        write.WriteLine(strTextToSave);
                    }
                }
            }

        }


        private void LoadAppStateDataAsync()
        {
            Thread t = new Thread(new ThreadStart(LoadAppStateData));
            t.Start();
        }

        private void LoadAppStateData()
        {
            string strData = String.Empty;
            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
            {

                //Check if file exits
                if (isf.FileExists("TextboxText.dat"))
                {
                    using (IsolatedStorageFileStream fs = isf.OpenFile("TextboxText.dat", System.IO.FileMode.Open))
                    {
                        using (StreamReader reader = new StreamReader(fs))
                        {
                            strData = reader.ReadToEnd();
                        }
                    }
                }

            }
            Dispatcher.BeginInvoke(() => { textBox1.Text = strData;  });
        }

    }
}