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
using System.Threading;

namespace ProgressBarWithTextSample
{
    public partial class MainPage : PhoneApplicationPage
    {


        public bool ShowProgress
        {
            get { return (bool)GetValue(ShowProgressProperty); }
            set { SetValue(ShowProgressProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowProgress.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowProgressProperty =
            DependencyProperty.Register("ShowProgress", typeof(bool), typeof(MainPage), new PropertyMetadata(false));



        // Constructor
        public MainPage()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            ShowProgress = true;

            ThreadPool.QueueUserWorkItem(
                (o) =>
                {
                    Thread.Sleep(TimeSpan.FromSeconds(5));
                    this.Dispatcher.BeginInvoke(
                        () => 
                        {
                            ShowProgress = false;
                        });
                });

        }
    }
}