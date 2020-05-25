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
    public class CommentClass
    {
        public String comment_id
        {
            get;
            set;
        }

        public String user_id
        {
            get;
            set;
        }

        public String name
        {
            get;
            set;
        }

        public String subject
        {
            get;
            set;
        }

        public String body_value
        {
            get;
            set;
        }
    }

    public class RootCommentClass
    {
        public CommentClass[] items { get; set; }
        public Boolean success { get; set; }
        public String quantity { get; set; }
        public String total_quantity { get; set; }

    }
}
