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
    public class TransactionClass
    {
        public String transaction_id
        {
            get;
            set;
        }
        public String transaction_date
        {
            get;
            set;
        }
        public String transaction_value
        {
            get;
            set;
        }
        public String stop_time
        {
            get;
            set;
        }
        public String content_picture_path
        {
            get;
            set;
        }
        public String content_id
        {
            get;
            set;
        }
        public String content_name
        {
            get;
            set;
        }
    }

    public class RootTransactionClass
    {
        public TransactionClass[] items { get; set; }
        public Boolean success { get; set; }
        public String quantity { get; set; }
        public String type { get; set; }
        public String total_quantity { get; set; }

    }
}
