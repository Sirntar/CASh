using System;
using System.Collections.Generic;
using CASh.MVVM.Model;
using CASh.Core;
using System.ComponentModel;
using CASh.MVVM.View;

namespace CASh.MVVM.ViewModel
{
    class ClientViewModel : ObservableObject
    {
        public event PropertyChangedEventHandler? SelectionChanged;

        private void onSelectionChanged(object? sender, PropertyChangedEventArgs e)
        {
            SelectionChanged?.Invoke(sender, e);
        }

        private BusinessCustomerViewModel B2B_VM { get; set; }
        private CustomerViewModel B2C_VM { get; set; }

        private object _currentDataView;

        public object CurrentDataView
        {
            get { return _currentDataView; }
            set { 
                _currentDataView = value;
                OnPropertyChanged();
            }
        }

        private string _searchText = "";

        public string SearchText
        {
            get { return _searchText; }
            set { 
                _searchText = value;
                OnPropertyChanged();

                if(_currentDataView == B2B_VM)
                    (CurrentDataView as BusinessCustomerViewModel)?.Search(value);
                else
                    (CurrentDataView as CustomerViewModel)?.Search(value);
            }
        }


        public RelayCommand AddClientCommand { get; set; }

        public ClientViewModel()
        {
            B2B_VM = new BusinessCustomerViewModel();
            B2C_VM = new CustomerViewModel();

            B2B_VM.PropertyChanged += onSelectionChanged;
            B2C_VM.PropertyChanged += onSelectionChanged;

            _currentDataView = B2B_VM;

            AddClientCommand = new RelayCommand((o) =>
            {
                ClientBoxWindowView clientBox = new();

                bool isB2B = _currentDataView == B2B_VM;
                object newClient;

                if (isB2B)
                    newClient = new BusinessCustomerModel() { Id = -1, Name = "CompanyName", Email = "email", PhoneNumber = "00000000", TotalMoneySpend = 0, VATIN = "NL999999999B01", IBAN = "PL12 1234 1234 1234 1234 1234 1234", OrderCounter = 0, RmaCounter = 0, CreatedAt = new DateTime(2022, 3, 8), UpdatedAt = new DateTime(2022, 3, 8) };
                else
                    newClient = new CustomerModel() { Id = -1, FirstName = "Name", LastName = "LastName", Email = "email", PhoneNumber = "00000000", TotalMoneySpend = 0, PESEL = "12345612345", IBAN = "PL12 1234 1234 1234 1234 1234 1234", OrderCounter = 0, RmaCounter = 0, CreatedAt = new DateTime(2022, 3, 8), UpdatedAt = new DateTime(2022, 3, 8) };

                clientBox.DataContext = new ClientBoxWindowViewModel(newClient, isB2B, true);

                clientBox.Closed += (obj, args) =>
                {
                    if (_currentDataView == B2B_VM)
                    {
                        changeCurrentDataViewToB2B();
                    } 
                    else
                    {
                        changeCurrentDataViewToB2C();
                    }
                };
            });
        }

        public void changeCurrentDataViewToB2B()
        {
            if(CurrentDataView != B2B_VM)
                CurrentDataView = B2B_VM;
            B2B_VM.BusinessCustomersList = BusinessCustomerModel.FetchAll();
            B2B_VM.Selected = null;
            SearchText = "";
        }

        public void changeCurrentDataViewToB2C()
        {
            if (CurrentDataView != B2C_VM)
                CurrentDataView = B2C_VM;
            B2C_VM.CustomersList = CustomerModel.FetchAll();
            B2C_VM.Selected = null;
            SearchText = "";
        }
    }
}
