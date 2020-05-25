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
using Microsoft.SilverlightMediaFramework.Core.Media;

namespace iTVOD_WindowPhone7
{
    public partial class Live_Channel_PLayer_V2 : PhoneApplicationPage
    {
        public Live_Channel_PLayer_V2()
        {
            InitializeComponent();
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
            PlaylistItem item = new PlaylistItem();
            item.MediaSource = new Uri(live_channel_url);
            item.DeliveryMethod = Microsoft.SilverlightMediaFramework.Plugins.Primitives.DeliveryMethods.Streaming;
            strmPlayer.Playlist.Add(item);
            strmPlayer.Play();
            
        }
        
    }
}