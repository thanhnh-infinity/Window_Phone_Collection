﻿#pragma checksum "C:\Users\user\documents\visual studio 2010\Projects\Example_5_MediaPlayerDemo\Example_5_MediaPlayerDemo\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "8BB08A0C8CA2295D34946FD2E69E3FF7"
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


namespace Example_5_MediaPlayerDemo {
    
    
    public partial class MainPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.StackPanel TitlePanel;
        
        internal System.Windows.Controls.TextBlock ApplicationTitle;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal System.Windows.Controls.TextBlock lblURL;
        
        internal System.Windows.Controls.TextBox txtURL;
        
        internal System.Windows.Controls.TextBlock lblBufferingTime;
        
        internal System.Windows.Controls.TextBox txtBufferingTime;
        
        internal System.Windows.Controls.MediaElement mediaPlayer;
        
        internal System.Windows.Controls.Button btnPlay;
        
        internal System.Windows.Controls.Button btnPause;
        
        internal System.Windows.Controls.Button btnStop;
        
        internal System.Windows.Controls.Slider mediaTimeLine;
        
        internal System.Windows.Controls.TextBlock lblStatus;
        
        internal System.Windows.Controls.TextBlock lblBuffering;
        
        internal System.Windows.Controls.TextBlock lblDowloading;
        
        internal System.Windows.Controls.Button btnMute;
        
        internal System.Windows.Controls.TextBlock lblSoundStatus;
        
        internal System.Windows.Controls.Button btnMediaPlayerLauncher;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/Example_5_MediaPlayerDemo;component/MainPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.TitlePanel = ((System.Windows.Controls.StackPanel)(this.FindName("TitlePanel")));
            this.ApplicationTitle = ((System.Windows.Controls.TextBlock)(this.FindName("ApplicationTitle")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.lblURL = ((System.Windows.Controls.TextBlock)(this.FindName("lblURL")));
            this.txtURL = ((System.Windows.Controls.TextBox)(this.FindName("txtURL")));
            this.lblBufferingTime = ((System.Windows.Controls.TextBlock)(this.FindName("lblBufferingTime")));
            this.txtBufferingTime = ((System.Windows.Controls.TextBox)(this.FindName("txtBufferingTime")));
            this.mediaPlayer = ((System.Windows.Controls.MediaElement)(this.FindName("mediaPlayer")));
            this.btnPlay = ((System.Windows.Controls.Button)(this.FindName("btnPlay")));
            this.btnPause = ((System.Windows.Controls.Button)(this.FindName("btnPause")));
            this.btnStop = ((System.Windows.Controls.Button)(this.FindName("btnStop")));
            this.mediaTimeLine = ((System.Windows.Controls.Slider)(this.FindName("mediaTimeLine")));
            this.lblStatus = ((System.Windows.Controls.TextBlock)(this.FindName("lblStatus")));
            this.lblBuffering = ((System.Windows.Controls.TextBlock)(this.FindName("lblBuffering")));
            this.lblDowloading = ((System.Windows.Controls.TextBlock)(this.FindName("lblDowloading")));
            this.btnMute = ((System.Windows.Controls.Button)(this.FindName("btnMute")));
            this.lblSoundStatus = ((System.Windows.Controls.TextBlock)(this.FindName("lblSoundStatus")));
            this.btnMediaPlayerLauncher = ((System.Windows.Controls.Button)(this.FindName("btnMediaPlayerLauncher")));
        }
    }
}

