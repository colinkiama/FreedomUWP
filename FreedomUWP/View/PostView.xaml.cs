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

namespace FreedomUWP.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PostView : Page
    {
        public PostView()
        {
            this.InitializeComponent();
            ContentRichEditBox.Loaded += ContentRichEditBox_Loaded;
        }

        private void ContentRichEditBox_Loaded(object sender, RoutedEventArgs e)
        {
            var reb = (RichEditBox)sender;
            reb.Focus(FocusState.Programmatic);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }
        private void EditTitleButton_Click(object sender, RoutedEventArgs e)
        {
            
            TitleStackPanel.Visibility = Visibility.Collapsed;
            EditTitleTextBox.Visibility = Visibility.Visible;
            EditTitleTextBox.Focus(FocusState.Programmatic);
            EditTitleTextBox.SelectAll();
        }

        private void EditTitleTextBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                EditTitleTextBox.Visibility = Visibility.Collapsed;
                TitleStackPanel.Visibility = Visibility.Visible;
            }
        }

        private void ContentRichEditBox_TextChanged(object sender, RoutedEventArgs e)
        {
            var reb = (RichEditBox)sender;
            string richEditBoxContent = "";
            reb.Document.GetText(Windows.UI.Text.TextGetOptions.None, out richEditBoxContent);
            PublishArticleButton.CommandParameter = richEditBoxContent;
        }
    }
}
