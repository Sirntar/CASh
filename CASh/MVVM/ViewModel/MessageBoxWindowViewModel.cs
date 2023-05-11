using System;
using System.Collections.Generic;
using CASh.MVVM.Model;
using CASh.Core;
using System.ComponentModel;

namespace CASh.MVVM.ViewModel
{
    enum ActionStatus
    {
         ERROR = -1,
        CANCEL =  0,
            OK =  1
    }

    class MessageBoxWindowViewModel : ObservableObject
    {
        private string _title;

        public string Title
        {
            get { return _title; }
            set { 
                _title = value;
                OnPropertyChanged();
            }
        }

        private string _message;

        public string Message
        {
            get { return _message; }
            set { 
                _message = value;
                OnPropertyChanged();
            }
        }

        private ActionStatus _action;

        public ActionStatus Action
        {
            get { return _action; }
            set { 
                _action = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand CancelCommand { get; set; }
        public RelayCommand OKCommand { get; set; }


        public MessageBoxWindowViewModel(string title, string message)
        {
            _title = title;
            _message = message;
            _action = ActionStatus.ERROR;

            CancelCommand = new RelayCommand(window =>
            {
                Action = ActionStatus.CANCEL;
                (window as System.Windows.Window)?.Close();
            });

            OKCommand = new RelayCommand(window =>
            {
                Action = ActionStatus.OK;
                (window as System.Windows.Window)?.Close();
            });
        }
    }
}
