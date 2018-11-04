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

        public RelayCommand<string> PostCommand;

        public PostViewModel()
        {
            CurrentUser = App.mediumClient.GetCurrentUser(TokenHelper.GetToken());
            Title = "New Post";
            PostCommand = new RelayCommand<string>(PostArticle);
        }

        private void PostArticle(string articleContent)
        {
            Post publishedPost = App.mediumClient.CreatePost(CurrentUser.Id,
                 new CreatePostRequestBody()
                 {
                     Title = Title,
                     Content = articleContent,
                     ContentFormat = ContentFormat.Html,
                     PublishStatus = PublishStatus.Draft,
                 },
                 TokenHelper.GetToken());

            Debug.WriteLine($"Post available in: {publishedPost.Url}");
        }
    }
}
