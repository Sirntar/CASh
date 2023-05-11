using CASh.Core;
using System.ComponentModel;
using System.Windows.Input;
using CASh.MVVM.View;
using CASh.Core.DBClient;
using Microsoft.Data.Sqlite;
using System;
using CASh.MVVM.Model;

namespace CASh.MVVM.ViewModel
{
    class MainViewModel : ObservableObject 
    {
        private SQLQuery? _sqlQuery;

        private bool _isB2B = true;
        private object? _selectedClient;
        public ClientViewModel ClientVM { get; set; }

        private object? _currentView;

        public object? CurrentView
        {
            get { return _currentView; }
            set { 
                _currentView = value; 
                OnPropertyChanged();
            }
        }

        private string _isExtraToolsPlaceholderVisable;

        public string IsExtraToolsPlaceholderVisable
        {
            get { return _isExtraToolsPlaceholderVisable; }
            set { 
                _isExtraToolsPlaceholderVisable = value;
                OnPropertyChanged();
            }
        }

        private string _isExtraToolsVisable;

        public string IsExtraToolsVisable
        {
            get { return _isExtraToolsVisable; }
            set {
                _isExtraToolsVisable = value;
                OnPropertyChanged();
            }
        }

        private ClientBoxWindowViewModel _clientBoxWindowViewModel;

        private void onSelectionChanged(object? sender, PropertyChangedEventArgs? e)
        {
            if (sender != null)
            {
                IsExtraToolsPlaceholderVisable = "Hidden";
                IsExtraToolsVisable = "Visible";

                if (_isB2B)
                    _selectedClient = (sender as BusinessCustomerViewModel)?.Selected;
                else
                    _selectedClient = (sender as CustomerViewModel)?.Selected;

                _clientBoxWindowViewModel.SelectedClient = _selectedClient;
            }
            else
            {
                _selectedClient = null;
                IsExtraToolsPlaceholderVisable = "Visible";
                IsExtraToolsVisable = "Hidden";
            }
        }

        public RelayCommand ChangeDataViewToB2BCommand { get; set; }
        public RelayCommand ChangeDataViewToB2CCommand { get; set; }

        public RelayCommand DeleteClientCommand { get; set; }
        public RelayCommand EditClientCommand { get; set; }
        public RelayCommand ShowClientCommand { get; set; }

        public MainViewModel()
        {
            // SQL Connection
            _sqlQuery = new(DatabaseInterface.SQLite, new SqliteConnectionStringBuilder()
            {
                DataSource = Environment.CurrentDirectory + "/database.db",
                Mode = SqliteOpenMode.ReadWriteCreate,
                DefaultTimeout = 0
            }.ToString());

            // Set sql access to objects
            AbstractModel.SQLQuery = _sqlQuery;

            ClientVM = new ClientViewModel();
            CurrentView = ClientVM;

            ChangeDataViewToB2BCommand = new RelayCommand(o => {
                ClientVM.changeCurrentDataViewToB2B();
                _isB2B = true;
                onSelectionChanged(null, null);
            });
            
            ChangeDataViewToB2CCommand = new RelayCommand(o => {
                ClientVM.changeCurrentDataViewToB2C();
                _isB2B = false;
                onSelectionChanged(null, null);
            });

            DeleteClientCommand = new RelayCommand(deleteClient);
            EditClientCommand = new RelayCommand(showClientBox);
            ShowClientCommand = new RelayCommand(showClientBox);

            _isExtraToolsVisable = "Hidden";
            _isExtraToolsPlaceholderVisable = "Visible";

            ClientVM.SelectionChanged += onSelectionChanged;

            _clientBoxWindowViewModel = new(_selectedClient, _isB2B);
        }

        private void deleteClient(object? o)
        {
            if (MessageBoxWindowView.Instance != null)
                return;

            MessageBoxWindowView messageBox = new();
            MessageBoxWindowViewModel messageBoxVM = new("DELETE Client", "WARNING: Your action will be permament!");

            messageBox.DataContext = messageBoxVM;

            messageBoxVM.PropertyChanged += (s, e) =>
            {
                if(e.PropertyName == "Action")
                {
                    ActionStatus? status = (s as MessageBoxWindowViewModel)?.Action;

                    if(status != null)
                        switch (status)
                        {
                            case ActionStatus.OK:
                                if(_isB2B)
                                    (_selectedClient as BusinessCustomerModel)?.Delete();
                                else
                                    (_selectedClient as CustomerModel)?.Delete();
                                break;
                            default:
                                break;
                        }
                }
            };
        }

        private void showClientBox(object? o)
        {
            ClientBoxWindowView clientBox = new();
            _clientBoxWindowViewModel = new(_selectedClient, _isB2B);

            clientBox.DataContext = _clientBoxWindowViewModel;
        }
    }
}
