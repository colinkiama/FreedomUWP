using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace FreedomUWP.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AuthView : Page
    {
        Uri startUri;
        Uri callbackUri;
        public AuthView()
        {
            this.InitializeComponent();
            mainWebView.NavigationStarting += MainWebView_NavigationStarting;
        }

        private void MainWebView_NavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            if (args.Uri.AbsolutePath == callbackUri.AbsolutePath)
            {
                string query = args.Uri.Query;
                Debug.WriteLine(query);
            }
        }

        internal string Authenticate(string startURL, string callbackURL)
        {
            string result = "null";
            startUri = new Uri(startURL);
            callbackUri = new Uri(callbackURL);
            mainWebView.Navigate(startUri);
            
            return result;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Shell.RequestAuthViewClose();
        }
    }
}
