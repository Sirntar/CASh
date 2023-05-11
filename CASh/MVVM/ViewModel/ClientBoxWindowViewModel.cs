using System;
using System.Collections.Generic;
using CASh.MVVM.Model;
using CASh.MVVM.View;
using CASh.Core;
using System.Reflection;

namespace CASh.MVVM.ViewModel
{
    class ClientBoxWindowViewModel : ObservableObject
    {
        private bool _isB2B;

        private object? _selectedClient;
        
        public object? SelectedClient
        {
            get { return _selectedClient; }
            set { 
                _selectedClient = value; 
                OnPropertyChanged();
                createNewDummy();
            }
        }

        private ObservablePairCollection<string, string>? _customer;
        private ObservablePairCollection<string, string>? _customerDummy;

        public ObservablePairCollection<string, string>? Customer
        {
            get { 
                return _customerDummy; 
            }
            set { 
                _customerDummy = value;
                OnPropertyChanged();
            }
        }

        private void createNewDummy()
        {
            if (_selectedClient == null)
                return;

            ObservablePairCollection<string, string> customerDummy = new ObservablePairCollection<string, string>();
            _customer = new ObservablePairCollection<string, string>();

            PropertyInfo[] infos = _selectedClient.GetType().GetProperties();

            foreach (PropertyInfo info in infos)
            {
                string? str = info.GetValue(_selectedClient, null)?.ToString();

                if (str == null)
                    str = "";

                customerDummy.Add(info.Name, str);
                _customer.Add(info.Name, str);
            }

            Customer = customerDummy;
        }

        public RelayCommand DeleteCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }
        public RelayCommand SaveCommand { get; set; }

        public ClientBoxWindowViewModel(object? cient, bool isB2B, bool isPrototype = false)
        {
            _selectedClient = cient;
            _isB2B = isB2B;

            if (!isPrototype)
            {
                DeleteCommand = new RelayCommand(deleteClient);
                SaveCommand = new RelayCommand(saveClient);
            } else
            {
                DeleteCommand = new RelayCommand(window =>
                {
                    if (window != null)
                        (window as System.Windows.Window)?.Close();
                });

                SaveCommand = new RelayCommand((window) =>
                {
                    if (_selectedClient != null)
                    {
                        if (_isB2B)
                            BusinessCustomerModel.Add(_selectedClient);
                        else
                            CustomerModel.Add(_selectedClient);
                    }

                    if (window != null)
                        (window as System.Windows.Window)?.Close();
                });
            }

            CancelCommand = new RelayCommand(window =>
            {
                if (window != null)
                    (window as System.Windows.Window)?.Close();
            });

            createNewDummy();
        }

        private Dictionary<string, string>? getChangedCollection()
        {
            if(_customer == null || _customerDummy == null) return null;

            Dictionary<string, string>? dummy = new();

            for (int i = 0; i < _customer.Count; i++)
            {
                if(_customer[i].Value != _customerDummy[i].Value)
                {
                    dummy.Add(new String(_customerDummy[i].Key), new String(_customerDummy[i].Value));
                }
            }

            if (dummy.Count > 0) {
                dummy.Add("OldId", new string((_selectedClient as CustomerAbstractModel)?.Id.ToString()));
                return dummy;
            }

            return null;
        }

        private void saveClient(object? window)
        {
            // detect all changes done by the user
            Dictionary<string, string>? changes = getChangedCollection();

            if (changes != null)
            {
                if (_isB2B)
                    (SelectedClient as BusinessCustomerModel)?.Update(changes);
                else
                    (SelectedClient as CustomerModel)?.Update(changes);
            }

            if (window != null)
                (window as System.Windows.Window)?.Close();
        }

        private void deleteClient(object? window)
        {
            if (MessageBoxWindowView.Instance != null)
                return;

            MessageBoxWindowView messageBox = new();
            MessageBoxWindowViewModel messageBoxVM = new("DELETE Client", "WARNING: Your action will be permament!");

            messageBox.DataContext = messageBoxVM;

            messageBoxVM.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == "Action")
                {
                    ActionStatus? status = (s as MessageBoxWindowViewModel)?.Action;

                    if (status != null)
                        switch (status)
                        {
                            case ActionStatus.OK:
                                if (_isB2B)
                                    (_selectedClient as BusinessCustomerModel)?.Delete();
                                else
                                    (_selectedClient as CustomerModel)?.Delete();
                                (window as System.Windows.Window)?.Close();
                                break;
                            default:
                                break;
                        }
                }
            };
        }
    }
}
