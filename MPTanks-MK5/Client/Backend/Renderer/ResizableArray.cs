using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Client.Backend.Renderer
{
    class ResizableArray<T>
    {
        T[] _array;
        int _count;

        public ResizableArray(int? initialCapacity = null)
        {
            _array = new T[initialCapacity ?? 4]; // or whatever
        }

        internal T[] InternalArray { get { return _array; } }

        public int Count { get { return _count; } }

        public void Add(T element)
        {
            lock (_array)
            {
                if (_count == _array.Length)
                {
                    Array.Resize(ref _array, _array.Length * 2);
                }

                _array[_count++] = element;
            }
        }
        public void Clear()
        {
            lock (_array)
            {
                for (var i = 0; i < _array.Length; i++)
                    _array[i] = default(T);

                _count = 0;
            }
        }
    }
}
