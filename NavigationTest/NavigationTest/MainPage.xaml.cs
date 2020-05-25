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

namespace NavigationTest
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        private void hyperLinkBtnGoToPage1_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Page/Page1.xaml?message=ThanhNH2911", UriKind.Relative));
        }

        private void btnGoToPage2_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Page/Page2.xaml?message=NguyenHaiThanh", UriKind.Relative));
        }
    }
}