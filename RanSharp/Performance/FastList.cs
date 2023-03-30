using System.Collections;

namespace RanSharp.Performance
{
    /// <summary>
    /// A fast list that is not thread-safe. It provides a subset of the functionality of <see cref="List{T}"/>.
    /// It is faster than <see cref="List{T}"/> because it directly exposes the underlying array in the <see cref="ItemsUnsafe"/> property,
    /// allowing for fast iteration using <see cref="Span{T}"/> or unsafe code.
    /// </summary>
    public class FastList<T> : IList<T>, ICollection<T>, IEnumerable<T>
    {
        #region IList<T> Interface
        private const int _defaultCapacity = 4;
        private T[] _items;
        private int _size = 0;
        private int _version = 0;
        static readonly T[] _emptyArray = Array.Empty<T>();
        /// <summary>
        /// Gets the number of elements contained in the <see cref="FastList{T}"/>.
        /// </summary>
        public int Count => _size;
        /// <summary>
        /// Always returns false because <see cref="FastList{T}"/> is not read-only.
        /// </summary>
        public bool IsReadOnly => false;
        /// <summary>
        /// Gets or sets the capacity of the <see cref="FastList{T}"/>.
        /// </summary>
        public int Capacity
        {
            get { return _items.Length; }
            set
            {
                if (value < _size) throw new ArgumentOutOfRangeException(nameof(value));
                if (value != _items.Length)
                {
                    if (value < 0) _items = _emptyArray;
                    else
                    {
                        T[] newItems = new T[value];
                        if (_size > 0) Array.Copy(_items, 0, newItems, 0, _size);
                        _items = newItems;
                    }
                }
            }
        }
        /// <summary>
        /// Adds an item to the <see cref="FastList{T}"/>.
        /// </summary>
        public void Add(T item)
        {
            if (_size == _items.Length) EnsureCapacity(_size + 1);
            _items[_size++] = item;
            _version++;
        }
        /// <summary>
        /// Adds the elements of the specified collection to the end of the <see cref="FastList{T}"/>.
        /// </summary>
        public void AddRange(IEnumerable<T> collection) { InsertRange(_size, collection); }
        /// <summary>
        /// Removes all elements from the <see cref="FastList{T}"/>.
        /// </summary>
        public void Clear()
        {
            if (_size > 0)
            {
                Array.Clear(_items, 0, _size);
                _size = 0;
            }
            _version++;
        }
        /// <summary>
        /// Determines whether an element is in the <see cref="FastList{T}"/>.
        /// </summary>
        public bool Contains(T item)
        {
            if (null == item) return Loop<T>.TrueForAny(this, x => null == x);
            else return Loop<T>.TrueForAny(this, x => item.Equals(x));
        }
        /// <summary>
        /// Converts the elements in the <see cref="FastList{T}"/> to another type, and returns a <see cref="FastList{TOutput}"/> containing the converted elements.
        /// </summary>
        public FastList<TOutput> ConvertAll<TOutput>(Converter<T, TOutput> converter)
        {
            if (converter == null) throw new ArgumentNullException(nameof(converter));
            FastList<TOutput> list = new(_size);
            for (int i = 0; i < _size; i++)
                list._items[i] = converter(_items[i]);
            list._size = _size;
            return list;
        }
        /// <summary>
        /// Copies the entire <see cref="FastList{T}"/> to a compatible one-dimensional array, starting at index 0.
        /// </summary>
        public void CopyTo(T[] array) { CopyTo(array, 0); }
        /// <summary>
        /// Copies the entire <see cref="FastList{T}"/> to a compatible one-dimensional array, starting at the specified index of the target array.
        /// </summary>
        public void CopyTo(T[] array, int arrayIndex)
        {
            Array.Copy(_items, 0, array, arrayIndex, _size);
        }
        /// <summary>
        /// Copies a range of elements from the <see cref="FastList{T}"/> to a compatible one-dimensional array, starting at the specified index of the target array.
        /// </summary>
        public void CopyTo(int index, T[] array, int arrayIndex, int count)
        {
            if (_size - index < count) throw new ArgumentOutOfRangeException(nameof(count));
            Array.Copy(_items, index, array, arrayIndex, count);
        }
        private void EnsureCapacity(int min)
        {
            if (_items.Length < min)
            {
                int newCapacity = _items.Length == 0 ? _defaultCapacity : _items.Length * 2;
                if ((uint)newCapacity > Array.MaxLength) newCapacity = Array.MaxLength;
                if (newCapacity < min) newCapacity = min;
                Capacity = newCapacity;
            }
        }
        /// <summary>
        /// Applies an <see cref="Action{T}"/> to each element of the <see cref="FastList{T}"/>.
        /// </summary>
        public void ForEach(Action<T> action)
        {
            if (null == action) throw new ArgumentNullException(nameof(action));
            int version = _version;
            ReadOnlySpan<T> items = _items.AsSpan(0, _size);
            for (int i = 0; i < items.Length; i++)
            {
                if (version != _version) throw new InvalidOperationException("Data race!");
                action(items[i]);
            }
        }
        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="FastList{T}"/>.
        /// </summary>
        public IEnumerator<T> GetEnumerator() { return new Enumerator(this); }
        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the first occurrence within the entire <see cref="FastList{T}"/>.
        /// </summary>
        public int IndexOf(T item) { return Array.IndexOf(_items, item, 0, _size); }
        /// <summary>
        /// Inserts an element into the <see cref="FastList{T}"/> at the specified index.
        /// </summary>
        public void Insert(int index, T item)
        {
            if ((uint)index > (uint)_size) throw new ArgumentOutOfRangeException(nameof(index));
            if (_size == _items.Length) EnsureCapacity(_size + 1);
            if (index < _size) Array.Copy(_items, index, _items, index + 1, _size - index);
            _items[index] = item;
            _size++;
            _version++;
        }
        /// <summary>
        /// Inserts the elements of a collection into the <see cref="FastList{T}"/> at the specified index.
        /// </summary>
        public void InsertRange(int index, IEnumerable<T> collection)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if ((uint)index > (uint)_size) throw new ArgumentOutOfRangeException(nameof(index));
            if (collection is not ICollection<T> c)
            {
                using IEnumerator<T> en = collection.GetEnumerator();
                while (en.MoveNext()) Insert(index++, en.Current);
            }
            else
            {    // if collection is ICollection<T>
                int count = c.Count;
                if (count > 0)
                {
                    EnsureCapacity(_size + count);
                    if (index < _size) Array.Copy(_items, index, _items, index + count, _size - index);
                    if (this == c)// If we're inserting a List into itself
                    {
                        Array.Copy(_items, 0, _items, index, index);
                        Array.Copy(_items, index + count, _items, index * 2, _size - index);
                    }
                    else
                    {
                        T[] itemsToInsert = new T[count];
                        c.CopyTo(itemsToInsert, 0);
                        itemsToInsert.CopyTo(_items, index);
                    }
                    _size += count;
                }
            }
            _version++;
        }
        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="FastList{T}"/>.
        /// </summary>
        public bool Remove(T item)
        {
            int index = IndexOf(item);
            if (index >= 0)
            {
                RemoveAt(index);
                return true;
            }
            return false;
        }
        /// <summary>
        /// Removes the element at the specified index of the <see cref="FastList{T}"/>.
        /// </summary>
        public void RemoveAt(int index)
        {
            if ((uint)index >= (uint)_size) throw new ArgumentOutOfRangeException(nameof(index));
            _size--;
            if (index < _size) Array.Copy(_items, index + 1, _items, index, _size - index);
#pragma warning disable CS8601 // Possible null reference assignment.
            _items[_size] = default;
#pragma warning restore CS8601 // Possible null reference assignment.
            _version++;
        }
        /// <summary>
        /// Removes a range of elements from the <see cref="FastList{T}"/>.
        /// </summary>
        public void RemoveRange(int index, int count)
        {
            if (index < 0) throw new ArgumentOutOfRangeException(nameof(index));
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));
            if (_size - index < count) throw new ArgumentOutOfRangeException(nameof(count));
            if (count > 0)
            {
                _size -= count;
                if (index < _size) Array.Copy(_items, index + count, _items, index, _size - index);
                Array.Clear(_items, _size, count);
                _version++;
            }
        }
        /// <summary>
        /// Removes all the elements that match the conditions defined by the specified predicate.
        /// </summary>
        public int RemoveAll(Predicate<T> match)
        {
            if (match == null) throw new ArgumentNullException(nameof(match));
            int freeIndex = 0;   // the first free slot in items array
            while (freeIndex < _size && !match(_items[freeIndex])) freeIndex++;
            if (freeIndex >= _size) return 0;
            int current = freeIndex + 1;
            while (current < _size)
            {
                while (current < _size && match(_items[current])) current++;
                if (current < _size)
                    _items[freeIndex++] = _items[current++];
            }
            Array.Clear(_items, freeIndex, _size - freeIndex);
            int result = _size - freeIndex;
            _size = freeIndex;
            _version++;
            return result;
        }
        /// <summary>
        /// Reverses the order of the elements in the entire <see cref="FastList{T}"/>.
        /// </summary>
        public void Reverse() { Reverse(0, Count); }
        /// <summary>
        /// Reverses the order of the elements in the specified range.
        /// </summary>
        public void Reverse(int index, int count)
        {
            if (index < 0) throw new ArgumentOutOfRangeException(nameof(index));
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));
            if (_size - index < count) throw new ArgumentOutOfRangeException(nameof(count));
            Array.Reverse(_items, index, count);
            _version++;
        }
        /// <summary>
        /// Sorts the elements in the entire <see cref="FastList{T}"/> using the default comparer.
        /// </summary>
        public void Sort() { Sort(0, Count, null); }
        /// <summary>
        /// Sorts the elements in the entire <see cref="FastList{T}"/> using the specified comparer.
        /// </summary>
        public void Sort(IComparer<T> comparer) { Sort(0, Count, comparer); }
        /// <summary>
        /// Sorts the elements in a range of elements in <see cref="FastList{T}"/> using the specified comparer.
        /// </summary>
        public void Sort(int index, int count, IComparer<T>? comparer)
        {
            if (index < 0) throw new ArgumentOutOfRangeException(nameof(index));
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));
            if (_size - index < count) throw new ArgumentOutOfRangeException(nameof(count));
            Array.Sort(_items, index, count, comparer);
            _version++;
        }
        /// <summary>
        /// Sorts the elements in the entire <see cref="FastList{T}"/> using the specified <see cref="Comparison{T}"/>.
        /// </summary>
        public void Sort(Comparison<T> comparison)
        {
            if (comparison == null) throw new ArgumentOutOfRangeException(nameof(comparison));
            if (_size > 0)
            {
                IComparer<T> comparer = Comparer<T>.Create(comparison);
                Array.Sort(_items, 0, _size, comparer);
            }
        }
        /// <summary>
        /// Copies the entire <see cref="FastList{T}"/> to a compatible one-dimensional array, returning the array.
        /// </summary>
        public T[] ToArray()
        {
            T[] array = new T[_size];
            Array.Copy(_items, 0, array, 0, _size);
            return array;
        }
        /// <summary>
        /// Finds and returns the first element that matches the conditions defined by the specified predicate.
        /// </summary>
        public T Find(Predicate<T> match)
        {
            if (match == null) throw new ArgumentNullException(nameof(match));
            ReadOnlySpan<T> span = _items.AsSpan(0, _size);
            for (int i = 0; i < span.Length; i++)
                if (match(span[i])) return _items[i];
#pragma warning disable CS8603 // Possible null reference return.
            return default;
#pragma warning restore CS8603 // Possible null reference return.
        }
        /// <summary>
        /// Finds and returns the last element that matches the conditions defined by the specified predicate.
        /// </summary>
        public T FindLast(Predicate<T> match)
        {
            if (match == null) throw new ArgumentNullException(nameof(match));
            ReadOnlySpan<T> span = _items.AsSpan(0, _size);
            for (int i = span.Length - 1; i >= 0; i--)
                if (match(span[i])) return _items[i];
#pragma warning disable CS8603 // Possible null reference return.
            return default;
#pragma warning restore CS8603 // Possible null reference return.
        }
        /// <summary>
        /// Finds and returns all the elements that match the conditions defined by the specified predicate in a new <see cref="FastList{T}"/>.
        /// </summary>
        public FastList<T> FindAll(Predicate<T> match)
        {
            if (match == null) throw new ArgumentNullException(nameof(match));
            FastList<T> list = new();
            Loop<T>.Apply(_items, x => { if (match(x)) list.Add(x); });
            return list;
        }
        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified predicate, 
        /// and returns the zero-based index of the first occurrence within a range specified by the start index and the number of elements to search.
        /// </summary>
        public int FindIndex(int startIndex, int count, Predicate<T> match)
        {
            if ((uint)startIndex > (uint)_size) throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0 || startIndex > _size - count) throw new ArgumentOutOfRangeException(nameof(count));
            if (match == null) throw new ArgumentNullException(nameof(match));
            int endIndex = startIndex + count;
            for (int i = startIndex; i < endIndex; i++)
                if (match(_items[i])) return i;
            return -1;
        }
        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified predicate, 
        /// and returns the zero-based index of the last occurrence within the range specified by the start index and the number of elements to search.
        /// </summary>
        public int FindLastIndex(int startIndex, int count, Predicate<T> match)
        {
            if (match == null) throw new ArgumentNullException(nameof(match));
            if (_size == 0)
                if (startIndex != -1) throw new ArgumentOutOfRangeException(nameof(startIndex));
            else
                if ((uint)startIndex >= (uint)_size) throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0 || startIndex - count + 1 < 0) throw new ArgumentOutOfRangeException(nameof(startIndex));
            int endIndex = startIndex - count;
            for (int i = startIndex; i > endIndex; i--)
                if (match(_items[i])) return i;
            return -1;
        }
        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified predicate,
        /// and returns the zero-based index of the last occurrence from the start index to the end of the <see cref="FastList{T}"/>.
        /// </summary>
        public int FindLastIndex(int startIndex, Predicate<T> match) { return FindLastIndex(startIndex, startIndex + 1, match); }
        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified predicate,
        /// and returns the zero-based index of the last occurrence from the entire <see cref="FastList{T}"/>.
        /// </summary>
        public int FindLastIndex(Predicate<T> match) { return FindLastIndex(_size - 1, _size, match); }
        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified predicate,
        /// and returns the zero-based index of the first occurrence from the start index to the end of the <see cref="FastList{T}"/>.
        /// </summary>
        public int FindIndex(int startIndex, Predicate<T> match) { return FindIndex(startIndex, _size - startIndex, match); }
        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified predicate,
        /// and returns the zero-based index of the first occurrence from the entire <see cref="FastList{T}"/>.
        /// </summary>
        public int FindIndex(Predicate<T> match) { return FindIndex(0, _size, match); }
        /// <summary>
        /// Find the last occured index of the specified item in the <see cref="FastList{T}"/> from the specified index to the specified count.
        /// </summary>
        public int LastIndexOf(T item, int index, int count)
        {
            if (_size == 0) return -1;
            if ((Count != 0) && (index < 0)) throw new ArgumentOutOfRangeException(nameof(index));
            if (index >= _size) throw new ArgumentOutOfRangeException(nameof(index));
            if (count > index + 1) throw new ArgumentOutOfRangeException(nameof(count));
            return Array.LastIndexOf(_items, item, index, count);
        }
        /// <summary>
        /// Find the last occured index of the specified item in the <see cref="FastList{T}"/> from the specified index to the end of the <see cref="FastList{T}"/>.
        /// </summary>
        public int LastIndexOf(T item, int index)
        {
            if (index >= _size) throw new ArgumentOutOfRangeException(nameof(index));
            return LastIndexOf(item, index, index + 1);
        }
        /// <summary>
        /// Find the last occured index of the specified item in the entire <see cref="FastList{T}"/>.
        /// </summary>
        public int LastIndexOf(T item)
        {
            if (_size == 0) return -1;
            return LastIndexOf(item, _size - 1, _size);
        }
        /// <summary>
        /// Remove the excess capacity of the <see cref="FastList{T}"/>.
        /// </summary>
        public void TrimExcess()
        {
            int threshold = (int)(_items.Length * 0.9);
            if (_size < threshold) Capacity = _size;
        }
        IEnumerator IEnumerable.GetEnumerator() { return new Enumerator(this); }
        internal struct Enumerator : IEnumerator<T>
        {
            private readonly FastList<T> list;
            private int index;
            private readonly int version;
            private T? current;
            internal Enumerator(FastList<T> list)
            {
                this.list = list;
                index = 0;
                version = list._version;
                current = default;
            }
            public void Dispose() { }
            public bool MoveNext()
            {
                FastList<T> localList = list;
                if (version == localList._version && ((uint)index < (uint)localList._size))
                {
                    current = localList._items[index];
                    index++;
                    return true;
                }
                if (version != list._version) throw new InvalidOperationException("Data race!");
                index = list._size + 1;
                current = default;
                return false;
            }
#pragma warning disable CS8603 // Possible null reference return.
            public T Current => current;
            object IEnumerator.Current
            {
                get
                {
                    if (index == 0 || index == list._size + 1) throw new InvalidOperationException("Data race!");
                    return Current;
                }
            }
#pragma warning restore CS8603 // Possible null reference return.
            void IEnumerator.Reset()
            {
                if (version != list._version) throw new InvalidOperationException("Data race!");
                index = 0;
                current = default;
            }
        }
        #endregion

        #region Constructors
        public FastList() { _items = _emptyArray; }
        public FastList(int capacity)
        {
            if (capacity < 0) throw new ArgumentOutOfRangeException(nameof(capacity));
            if (capacity == 0) _items = _emptyArray;
            else _items = new T[capacity];
        }
        public FastList(IEnumerable<T> collection)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (collection is not ICollection<T> c)
            {
                _items = _emptyArray;
                using IEnumerator<T> en = collection.GetEnumerator();
                while (en.MoveNext()) Add(en.Current);
            }
            else
            {
                int count = c.Count;
                if (count == 0) _items = _emptyArray;
                else
                {
                    _items = new T[count];
                    c.CopyTo(_items, 0);
                    _size = count;
                }
            }
        }
        public FastList(params T[] items) 
        {
            if (null == items) _items = _emptyArray;
            else
            {
                _items = items;
                _size = items.Length;
            }
        }
        public FastList(int capacity, Func<int, T> generator)
        {
            if (capacity < 0) throw new ArgumentOutOfRangeException(nameof(capacity));
            if (capacity == 0) _items = _emptyArray;
            else
            {
                _items = new T[capacity];
                Span<T> span = _items.AsSpan();
                for (int i = 0; i < span.Length; i++)
                    span[i] = generator(i);
                _size = span.Length;
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// Directly exposes the inner array. This allows direct access to the inner array, but it is not recommended to use this property 
        /// unless you know what you are doing. The <see cref="FastList{T}"/> will not be able to track changes made to the inner array.
        /// </summary>
        public T[] ItemsUnsafe { get { return _items; } }
        #endregion

        #region Public Methods
        /// <summary>
        /// Updates the size of the <see cref="FastList{T}"/> after direct changes to the <see cref="ItemsUnsafe"/> property. 
        /// This method is unsafe and should only be used if you know what you are doing.
        /// </summary>
        public void UpdateCountUnsafe(int count) => _size = count;
        /// <summary>
        /// The indexer of the <see cref="FastList{T}"/>. This allows direct access to available indexes in the <see cref="FastList{T}"/> 
        /// before the <see cref="Add"/> has been called, as long as the length of the inner array has not been exhausted. 
        /// However, you should not depend on this behaviour unless you know what you are doing.
        /// </summary>
        public T this[int index]
        {
            get // allows access to the full inner array
            {
                if ((uint)index >= (uint)_items.Length) throw new ArgumentOutOfRangeException(nameof(index));
                return _items[index];
            }
            set // allows access to the full inner array
            {
                if ((uint)index >= (uint)_items.Length) throw new ArgumentOutOfRangeException(nameof(index));
                _items[index] = value;
                if ((uint)index >= (uint)_size) _size = index + 1; // as soon as an index is set via direct access, the size grows to that index
                _version++;
            }
        }
        /// <summary>
        /// Composite the <see cref="FastList{T}"/> with another <see cref="FastList{T}"/> using the provided function.
        /// </summary>
        public FastList<T> Composite(FastList<T> list, Func<T, T, T> f) => Loop<T>.Composite(this, list, f);
        /// <summary>
        /// Combine the <see cref="FastList{T}"/> with a provided value using the provided function.
        /// </summary>
        public FastList<T> Combine(T b, Func<T, T, T> f) => Loop<T>.Combine(this, b, f);
        /// <summary>
        /// Map each element of the <see cref="FastList{T}"/> to a corresponding new element using the provided function, and returns the new <see cref="FastList{T}"/>.
        /// </summary>
        public FastList<T> Map(Func<T, T> f) => Loop<T>.Map(this, f);
        /// <summary>
        /// Map each index of the <see cref="FastList{T}"/> to a corresponding new element using the provided function, and returns the new <see cref="FastList{T}"/>.
        /// </summary>
        public FastList<T> ReMap(Func<int, T> f) => Loop<T>.ReMap(this, f);
        /// <summary>
        /// Composite the <see cref="FastList{T}"/> with another <see cref="FastList{T}"/> using the provided function, modifying the original <see cref="FastList{T}"/>.
        /// </summary>
        public void CompositeInPlace(FastList<T> list, Func<T, T, T> f) => Loop<T>.CompositeInPlace(this, list, f);
        /// <summary>
        /// Combine the <see cref="FastList{T}"/> with a provided value using the provided function, modifying the original <see cref="FastList{T}"/>.
        /// </summary>
        public void CombineInPlace(T b, Func<T, T, T> f) => Loop<T>.CombineInPlace(this, b, f);
        /// <summary>
        /// Map each element of the <see cref="FastList{T}"/> to a corresponding new element using the provided function, modifying the original <see cref="FastList{T}"/>.
        /// </summary>
        public void MapInPlace(Func<T, T> f) => Loop<T>.MapInPlace(this, f);
        /// <summary>
        /// Map each index of the <see cref="FastList{T}"/> to a corresponding new element using the provided function, modifying the original <see cref="FastList{T}"/>.
        /// </summary>
        public void ReMapInPlace(Func<int, T> f) => Loop<T>.ReMapInPlace(this, f);
        /// <summary>
        /// Applies an action using each element of the <see cref="FastList{T}"/> as input. Does not modify the original <see cref="FastList{T}"/>.
        /// </summary>
        public void Apply(Action<T> action) => Loop<T>.Apply(this, action);
        /// <summary>
        /// Applies the provided function to each element of the <see cref="FastList{T}"/>, accumulating the result with the provided accumulator.
        /// </summary>
        public T Accumulate(T acc, Func<T, T, T> f) => Loop<T>.Accumulate(this, acc, f);
        /// <summary>
        /// Returns true if all elements of the <see cref="FastList{T}"/> satisfy the provided predicate.
        /// </summary>
        public bool TrueForAll(Predicate<T> match)
        {
            if (match == null) throw new ArgumentNullException(nameof(match));
            return Loop<T>.TrueForAll(_items, x => match(x));
        }
        /// <summary>
        /// Returns true if all elements of the <see cref="FastList{T}"/> satisfy the provided match function.
        /// </summary>
        public bool TrueForAll(Func<T, bool> match) => Loop<T>.TrueForAll(this, match);
        /// <summary>
        /// Returns true if any elements of the <see cref="FastList{T}"/> satisfy the provided predicate.
        /// </summary>
        public bool TrueForAny(Predicate<T> match)
        {
            if (match == null) throw new ArgumentNullException(nameof(match));
            return Loop<T>.TrueForAny(_items, x => match(x));
        }
        /// <summary>
        /// Returns true if any elements of the <see cref="FastList{T}"/> satisfy the provided match function.
        /// </summary>
        public bool TrueForAny(Func<T, bool> match) => Loop<T>.TrueForAny(this, match);
        #endregion
    }
}
