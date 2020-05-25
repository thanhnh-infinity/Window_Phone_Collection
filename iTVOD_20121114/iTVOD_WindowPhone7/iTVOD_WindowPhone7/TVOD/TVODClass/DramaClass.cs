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
    public class DramaClass
    {
        public String drama_id
        {
            get;
            set;
        }

        public String drama_english_title
        {
            get;
            set;
        }
        public String drama_vietnamese_title
        {
            get;
            set;
        }
        public String drama_quantity
        {
            get;
            set;
        }
        public String drama_image_path
        {
            get;
            set;
        }
       
    }

    public class RootDramaClass
    {
        public DramaClass[] items { get; set; }
        public Boolean success { get; set; }
        public String quantity { get; set; }
        public String type { get; set; }
        public String total_quantity { get; set; }

    }
}
