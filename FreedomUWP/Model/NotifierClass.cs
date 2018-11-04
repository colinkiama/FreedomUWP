﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;


namespace FreedomUWP.Model
{
    public abstract class NotifierClass : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        // "CallerMemberName" lets you call this method without having to included the properties name
        // by yourself e.g "NotifyPropertyChanged();"
        public void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
