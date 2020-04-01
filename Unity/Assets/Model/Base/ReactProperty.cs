using System;
using ETModel;

namespace ETModel
{
    public class ReactProperty<T> : IDisposable
    {
        public Action<T> _onChanged;
        public bool _onlyWhenChanged = false;
        private T _value;
        public T Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (_onlyWhenChanged)
                {
                    if (_value != null)
                    {
                        if (!_value.Equals(value))
                        {
                            _value = value;
                            if (_onChanged != null)
                                _onChanged.Invoke(_value);
                        }
                    }
                    else
                    {
                        if (value != null)
                        {
                            _value = value;
                            if (_onChanged != null)
                                _onChanged.Invoke(_value);
                        }
                    }
                }
                else
                {
                    _value = value;
                    if (_onChanged != null)
                        _onChanged.Invoke(_value);
                }
            }
        }

        public ReactProperty(T value = default)
        {
            _value = value;
        }

        //订阅
        public void Subscribe(Action<T> action)
        {
            disposedValue = false;
            _onChanged += action;
            action.Invoke(_value);
        }

        //取消订阅
        public void UnSubscribe(Action<T> action)
        {
            _onChanged -= action;
        }

        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    while (_onChanged != null)
                        _onChanged -= _onChanged;
                    // TODO: 释放托管状态(托管对象)。
                }

                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。

                disposedValue = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~ReactProperty()
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