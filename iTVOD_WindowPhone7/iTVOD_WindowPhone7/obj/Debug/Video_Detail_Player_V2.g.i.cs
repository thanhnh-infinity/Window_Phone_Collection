﻿#pragma checksum "C:\Users\user\documents\visual studio 2010\Projects\iTVOD_WindowPhone7\iTVOD_WindowPhone7\Video_Detail_Player_V2.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "9519B0890F8AE02E6AD149CA13D31FF6"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.296
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


namespace iTVOD_WindowPhone7 {
    
    
    public partial class Video_Detail_Player_V2 : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Grid ViewVideo;
        
        internal System.Windows.Controls.StackPanel TitlePanel;
        
        internal System.Windows.Controls.TextBlock lblVideoTitle;
        
        internal System.Windows.Controls.ProgressBar pBar;
        
        internal System.Windows.Controls.Image imgVideo;
        
        internal System.Windows.Controls.TextBlock lblEnglishTitle;
        
        internal System.Windows.Controls.TextBlock lblVietnameseTitle;
        
        internal System.Windows.Controls.TextBlock lblSoLuotXem;
        
        internal System.Windows.Controls.Button btnViewVideo;
        
        internal System.Windows.Controls.Button btnSendGift;
        
        internal System.Windows.Controls.TextBlock lblVideoPrice;
        
        internal System.Windows.Controls.Button btnFavourite;
        
        internal System.Windows.Controls.TextBlock lblDetailVideo;
        
        internal System.Windows.Controls.ListBox lstComments;
        
        internal System.Windows.Controls.TextBox txtCommentBody;
        
        internal System.Windows.Controls.Button btnAddComment;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/iTVOD_WindowPhone7;component/Video_Detail_Player_V2.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.ViewVideo = ((System.Windows.Controls.Grid)(this.FindName("ViewVideo")));
            this.TitlePanel = ((System.Windows.Controls.StackPanel)(this.FindName("TitlePanel")));
            this.lblVideoTitle = ((System.Windows.Controls.TextBlock)(this.FindName("lblVideoTitle")));
            this.pBar = ((System.Windows.Controls.ProgressBar)(this.FindName("pBar")));
            this.imgVideo = ((System.Windows.Controls.Image)(this.FindName("imgVideo")));
            this.lblEnglishTitle = ((System.Windows.Controls.TextBlock)(this.FindName("lblEnglishTitle")));
            this.lblVietnameseTitle = ((System.Windows.Controls.TextBlock)(this.FindName("lblVietnameseTitle")));
            this.lblSoLuotXem = ((System.Windows.Controls.TextBlock)(this.FindName("lblSoLuotXem")));
            this.btnViewVideo = ((System.Windows.Controls.Button)(this.FindName("btnViewVideo")));
            this.btnSendGift = ((System.Windows.Controls.Button)(this.FindName("btnSendGift")));
            this.lblVideoPrice = ((System.Windows.Controls.TextBlock)(this.FindName("lblVideoPrice")));
            this.btnFavourite = ((System.Windows.Controls.Button)(this.FindName("btnFavourite")));
            this.lblDetailVideo = ((System.Windows.Controls.TextBlock)(this.FindName("lblDetailVideo")));
            this.lstComments = ((System.Windows.Controls.ListBox)(this.FindName("lstComments")));
            this.txtCommentBody = ((System.Windows.Controls.TextBox)(this.FindName("txtCommentBody")));
            this.btnAddComment = ((System.Windows.Controls.Button)(this.FindName("btnAddComment")));
        }
    }
}
