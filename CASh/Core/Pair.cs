using System.Collections.Generic;

namespace CASh.Core
{
    public class Pair<TKey, TValue> : ObservableObject
    {
        protected TKey? _key;
        protected TValue? _value;

        public TKey? Key
        {
            get { return _key; }
            set
            {
                if ((_key == null && value != null) || (_key != null && value == null) || !_key.Equals(value))
                {
                    _key = value;
                    OnPropertyChanged();
                }
            }
        }

        public TValue? Value
        {
            get { return _value; }
            set
            {
                if ((_value == null && value != null) || (_value != null && value == null) || (_value != null && !_value.Equals(value)))
                {
                    _value = value;
                    OnPropertyChanged();
                }
            }
        }

        public Pair() { }

        public Pair(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }

        public Pair(KeyValuePair<TKey, TValue> kv)
        {
            Key = kv.Key;
            Value = kv.Value;
        }
    }
}
