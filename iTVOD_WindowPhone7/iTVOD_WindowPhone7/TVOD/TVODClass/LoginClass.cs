﻿using System;
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
    public class LoginClass
    {
        public Boolean success
        {
            set;
            get;
        }
        public string sessionID
        {
            set;
            get;
        }
        public string user_name
        {
            set;
            get;
        }
        public string password
        {
            set;
            get;
        }
    }
}
