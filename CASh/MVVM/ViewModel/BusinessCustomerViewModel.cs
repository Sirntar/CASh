using System;
using System.Collections.Generic;
using CASh.MVVM.Model;
using CASh.Core;
using System.Linq;

namespace CASh.MVVM.ViewModel
{
    class BusinessCustomerViewModel : ObservableObject
    {
        private List<BusinessCustomerModel> _unmodifybusinessCustomersList;
        private List<BusinessCustomerModel>? _businessCustomersList;

        private bool _isSearch = false;

        public List<BusinessCustomerModel>? BusinessCustomersList
        {
            get { return _businessCustomersList; }
            set { 
                _businessCustomersList = value;
                OnPropertyChanged();
                AddEvents();

                if (!_isSearch)
                {
                    if (value != null)
                        _unmodifybusinessCustomersList = new List<BusinessCustomerModel>(value);
                    else
                        _unmodifybusinessCustomersList.Clear();
                }
            }
        }

        private BusinessCustomerModel? _selected;

        public BusinessCustomerModel? Selected
        {
            get { return _selected; }
            set {
                _selected = value;
                OnPropertyChanged();
            }
        }

        private void AddEvents()
        {
            if (BusinessCustomersList == null) return;

            foreach (var businessCustomers in BusinessCustomersList)
            {
                businessCustomers.CustomerDeleted += (obj, args) =>
                {
                    var item = obj as BusinessCustomerModel;
                    if (item != null)
                    {
                        List<BusinessCustomerModel>? list = new List<BusinessCustomerModel>(_unmodifybusinessCustomersList);
                        list?.Remove(item);
                        Selected = null;

                        // refreshing view that way is simpler than
                        // using for example
                        // CollectionViewSource.GetDefaultView(BusinessCustomersList).Refresh();
                        BusinessCustomersList = null;
                        BusinessCustomersList = list;
                        _unmodifybusinessCustomersList = new List<BusinessCustomerModel>(list);
                        Search(_query);
                    }
                };
                businessCustomers.CustomerUpdated += (obj, args) =>
                {
                    if (obj != null)
                    {
                        List<BusinessCustomerModel>? list = new List<BusinessCustomerModel>(_unmodifybusinessCustomersList);
                        Selected = null;

                        // refreshing view that way is simpler than
                        // using for example
                        // CollectionViewSource.GetDefaultView(BusinessCustomersList).Refresh();
                        BusinessCustomersList = null;
                        BusinessCustomersList = list;
                        _unmodifybusinessCustomersList = new List<BusinessCustomerModel>(list);
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
                BusinessCustomersList = new List<BusinessCustomerModel>(_unmodifybusinessCustomersList);
                _isSearch = false;
                return;
            }
            query = query.ToLower();

            BusinessCustomersList = _unmodifybusinessCustomersList?.Where(
                o => {
                    if(o.Name != null)
                        if(o.Name.ToString().ToLower().Contains(query))
                            return true;
                    if (o.PhoneNumber != null)
                        if (o.PhoneNumber.ToString().ToLower().Contains(query))
                            return true;
                    if (o.Email != null)
                        if (o.Email.ToString().ToLower().Contains(query))
                            return true;
                    if (o.IBAN != null)
                        if (o.IBAN.ToString().ToLower().Contains(query))
                            return true;
                    return false;
                }).ToList();
            _isSearch = false;
        }

        public BusinessCustomerViewModel()
        {
            List<BusinessCustomerModel> businessCustomersList = BusinessCustomerModel.FetchAll();
            _businessCustomersList = businessCustomersList;
            _unmodifybusinessCustomersList = new List<BusinessCustomerModel>(businessCustomersList);
            AddEvents();
        }
    }
}
