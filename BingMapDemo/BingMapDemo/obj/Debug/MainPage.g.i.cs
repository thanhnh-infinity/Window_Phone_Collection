﻿#pragma checksum "c:\users\user\documents\visual studio 2010\Projects\BingMapDemo\BingMapDemo\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "26C857E032283ECE9A300D919D720709"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.269
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using Microsoft.Phone.Controls.Maps;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace BingMapDemo {
    
    
    public partial class MainPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Media.Animation.Storyboard BlinkLocator;
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.StackPanel TitlePanel;
        
        internal System.Windows.Controls.TextBlock ApplicationTitle;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal Microsoft.Phone.Controls.Maps.Map bingMap;
        
        internal Microsoft.Phone.Controls.Maps.Pushpin bingMapLocator;
        
        internal System.Windows.Shapes.Ellipse locator;
        
        internal System.Windows.Controls.Button Start;
        
        internal System.Windows.Controls.TextBox txtStatus;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/BingMapDemo;component/MainPage.xaml", System.UriKind.Relative));
            this.BlinkLocator = ((System.Windows.Media.Animation.Storyboard)(this.FindName("BlinkLocator")));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.TitlePanel = ((System.Windows.Controls.StackPanel)(this.FindName("TitlePanel")));
            this.ApplicationTitle = ((System.Windows.Controls.TextBlock)(this.FindName("ApplicationTitle")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.bingMap = ((Microsoft.Phone.Controls.Maps.Map)(this.FindName("bingMap")));
            this.bingMapLocator = ((Microsoft.Phone.Controls.Maps.Pushpin)(this.FindName("bingMapLocator")));
            this.locator = ((System.Windows.Shapes.Ellipse)(this.FindName("locator")));
            this.Start = ((System.Windows.Controls.Button)(this.FindName("Start")));
            this.txtStatus = ((System.Windows.Controls.TextBox)(this.FindName("txtStatus")));
        }
    }
}

