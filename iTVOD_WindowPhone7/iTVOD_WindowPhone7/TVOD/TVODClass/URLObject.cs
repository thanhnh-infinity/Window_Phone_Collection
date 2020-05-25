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
    public class URLObject
    {
        public Boolean success
        {
            get;
            set;
        }
        public string url
        {
            get;
            set;
        }
        public string subtitle
        {
            get;
            set;
        }
        public string reason
        {
            get;
            set;
        }
        public string type
        {
            get;
            set;
        }
    }
}
