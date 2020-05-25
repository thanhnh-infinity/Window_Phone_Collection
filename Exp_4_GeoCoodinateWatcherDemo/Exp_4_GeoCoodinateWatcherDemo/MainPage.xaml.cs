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
using Microsoft.Phone.Reactive;
using System.Device.Location;



namespace Exp_4_GeoCoodinateWatcherDemo
{
    public partial class MainPage : PhoneApplicationPage
    {
        GeoCoordinateWatcher _geoCoordinateWatcher;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            _geoCoordinateWatcher = new GeoCoordinateWatcher();
            _geoCoordinateWatcher.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(_geoCoordinateWatcher_PositionChanged);

            Thread simulateGpsThread = new Thread(SimulateGPS);
            simulateGpsThread.Start();

        }

        private void SimulateGPS()
        {
            var position = GPSPositionChangedEvents().ToObservable();
            position.Subscribe(evt => _geoCoordinateWatcher_PositionChanged(null, evt));
        }

        private static IEnumerable<GeoPositionChangedEventArgs<GeoCoordinate>> GPSPositionChangedEvents()
        {
            Random random = new Random();
            while (true)
            {
                Thread.Sleep(TimeSpan.FromSeconds(2));
                double latitude = (random.NextDouble() * 180.0) - 90.0;
                double longitude = (random.NextDouble() * 360.0) - 90.0;
                yield return new GeoPositionChangedEventArgs<GeoCoordinate>(new GeoPosition<GeoCoordinate>(DateTimeOffset.Now, new GeoCoordinate(latitude, longitude)));

            }
        }

        private void _geoCoordinateWatcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            this.Dispatcher.BeginInvoke(() =>
                {
                    txtLatitude.Text = e.Position.Location.Latitude.ToString();
                    txtLongitude.Text = e.Position.Location.Longitude.ToString();
                }
            );
        }
    }
}