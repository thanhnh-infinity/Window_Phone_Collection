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
using System.Globalization;
using System.Threading;
using System.Resources;
using System.Reflection;

namespace Example_3_InternationalizationSample
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            PageTitle.Text = CultureInfo.CurrentCulture.ToString();
            ShowEventDetails();
        }


        private void ShowEventDetails()
        {

            ResourceManager rm = new ResourceManager("Example_3_InternationalizationSample.AppResources_vn", Assembly.GetExecutingAssembly());

            ApplicationTitle.Text = rm.GetString("EventTitle");
            txtEventCostCaption.Text = rm.GetString("EventCost");
            txtEventDateCaption.Text = rm.GetString("EventDate");
            txtEventTimeCaption.Text = rm.GetString("EventTime");



            DateTime dtLaunchDate = new DateTime(2012,10,23,21,0,0);
            decimal decEventCost = 5.0M;

            txtEventDate.Text = dtLaunchDate.ToString("D",Thread.CurrentThread.CurrentCulture);
            txtEventTime.Text = dtLaunchDate.ToString("T");
            txtEventCost.Text = decEventCost.ToString("C");
        }

        private void ToggleEventLocale()
        {
            String cul = "en-US";
            if (button1.Content.ToString() == "Button")
            {
                cul = "es-ES";
            }
            else
            {
                cul = "en-US";
            }
            CultureInfo newCulture = new CultureInfo(cul);
            Thread.CurrentThread.CurrentCulture = newCulture;
            ShowEventDetails();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            ToggleEventLocale();
        }
    }
}