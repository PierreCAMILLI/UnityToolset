using System;
using System.Collections;
using System.Collections.Generic;

namespace Toolset
{
    public interface IArray2D : IEnumerable, ICloneable
    {
        public bool IsOutOfBounds(int x, int y);
        public int Width { get; }
        public int Height { get; }
    }

    public interface IArray2D<T> : IArray2D, ICollection<T>, IEnumerable<T>, IList<T>
    {
        public T this[int x, int y] { get; set; }
    }

    public class Array2D<T> : IArray2D<T>
    {
        private T[] _array;
        private int _width;
        private int _height;

        public Array2D(int width, int height)
        {
            _width = width;
            _height = height;
            _array = new T[width * height];
        }

        public T this[int i]
        {
            get => _array[i];
            set => _array[i] = value;
        }

        public T this[int x, int y]
        {
            get => _array[x + (y * _width)];
            set => _array[x + (y * _width)] = value;
        }

        public int Width => _width;
        public int Height => _height;

        public int Count => _array.Length;

        public bool IsSynchronized => _array.IsSynchronized;

        public object SyncRoot => _array.SyncRoot;

        public bool IsReadOnly => _array.IsReadOnly;

        public void Add(T item)
        {
            throw new NotSupportedException();
        }

        public void Clear()
        {
            throw new NotSupportedException();
        }

        public object Clone()
        {
            Array2D<T> clone = new Array2D<T>(_width, _height);
            clone._array = (T[])_array.Clone();
            return clone;
        }

        public bool Contains(T item)
        {
            for (int i = 0; i < _array.Length; ++i)
            {
                if (_array[i].Equals(item))
                {
                    return true;
                }
            }
            return false;
        }

        public void CopyTo(Array array, int index)
        {
            _array.CopyTo(array, index);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _array.SetValue(array, arrayIndex);
        }

        public IEnumerator GetEnumerator()
        {
            return _array.GetEnumerator();
        }

        public int IndexOf(T item)
        {
            for (int i = 0; i < _array.Length; ++i)
            {
                if (_array[i].Equals(item))
                {
                    return i;
                }
            }
            return -1;
        }

        public void Insert(int index, T item)
        {
            throw new NotSupportedException();
        }

        public bool IsOutOfBounds(int x, int y)
        {
            return x < 0 || x >= _width || y < 0 || y >= _height;
        }

        public bool Remove(T item)
        {
            throw new NotSupportedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            throw new NotSupportedException();
        }
    }

    public class Array2DBool : IArray2D<bool>
    {
        private BitArray _array;
        private int _width;
        private int _height;

        public Array2DBool(int width, int height)
        {
            _width = width;
            _height = height;
            _array = new BitArray(width * height);
        }

        public bool this[int i]
        {
            get => _array[i];
            set => _array[i] = value;
        }

        public bool this[int x, int y]
        {
            get => _array[x + (y * _width)];
            set => _array[x + (y * _width)] = value;
        }

        public int Width => _width;
        public int Height => _height;

        public int Count => _array.Length;

        public bool IsSynchronized => _array.IsSynchronized;

        public object SyncRoot => _array.SyncRoot;

        public bool IsReadOnly => _array.IsReadOnly;

        public void Add(bool item)
        {
            throw new NotSupportedException();
        }

        public void Clear()
        {
            throw new NotSupportedException();
        }

        public object Clone()
        {
            Array2DBool clone = new Array2DBool(_width, _height);
            clone._array = (BitArray)_array.Clone();
            return clone;
        }

        public bool Contains(bool item)
        {
            for (int i = 0; i < _array.Length; ++i)
            {
                if (_array[i].Equals(item))
                {
                    return true;
                }
            }
            return false;
        }

        public void CopyTo(Array array, int index)
        {
            _array.CopyTo(array, index);
        }

        public void CopyTo(bool[] array, int arrayIndex)
        {
            _array.CopyTo(array, arrayIndex);
        }

        public IEnumerator GetEnumerator()
        {
            return _array.GetEnumerator();
        }

        public int IndexOf(bool item)
        {
            for (int i = 0; i < _array.Length; ++i)
            {
                if (_array[i].Equals(item))
                {
                    return i;
                }
            }
            return -1;
        }

        public void Insert(int index, bool item)
        {
            throw new NotSupportedException();
        }

        public bool IsOutOfBounds(int x, int y)
        {
            return x < 0 || x >= _width || y < 0 || y >= _height;
        }

        public bool Remove(bool item)
        {
            throw new NotSupportedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        IEnumerator<bool> IEnumerable<bool>.GetEnumerator()
        {
            throw new NotSupportedException();
        }
    }
}
