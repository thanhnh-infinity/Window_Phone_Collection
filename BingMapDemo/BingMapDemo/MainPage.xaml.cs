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
using Microsoft.Maps.MapControl;
using System.Device.Location;

namespace BingMapDemo
{
    public partial class MainPage : PhoneApplicationPage
    {
        GeoCoordinateWatcher _geoCoordinateWatcher;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            bingMap.CredentialsProvider = new ApplicationIdCredentialsProvider("Aju2BL9XfGi73nB7RZrWUuvOxLmBxHQPZTc54GKVBB-SFYLByskD4clINn4mU_rt");

        }
    }
}