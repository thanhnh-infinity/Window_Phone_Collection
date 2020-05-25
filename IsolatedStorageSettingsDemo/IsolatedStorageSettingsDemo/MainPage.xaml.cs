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
using System.IO.IsolatedStorage;

namespace IsolatedStorageSettingsDemo
{
    public partial class MainPage : PhoneApplicationPage
    {
        private IsolatedStorageSettings _appSettings;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            SupportedOrientations = SupportedPageOrientation.Portrait | SupportedPageOrientation.Landscape;
            _appSettings = IsolatedStorageSettings.ApplicationSettings;

            BindKeyList();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(txtKey.Text))
            {
                if (_appSettings.Contains(txtKey.Text))
                {
                    _appSettings[txtKey.Text] = txtValue.Text;
                }
                else
                {
                    _appSettings.Add(txtKey.Text, txtValue.Text);
                }
                _appSettings.Save();
                BindKeyList();
            }
        }


        private void BindKeyList()
        {
            lstKeys.Items.Clear();
            foreach (string key in _appSettings.Keys)
            {
                lstKeys.Items.Add(key);
            }
            txtValue.Text = "";
            txtKey.Text = "";
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (lstKeys.SelectedIndex > -1)
            {
                _appSettings.Remove(lstKeys.SelectedItem.ToString());
                _appSettings.Save();
                BindKeyList();
            }
        }

        private void lstKeys_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                string key = e.AddedItems[0].ToString();
                if (_appSettings.Contains(key))
                {
                    txtKey.Text = key;
                    txtValue.Text = _appSettings[key].ToString();
                }
            }
        }
    }
}