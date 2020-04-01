using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ETModel;

namespace ETModel
{
    public class ReactDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IEnumerable, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IDictionary, IDisposable
    {
        readonly Dictionary<TKey, TValue> inner;

        public ReactDictionary()
        {
            inner = new Dictionary<TKey, TValue>();
        }

        public ReactDictionary(IEqualityComparer<TKey> comparer)
        {
            inner = new Dictionary<TKey, TValue>(comparer);
        }

        public ReactDictionary(Dictionary<TKey, TValue> innerDictionary)
        {
            inner = innerDictionary;
        }

        public TValue this[TKey key]
        {
            get
            {
                return inner[key];
            }

            set
            {
                TValue oldValue;
                if (TryGetValue(key, out oldValue))
                {
                    inner[key] = value;
                    if (_onDictionaryReplace != null)
                        _onDictionaryReplace.Invoke(key, oldValue, value);
                }
                else
                {
                    inner[key] = value;
                    if (_onDictionaryAdd != null)
                        _onDictionaryAdd.Invoke(key, value);
                }
                if (_onDictionaryChange != null)
                    _onDictionaryChange.Invoke(this);
            }
        }

        public int Count
        {
            get
            {
                return inner.Count;
            }
        }

        public Dictionary<TKey, TValue>.KeyCollection Keys
        {
            get
            {
                return inner.Keys;
            }
        }

        public Dictionary<TKey, TValue>.ValueCollection Values
        {
            get
            {
                return inner.Values;
            }
        }

        public void Add(TKey key, TValue value)
        {
            inner.Add(key, value);
            if (_onDictionaryAdd != null)
                _onDictionaryAdd.Invoke(key, value);
            if (_onDictionaryChange != null)
                _onDictionaryChange.Invoke(this);
        }

        public void AddOrReplace(TKey key, TValue value)
        {
            if (inner.ContainsKey(key))
                this[key] = value;
            else
                Add(key, value);
        }

        public void Clear()
        {
            var beforeCount = Count;
            inner.Clear();

            //if (collectionReset != null) collectionReset.OnNext(Unit.Default);
            if (beforeCount > 0)
            {
                //if (countChanged != null) countChanged.OnNext(Count);
            }
        }

        public bool Remove(TKey key)
        {
            TValue oldValue;
            if (inner.TryGetValue(key, out oldValue))
            {
                var isSuccessRemove = inner.Remove(key);
                if (isSuccessRemove)
                {
                    if (_onDictionaryRemove != null)
                        _onDictionaryRemove.Invoke(key, oldValue);
                    if (_onDictionaryChange != null)
                        _onDictionaryChange.Invoke(this);
                }
                return isSuccessRemove;
            }
            else
            {
                return false;
            }
        }

        public bool ContainsKey(TKey key)
        {
            return inner.ContainsKey(key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return inner.TryGetValue(key, out value);
        }

        public Dictionary<TKey, TValue>.Enumerator GetEnumerator()
        {
            return inner.GetEnumerator();
        }


        public Action<object> _onDictionaryChange;
        public void SubscribeChange(Action<object> action)
        {
            disposedValue = false;
            _onDictionaryChange += action;
            action.Invoke(this);
        }

        public void UnSubscribeChange(Action<object> action)
        {
            _onDictionaryChange -= action;
        }

        public Action<TKey, TValue> _onDictionaryAdd;
        public void SubscribeAdd(Action<TKey, TValue> action)
        {
            disposedValue = false;
            _onDictionaryAdd += action;
        }

        public void UnSubscribeAdd(Action<TKey, TValue> action)
        {
            _onDictionaryAdd -= action;
        }

        public Action<TKey, TValue> _onDictionaryRemove;
        public void SubscribeRemove(Action<TKey, TValue> action)
        {
            disposedValue = false;
            _onDictionaryRemove += action;
        }

        public void UnSubscribeRemove(Action<TKey, TValue> action)
        {
            _onDictionaryRemove -= action;
        }

        public Action<TKey, TValue, TValue> _onDictionaryReplace;
        public void SubscribeReplace(Action<TKey, TValue, TValue> action)
        {
            disposedValue = false;
            _onDictionaryReplace += action;
        }

        public void UnSubscribeReplace(Action<TKey, TValue, TValue> action)
        {
            _onDictionaryReplace -= action;
        }


        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)。
                    _onDictionaryAdd = null;
                    _onDictionaryRemove = null;
                    _onDictionaryReplace = null;
                }

                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。

                disposedValue = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~ReactCollection()
        // {
        //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
        //   Dispose(false);
        // }

        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            // GC.SuppressFinalize(this);
        }
        #endregion

        #region implement explicit

        object IDictionary.this[object key]
        {
            get
            {
                return this[(TKey)key];
            }

            set
            {
                this[(TKey)key] = (TValue)value;
            }
        }


        bool IDictionary.IsFixedSize
        {
            get
            {
                return ((IDictionary)inner).IsFixedSize;
            }
        }

        bool IDictionary.IsReadOnly
        {
            get
            {
                return ((IDictionary)inner).IsReadOnly;
            }
        }

        bool ICollection.IsSynchronized
        {
            get
            {
                return ((IDictionary)inner).IsSynchronized;
            }
        }

        ICollection IDictionary.Keys
        {
            get
            {
                return ((IDictionary)inner).Keys;
            }
        }

        object ICollection.SyncRoot
        {
            get
            {
                return ((IDictionary)inner).SyncRoot;
            }
        }

        ICollection IDictionary.Values
        {
            get
            {
                return ((IDictionary)inner).Values;
            }
        }


        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
        {
            get
            {
                return ((ICollection<KeyValuePair<TKey, TValue>>)inner).IsReadOnly;
            }
        }

        ICollection<TKey> IDictionary<TKey, TValue>.Keys
        {
            get
            {
                return inner.Keys;
            }
        }

        ICollection<TValue> IDictionary<TKey, TValue>.Values
        {
            get
            {
                return inner.Values;
            }
        }

        void IDictionary.Add(object key, object value)
        {
            Add((TKey)key, (TValue)value);
        }

        bool IDictionary.Contains(object key)
        {
            return ((IDictionary)inner).Contains(key);
        }

        void ICollection.CopyTo(Array array, int index)
        {
            ((IDictionary)inner).CopyTo(array, index);
        }

        void IDictionary.Remove(object key)
        {
            Remove((TKey)key);
        }

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
        {
            Add((TKey)item.Key, (TValue)item.Value);
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
        {
            return ((ICollection<KeyValuePair<TKey, TValue>>)inner).Contains(item);
        }

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<TKey, TValue>>)inner).CopyTo(array, arrayIndex);
        }

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        {
            return ((ICollection<KeyValuePair<TKey, TValue>>)inner).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return inner.GetEnumerator();
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            TValue v;
            if (TryGetValue(item.Key, out v))
            {
                if (EqualityComparer<TValue>.Default.Equals(v, item.Value))
                {
                    Remove(item.Key);
                    return true;
                }
            }

            return false;
        }

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            return ((IDictionary)inner).GetEnumerator();
        }

        #endregion
    }
}