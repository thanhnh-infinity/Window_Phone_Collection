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
using Microsoft.Web.Media.SmoothStreaming;

namespace iTVOD_WindowPhone7
{
    public partial class Live_Channel_Player : PhoneApplicationPage
    {
        public Live_Channel_Player()
        {
            InitializeComponent();
            /*
            liveChannelPlayer.SmoothStreamingErrorOccurred += (s, e) =>
                {
                    lblDowloadingTime.Visibility = Visibility.Visible;
                    lblDowloadingTime.Text = "Lỗi nguồn kênh. Nhấn Back để thoát";
                };
            liveChannelPlayer.MediaFailed += (s, e) =>
                {
                    lblDowloadingTime.Visibility = Visibility.Visible;
                    lblDowloadingTime.Text = "Lỗi nguồn kênh. Nhấn Back để thoát";
                };
            liveChannelPlayer.MediaOpened += (s, e) =>
                {
                    lblDowloadingTime.Visibility = Visibility.Collapsed;
                };
            liveChannelPlayer.ManifestReady += (s, e) =>
                {
                    lblDowloadingTime.Visibility = Visibility.Collapsed;
                };
             */

            
            
            liveChannelPlayer.BufferingProgressChanged += (s, e) =>
             {
                  /*
                  if (liveChannelPlayer.CurrentState == SmoothStreamingMediaElementState.Playing) {
                      lblDowloadingTime.Visibility = Visibility.Collapsed;
                  }
                  else if (liveChannelPlayer.CurrentState == SmoothStreamingMediaElementState.Buffering)
                  {
                      lblDowloadingTime.Visibility = Visibility.Visible;
                      lblDowloadingTime.Text = "Loading.......";
                  }
                   */
                 //lblDowloading.Text = liveChannelPlayer.BufferingProgress.ToString() ;

             };
             

        }
         
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            string live_channel_id = "";
            string live_channel_url = "";
            string live_channel_folder = "";
            string msg = "";
            if (NavigationContext.QueryString.TryGetValue("live_channel_id", out msg))
            {
                live_channel_id = msg;
            }
            if (NavigationContext.QueryString.TryGetValue("live_channel_url", out msg))
            {
                live_channel_url = msg;
            }
            if (NavigationContext.QueryString.TryGetValue("live_channel_folder", out msg))
            {
                live_channel_folder = msg;
            }
            live_channel_url += "/manifest";
            //liveChannelPlayer.SmoothStreamingSource = new Uri("http://dsti.vn/movies/bigbugbunny.ssm/manifest");
            liveChannelPlayer.SmoothStreamingSource = new Uri(live_channel_url);
            //liveChannelPlayer.SmoothStreamingSource = new Uri("http://vodpack.unified-streaming.com/video/oceans/oceans.ism/Manifest");    
        }
        
        
    }
}