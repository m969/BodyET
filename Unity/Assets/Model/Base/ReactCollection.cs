using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ETModel;

namespace ETModel
{
    public class ReactCollection<T> : Collection<T>, IDisposable
    {
        public ReactCollection()
        {

        }

        public ReactCollection(IEnumerable<T> collection)
        {
            if (collection == null) throw new ArgumentNullException("collection");

            foreach (var item in collection)
            {
                Add(item);
            }
        }

        public ReactCollection(List<T> list)
            : base(list != null ? new List<T>(list) : null)
        {
        }

        //protected override void ClearItems()
        //{
        //    var beforeCount = Count;
        //    base.ClearItems();

        //    if (collectionReset != null) collectionReset.OnNext(Unit.Default);
        //    if (beforeCount > 0)
        //    {
        //        if (countChanged != null) countChanged.OnNext(Count);
        //    }
        //}

        protected override void InsertItem(int index, T item)
        {
            base.InsertItem(index, item);
            if (_onCollectionInsert != null)
                _onCollectionInsert.Invoke(index, item);
            if (_onCollectionChange != null)
                _onCollectionChange.Invoke(this);
        }

        protected override void RemoveItem(int index)
        {
            T item = this[index];
            base.RemoveItem(index);
            if (_onCollectionRemove != null)
                _onCollectionRemove.Invoke(index, item);
            if (_onCollectionChange != null)
                _onCollectionChange.Invoke(this);
        }

        protected override void SetItem(int index, T item)
        {
            T oldItem = this[index];
            base.SetItem(index, item);
            if (_onCollectionReplace != null)
                _onCollectionReplace.Invoke(index, oldItem, item);
            if (_onCollectionChange != null)
                _onCollectionChange.Invoke(this);
        }


        public Action<object> _onCollectionChange;
        public void SubscribeChange(Action<object> action)
        {
            disposedValue = false;
            _onCollectionChange += action;
            action.Invoke(this);
        }

        public void UnSubscribeChange(Action<object> action)
        {
            _onCollectionChange -= action;
        }

        public Action<int, T> _onCollectionInsert;
        public void SubscribeAdd(Action<int, T> action)
        {
            disposedValue = false;
            _onCollectionInsert += action;
        }

        public void UnSubscribeAdd(Action<int, T> action)
        {
            _onCollectionInsert -= action;
        }

        public Action<int, T> _onCollectionRemove;
        public void SubscribeRemove(Action<int, T> action)
        {
            disposedValue = false;
            _onCollectionRemove += action;
        }

        public void UnSubscribeRemove(Action<int, T> action)
        {
            _onCollectionRemove -= action;
        }

        public Action<int, T, T> _onCollectionReplace;
        public void SubscribeReplace(Action<int, T, T> action)
        {
            disposedValue = false;
            _onCollectionReplace += action;
        }

        public void UnSubscribeReplace(Action<int, T, T> action)
        {
            _onCollectionReplace -= action;
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
                    _onCollectionInsert = null;
                    _onCollectionReplace = null;
                    _onCollectionRemove = null;
                    //Delegate[] delArray = _onCollectionInsert.GetInvocationList();
                    //for (int i = 0; i < delArray.Length; i++)
                    //    _onCollectionInsert -= delArray[i] as Action<int, T>;
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
    }
}