using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace CASh.Core
{
    public class ObservablePairCollection<TKey, TValue> : ObservableCollection<Pair<TKey, TValue>>
    {
        public event PropertyChangedEventHandler? ValueChanged;
        public ObservablePairCollection()
            : base() {
            this.PropertyChanged += ObservablePairCollection_PropertyChanged;
        }

        private void ObservablePairCollection_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            ValueChanged?.Invoke(sender, e);
        }

        public ObservablePairCollection(IEnumerable<Pair<TKey, TValue>> enumerable)
            : base(enumerable) {
            this.PropertyChanged += ObservablePairCollection_PropertyChanged;
        }

        public ObservablePairCollection(List<Pair<TKey, TValue>> list)
            : base(list) {
            this.PropertyChanged += ObservablePairCollection_PropertyChanged;
        }

        public ObservablePairCollection(IDictionary<TKey, TValue> dictionary)
        {
            foreach (var kv in dictionary)
            {
                Add(new Pair<TKey, TValue>(kv));
            }
            this.PropertyChanged += ObservablePairCollection_PropertyChanged;
        }

        public void Add(TKey key, TValue value)
        {
            Add(new Pair<TKey, TValue>(key, value));
        }
    }
}
