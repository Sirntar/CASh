using System;
using System.Collections.Generic;
using CASh.MVVM.Model;
using CASh.Core;
using System.Linq;

namespace CASh.MVVM.ViewModel
{
    class CustomerViewModel : ObservableObject
    {
        private List<CustomerModel> _unmodifyCustomersList;
        private List<CustomerModel>? _customersList;

        private bool _isSearch = false;

        public List<CustomerModel>? CustomersList
        {
            get { return _customersList; }
            set { 
                _customersList = value;
                OnPropertyChanged();
                AddEvents();

                if (!_isSearch)
                {
                    if (value != null)
                        _unmodifyCustomersList = new List<CustomerModel>(value);
                    else
                        _unmodifyCustomersList.Clear();
                }
            }
        }

        private CustomerModel? _selected;

        public CustomerModel? Selected
        {
            get { return _selected; }
            set { 
                _selected = value; 
                OnPropertyChanged();
            }
        }

        private void AddEvents()
        {
            if (CustomersList == null) return;

            foreach (var customers in CustomersList)
            {
                customers.CustomerDeleted += (obj, args) =>
                {
                    var item = obj as CustomerModel;
                    if (item != null)
                    {
                        List<CustomerModel>? list = new List<CustomerModel>(_unmodifyCustomersList);
                        list?.Remove(item);
                        Selected = null;

                        // refreshing view that way is simpler than
                        // using for example
                        // CollectionViewSource.GetDefaultView(CustomersList).Refresh();
                        CustomersList = null;
                        CustomersList = list;
                        _unmodifyCustomersList = new List<CustomerModel>(list);
                        Search(_query);
                    }
                };
                customers.CustomerUpdated += (obj, args) =>
                {
                    if (obj != null)
                    {
                        List<CustomerModel>? list = new List<CustomerModel>(_unmodifyCustomersList);
                        Selected = null;

                        // refreshing view that way is simpler than
                        // using for example
                        // CollectionViewSource.GetDefaultView(CustomersList).Refresh();
                        CustomersList = null;
                        CustomersList = list;
                        _unmodifyCustomersList = new List<CustomerModel>(list);
                        Search(_query);
                    }
                };
            }
        }

        private String _query = "";

        public void Search(String query)
        {
            _query = query;
            _isSearch = true;

            if (query == null || query == "")
            {
                CustomersList = new List<CustomerModel>(_unmodifyCustomersList);
                _isSearch = false;
                return;
            }
            query = query.ToLower();

            CustomersList = _unmodifyCustomersList?.Where(
                o => {
                    if (o.FirstName != null)
                        if (o.FirstName.ToString().ToLower().Contains(query))
                            return true;
                    if (o.LastName != null)
                        if (o.LastName.ToString().ToLower().Contains(query))
                            return true;
                    if (o.PhoneNumber != null)
                        if (o.PhoneNumber.ToString().ToLower().Contains(query))
                            return true;
                    if (o.Email != null)
                        if (o.Email.ToString().ToLower().Contains(query))
                            return true;
                    if (o.PESEL != null)
                        if (o.PESEL.ToString().ToLower().Contains(query))
                            return true;
                    if (o.IBAN != null)
                        if (o.IBAN.ToString().ToLower().Contains(query))
                            return true;
                    return false;
                }).ToList();
            _isSearch = false;
        }

        public CustomerViewModel()
        {
            List<CustomerModel> customersList = CustomerModel.FetchAll();
            _customersList = customersList;
            _unmodifyCustomersList = new List<CustomerModel>(customersList);
            AddEvents();
        }
    }
}
