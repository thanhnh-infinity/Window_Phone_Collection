using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace iTVOD_WindowPhone7.TVOD.TVODClass
{
    public class LiveChannelClass
    {
        public String live_channel_id
        {
            get;
            set;
        }

        public String live_channel_title
        {
            get;
            set;
        }
        public String live_channel_number_view
        {
            get;
            set;
        }
        public String live_channel_folder
        {
            get;
            set;
        }
        public String live_channel_url
        {
            get;
            set;
        }
        public String video_picture_path
        {
            get;
            set;
        }
    }

    public class RootLiveChannelClass
    {
        public LiveChannelClass[] items { get; set; }
        public Boolean success { get; set; }
        public String quantity { get; set; }
        public String type { get; set; }
        public String total_quantity { get; set; }

    }
}
