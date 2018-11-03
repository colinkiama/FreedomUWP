﻿using FreedomUWP.View;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace FreedomUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Shell : Page
    {
        static event EventHandler<string[]> AuthViewRequested;
        public Shell()
        {
            this.InitializeComponent();
            AuthViewFrame.Visibility = Visibility.Collapsed;
            ContentFrame.Navigate(typeof(LoginView));
        }


        

        internal void AddAuthView(AuthView authView)
        {
            AuthViewFrame.Visibility = Visibility.Visible;
            AuthViewFrame.Content = authView;
        }

        internal static void RequestAuthViewClose()
        {
        }
    }
}
