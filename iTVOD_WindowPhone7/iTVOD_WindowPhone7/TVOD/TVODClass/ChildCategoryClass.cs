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
    public class ChildCategoryClass
    {
        public String category_id
        {
            get;
            set;
        }

        public String category_name
        {
            get;
            set;
        }
        public String category_image
        {
            get;
            set;
        }
        public String number_video_category
        {
            get;
            set;
        }
    }

    public class RootChildCategoryClass
    {
        public ChildCategoryClass[] items { get; set; }
        public Boolean success { get; set; }
        public String quantity { get; set; }

    }
}
