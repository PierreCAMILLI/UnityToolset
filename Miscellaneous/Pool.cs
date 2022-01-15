using System.Collections.Generic;
using UnityEngine;

namespace Toolset
{
    /// <summary>
    /// Interface defining a behaviour for an object pulled from a pool
    /// </summary>
    public interface IPopPool
    {
        public void OnPop();
    }

    /// <summary>
    /// Interface defining a behaviour for an object pushed in a pool
    /// </summary>
    public interface IPushPool
    {
        public void OnPush();
    }

    /// <summary>
    /// Interface defining a behaviour for an object destroyed in a pool
    /// </summary>
    public interface IDestroyPool
    {
        public void OnDestroyInPool();
    }

    [System.Serializable]
    public class Pool<T> where T : class
    {
        private Stack<T> _pool;

        public int Size { get { return _pool.Count; } }

        protected virtual void OnPop(ref T item) { }
        protected virtual void OnPush(ref T item) { }
        protected virtual void OnDestroy(ref T item) { }

        public Pool()
        {
            _pool = new Stack<T>();
        }

        /// <summary>
        /// Creates a new instance of object to store in the pool
        /// </summary>
        /// <returns></returns>
        protected virtual T Create()
        {
            return default(T);
        }

        /// <summary>
        /// Resize the container
        /// Creates new instances if size in parameter is greater than actual size
        /// Destroys instances if size in parameter is smaller than actual size
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public bool Resize(int size)
        {
            if (size < 0)
            {
                return false;
            }
            else if (size > _pool.Count)
            {
                while (_pool.Count != size)
                {
                    Push(Create());
                }
                _pool.TrimExcess();
                return true;
            }
            else if (size < _pool.Count)
            {
                while (_pool.Count != size)
                {
                    DestroyOnTop();
                }
                _pool.TrimExcess();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Creates new instances and add them to the pool
        /// </summary>
        /// <param name="count"></param>
        public void Allocate(int count)
        {
            if (count >= 0)
            {
                for (int i = 0; i < count; ++i)
                {
                    Push(Create());
                }
            }
        }

        /// <summary>
        /// Destroy the instance on top of the pool
        /// </summary>
        private void DestroyOnTop()
        {
            if (_pool.Count > 0)
            {
                T item = _pool.Pop();
                if (item is IDestroyPool)
                {
                    (item as IDestroyPool).OnDestroyInPool();
                }
                OnDestroy(ref item);
            }
        }

        /// <summary>
        /// Remove the instance at the top of the pool and returns it
        /// </summary>
        /// <returns></returns>
        public T Pop()
        {
            T item = _pool.Count > 0 ? _pool.Pop() : Create();
            if (item is IPopPool)
            {
                (item as IPopPool).OnPop();
            }
            OnPop(ref item);
            return item;
        }

        /// <summary>
        /// Remove the specified amount of instances from the pool and return them in a new array
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public T[] Pop(int count)
        {
            count = Mathf.Min(count, _pool.Count);
            T[] items = new T[count];
            for (int i = 0; i < count; ++i)
            {
                items[i] = Pop();
            }
            return items;
        }

        /// <summary>
        /// Remove instances from the pool and put them in the array parameter
        /// The array will be filled with as much place as available or with the whole pool content
        /// </summary>
        /// <param name="array"></param>
        public int Pop(T[] array)
        {
            int count = Mathf.Min(array.Length, _pool.Count);
            for (int i = 0; i < count; ++i)
            {
                array[i] = Pop();
            }
            return count;
        }

        /// <summary>
        /// Destroy every instances from the pool and clear its content
        /// </summary>
        public void Clear()
        {
            while (_pool.Count > 0)
            {
                DestroyOnTop();
            }
            _pool.TrimExcess();
        }

        /// <summary>
        /// Push a new instance in the pool
        /// </summary>
        /// <param name="item"></param>
        public void Push(T item)
        {
            if (item != null)
            {
                OnPush(ref item);
                _pool.Push(item);
                if (item is IPushPool)
                {
                    (item as IPushPool).OnPush();
                }
            }
        }

        /// <summary>
        /// Push every instances from an enumerable in the pool
        /// </summary>
        /// <param name="items"></param>
        public void Push(IEnumerable<T> items)
        {
            if (items != null)
            {
                foreach (T item in items)
                {
                    Push(item);
                }
            }
        }
    }

    [System.Serializable]
    public class PoolObject<T> : Pool<T> where T : UnityEngine.Object
    {
        [SerializeField]
        private T _reference;
        public T Reference { get { return _reference; } }

        private System.Action<T> _onPop;
        private System.Action<T> _onPush;
        private System.Action<T> _onDestroy;

        private PoolObject() { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="reference">Reference that will be associated with the pool</param>
        public PoolObject(T reference)
        {
            _reference = reference;
        }

        /// <summary>
        /// Set method called on instance when pulled from the pool
        /// </summary>
        /// <param name="onPop"></param>
        public void SetOnPop(System.Action<T> onPop)
        {
            _onPop = onPop;
        }

        /// <summary>
        /// Set method called on instance when pushed in the pool
        /// </summary>
        /// <param name="onPush"></param>
        public void SetOnPush(System.Action<T> onPush)
        {
            _onPush = onPush;
        }

        /// <summary>
        /// Set method called on instance when destroyed from the pool
        /// </summary>
        /// <param name="onDestroy"></param>
        public void SetOnDestroy(System.Action<T> onDestroy)
        {
            _onDestroy = onDestroy;
        }

        /// <summary>
        /// Create a new instance of the reference associated with the pool
        /// </summary>
        /// <returns></returns>
        protected override T Create()
        {
            return Object.Instantiate(_reference);
        }

        protected override void OnPop(ref T item)
        {
            if (_onPop != null)
            {
                _onPop(item);
            }
        }

        protected override void OnPush(ref T item)
        {
            if (_onPush != null)
            {
                _onPush(item);
            }
        }

        protected override void OnDestroy(ref T item)
        {
            if (_onDestroy != null)
            {
                _onDestroy(item);
            }
        }
    }
}
