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
using Microsoft.Phone.Shell;

namespace NewApplicationBar
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            SupportedOrientations = SupportedPageOrientation.Portrait | SupportedPageOrientation.Landscape;

            ApplicationBar = new ApplicationBar();
            ApplicationBar.IsMenuEnabled = true;
            ApplicationBar.IsVisible = true;

            ApplicationBarIconButton btnAdd = new ApplicationBarIconButton(new Uri("/Images/ApplicationIcon.png",UriKind.Relative));
            btnAdd.Text = "ADD";
            ApplicationBarIconButton btnSave = new ApplicationBarIconButton(new Uri("/Images/ApplicationIcon.png", UriKind.Relative));
            btnSave.Text = "SAVE";
            ApplicationBarIconButton btnDelete = new ApplicationBarIconButton(new Uri("/Images/ApplicationIcon.png", UriKind.Relative));
            btnDelete.Text = "DELETE";

            ApplicationBarMenuItem menuItem1 = new ApplicationBarMenuItem("Menu Item 1");
            ApplicationBarMenuItem menuItem2 = new ApplicationBarMenuItem("Menu Item 2");


            ApplicationBar.Buttons.Add(btnAdd);
            ApplicationBar.Buttons.Add(btnSave);
            ApplicationBar.Buttons.Add(btnDelete);

            ApplicationBar.MenuItems.Add(menuItem1);
            ApplicationBar.MenuItems.Add(menuItem2);
            menuItem1.Click += new EventHandler(menuItem1_Click);
            menuItem2.Click += new EventHandler(menuItem2_Click);


            btnAdd.Click += new EventHandler(btnAdd_Click);
            btnSave.Click += new EventHandler(btnSave_Click);
            btnDelete.Click += new EventHandler(btnDelete_Click);

        }

        private void menuItem1_Click(object sender, EventArgs e)
        {
            txtDisplay.Visibility = Visibility.Visible;
        }

        private void menuItem2_Click(object sender, EventArgs e)
        {
            txtDisplay.Visibility = Visibility.Collapsed;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            txtDisplay.Text = "Nguyen Hai Thanh - Doc co cau bai";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            txtDisplay.Text = "Da Save";
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            txtDisplay.Text = "";
        }
    }
}