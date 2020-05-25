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

namespace Example_2_CurrencyConversion
{
    public partial class MoreStuff : PhoneApplicationPage
    {
        Double dblExchgRate;
        Decimal decTotalToConvert;

        public MoreStuff()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            string strExchangeRate = "";
            string strTotalToConvert = "";
            if (NavigationContext.QueryString.TryGetValue("rate", out strExchangeRate))
            {
                dblExchgRate = Convert.ToDouble(strExchangeRate);
            }

            if (NavigationContext.QueryString.TryGetValue("total", out strTotalToConvert))
            {
                decTotalToConvert = Convert.ToDecimal(strTotalToConvert);
            }
        }

        private void btnBackToMain_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void btnCalculateDamage_Click(object sender, RoutedEventArgs e)
        {
            decimal decTotalToReceive;
            decimal decTotalAccordingToConversionRate;

            decTotalToReceive = Convert.ToDecimal(txtEnchangeRateQuoted.Text) * decTotalToConvert;
            decTotalAccordingToConversionRate = Convert.ToDecimal(dblExchgRate) * decTotalToConvert;

            txtDamageExplained.Text = "With exchange rate quoted, you will receive " + decTotalToReceive.ToString() + "\r\n";
            txtDamageExplained.Text = txtDamageExplained.Text + "Given market exchange rate, you should receive " + decTotalAccordingToConversionRate.ToString() + "\r\n";
            txtDamageExplained.Text = txtDamageExplained.Text + "You lose " + (decTotalAccordingToConversionRate - decTotalToReceive).ToString();
        }


    }
}