﻿#pragma checksum "C:\Users\user\documents\visual studio 2010\Projects\iTVOD_WindowPhone7\iTVOD_WindowPhone7\SearchActivity.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "1C52334C871FFFE18A1E8DD49A1523F8"
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


namespace iTVOD_WindowPhone7 {
    
    
    public partial class SearchActivity : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.StackPanel TitlePanel;
        
        internal System.Windows.Controls.TextBlock lblTitle;
        
        internal System.Windows.Controls.TextBox txtKeyWord;
        
        internal System.Windows.Controls.Button btnSearch;
        
        internal System.Windows.Controls.ListBox lstVideoSearching;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/iTVOD_WindowPhone7;component/SearchActivity.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.TitlePanel = ((System.Windows.Controls.StackPanel)(this.FindName("TitlePanel")));
            this.lblTitle = ((System.Windows.Controls.TextBlock)(this.FindName("lblTitle")));
            this.txtKeyWord = ((System.Windows.Controls.TextBox)(this.FindName("txtKeyWord")));
            this.btnSearch = ((System.Windows.Controls.Button)(this.FindName("btnSearch")));
            this.lstVideoSearching = ((System.Windows.Controls.ListBox)(this.FindName("lstVideoSearching")));
        }
    }
}

