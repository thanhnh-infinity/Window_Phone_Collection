using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Delay;

namespace PlaceImageDemo
{
    public partial class CatControl : UserControl
    {
        private readonly Random _rand = new Random();

        public CatControl()
        {
            InitializeComponent();
            DataContext = new Menagerie();
        }

        private void PlaceImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var placeImage = (PlaceImage)sender;
            placeImage.Source = new BitmapImage(new Uri(string.Format(CultureInfo.InvariantCulture, "http://placekitten.com/{0}/{1}", _rand.Next(50, 200), _rand.Next(50, 200))));
        }
    }
}
