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
    public class VideoClass
    {
        public String video_id
        {
            get;
            set;
        }

        public String video_english_title
        {
            get;
            set;
        }
        public String video_vietnamese_title
        {
            get;
            set;
        }
        public String video_number_views
        {
            get;
            set;
        }
        public String video_duration
        {
            get;
            set;
        }
        public String video_description
        {
            get;
            set;
        }
        public String video_picture_path
        {
            get;
            set;
        }
        public String video_price
        {
            get;
            set;
        }
    }

    public class RootVideoClass
    {
        public VideoClass[] items { get; set; }
        public Boolean success { get; set; }
        public String quantity { get; set; }
        public String type { get; set; }
        public String total_quantity { get; set; }

    }
}
