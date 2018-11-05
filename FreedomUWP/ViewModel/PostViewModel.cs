using FreedomUWP.Commands;
using FreedomUWP.Helpers;
using FreedomUWP.Model;
using Medium.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;

namespace FreedomUWP.ViewModel
{
    class PostViewModel : NotifierClass
    {
        #region Properties
        private User _currentUser;

        public User CurrentUser
        {
            get { return _currentUser; }
            set
            {
                _currentUser = value;
                NotifyPropertyChanged();
            }
        }
        #endregion

        private string _title;

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                NotifyPropertyChanged();
            }
        }


        public RelayCommand<string> ViewPostCommand;
        public RelayCommand<string> PostPublicCommand;
        public RelayCommand<string> PostUnlistedCommand;
        public RelayCommand<string> PostDraftCommand;
        public RelayCommand LogOutCommand;

        public PostViewModel()
        {
            CurrentUser = App.mediumClient.GetCurrentUser(TokenHelper.GetToken());
            Title = "New Post";
            PostPublicCommand = new RelayCommand<string>(PostPublicArticle);
            PostUnlistedCommand = new RelayCommand<string>(PostUnlistedArticle);
            PostDraftCommand = new RelayCommand<string>(PostDraft);
            LogOutCommand = new RelayCommand(LogOut);
            ViewPostCommand = new RelayCommand<string>(ViewPublishedPost);
        }

        private async void ViewPublishedPost(string publishedPostURL)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri(publishedPostURL));
        }

        private void LogOut()
        {
            TokenHelper.ClearTokenSettings();
        }

        private async void PostPublicArticle(string articleContent)
        {
            Post publishedPost = App.mediumClient.CreatePost(CurrentUser.Id,
                 new CreatePostRequestBody()
                 {
                     Title = Title,
                     Content = articleContent,
                     ContentFormat = ContentFormat.Html,
                     PublishStatus = PublishStatus.Public,
                 },
                 TokenHelper.GetToken());

           await ShowPublishedArticlePopup(publishedPost.Url);
        }

        private async void PostDraft(string draftContent)
        {
            Post publishedPost = App.mediumClient.CreatePost(CurrentUser.Id,
                new CreatePostRequestBody()
                {
                    Title = Title,
                    Content = draftContent,
                    ContentFormat = ContentFormat.Html,
                    PublishStatus = PublishStatus.Draft,
                },
                TokenHelper.GetToken());

            await ShowPublishedArticlePopup(publishedPost.Url);
        }


        private async void PostUnlistedArticle(string articleContent)
        {
            Post publishedPost = App.mediumClient.CreatePost(CurrentUser.Id,
                 new CreatePostRequestBody()
                 {
                     Title = Title,
                     Content = articleContent,
                     ContentFormat = ContentFormat.Html,
                     PublishStatus = PublishStatus.Unlisted,
                 },
                 TokenHelper.GetToken());

            await ShowPublishedArticlePopup(publishedPost.Url);
        }

        private async Task ShowPublishedArticlePopup(string url)
        {
            var dialog = new ContentDialog
            {
                Title = "Article Published",
                Content = $"Post available in: {url}",
                CloseButtonText = "Ok",
                PrimaryButtonText = "Go to URL",
                PrimaryButtonCommand = ViewPostCommand,
                PrimaryButtonCommandParameter = url
            };

            await dialog.ShowAsync();
        }
    }
}
