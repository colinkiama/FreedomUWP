using FreedomUWP.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Medium.Models;
using FreedomUWP.Helpers;
using System.Diagnostics;

namespace FreedomUWP.ViewModel
{
    class HomeViewModel : NotifierClass
    {
        #region Properties
        private ObservableCollection<Post> _posts;

        public ObservableCollection<Post> Posts
        {
            get { return _posts; }
            set { _posts = value; }
        }

        private User _currentUser;

        public User CurrentUser
        {
            get { return _currentUser; }
            set { _currentUser = value; }
        }

        #endregion
        public HomeViewModel()
        {
            
        }
    }
}
