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
using Microsoft.Phone.Reactive;
using Microsoft.Phone.Marketplace;

namespace Example_2_CurrencyConversion
{
    public partial class MainPage : PhoneApplicationPage
    {
        svcCurrencyConverter.CurrencyConvertorSoapClient currencyClient = new svcCurrencyConverter.CurrencyConvertorSoapClient();
        Double dblRate = 0.0;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            
            LoadCurrencies();
            try
            {
                var currency = Observable.FromEvent<svcCurrencyConverter.ConversionRateCompletedEventArgs>(currencyClient, "ConversionRateCompleted");
                currency.ObserveOn(Deployment.Current.Dispatcher).Subscribe(evt =>
                    {
                        dblRate = evt.EventArgs.Result;
                        txtStatus.Text = "The current rate is 1 " + lstConvertFrom.SelectedItem.ToString() + " to " + evt.EventArgs.Result.ToString() + " " + lstConvertTo.SelectedItem.ToString();
                        if (txtAmountToConvert.Text.Length > 0)
                        {
                            Double decTotal = evt.EventArgs.Result * Convert.ToDouble(txtAmountToConvert.Text);
                            txtTotalConverted.Text = txtAmountToConvert.Text + " " + lstConvertFrom.SelectedItem.ToString() + " = " + decTotal.ToString() + " " + lstConvertTo.SelectedItem.ToString();
                        }
                    },
                ex => { txtStatus.Text = "Sorry, we encountered a problem : " + ex.Message; }
                );
            }
            catch (Exception ex)
            {
                txtStatus.Text = ex.Message;
            }
        }

        private void btnConvert_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                currencyClient.ConversionRateAsync((svcCurrencyConverter.Currency)lstConvertFrom.SelectedItem, (svcCurrencyConverter.Currency)lstConvertTo.SelectedItem);
            }
            catch (Exception ex)
            {
                txtStatus.Text = ex.Message;
            }

        }

        private void btnMoreOptions_Click(object sender, RoutedEventArgs e)
        {
            LicenseInformation lic = new LicenseInformation();

            if (lic.IsTrial() == false)
            {
                NavigationService.Navigate(new Uri("/Upgrade.xaml", UriKind.RelativeOrAbsolute));
            }
            else
            {
                NavigationService.Navigate(new Uri("/MoreStuff.xaml?rate=" + dblRate.ToString() + "&total=" +  txtAmountToConvert.Text, UriKind.RelativeOrAbsolute));
            }
        }

        private void LoadCurrencies()
        {
            lstConvertFrom.Items.Add(svcCurrencyConverter.Currency.USD);
            lstConvertFrom.Items.Add(svcCurrencyConverter.Currency.EUR);
            lstConvertFrom.Items.Add(svcCurrencyConverter.Currency.RUB);

            lstConvertTo.Items.Add(svcCurrencyConverter.Currency.USD);
            lstConvertTo.Items.Add(svcCurrencyConverter.Currency.EUR);
            lstConvertTo.Items.Add(svcCurrencyConverter.Currency.RUB);

        }


    }   
}