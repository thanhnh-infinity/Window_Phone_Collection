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
using Microsoft.Phone.Marketplace;
using Microsoft.Phone.Tasks;

namespace Example_2_TrialExample
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            LicenseInformation lic = new LicenseInformation();
            if (lic.IsTrial() == false)
            {
                textBlock1.Text = "You are running trial version of our software !";
                btnUpgrade.Visibility = Visibility.Visible;
                btnCancel.Visibility = Visibility.Visible;
            }
            else
            {
                textBlock1.Text = "Thank you for using Full-Version of our software !";
            }
        }

        private void btnUpgrade_Click(object sender, RoutedEventArgs e)
        {
            MarketplaceReviewTask marketPlaceReviewTask = new MarketplaceReviewTask();
            marketPlaceReviewTask.Show();
        }
    }
}