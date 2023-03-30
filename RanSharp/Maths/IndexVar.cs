using System.Collections;

namespace RanSharp.Maths
{
    public readonly struct IndexVar<T> : IEnumerable<T[]>
    {
        private readonly HashSet<T[]> _values = new();
        public int Count => _values.Count;
        public List<T[]> Values => _values.ToList();
        public readonly int IndexLength;
        public IndexVar(int indexLen) { IndexLength = indexLen; }
        public T[] this[params T[] index]
        {
            get
            {
                if (null == index)
                    throw new ArgumentNullException(nameof(index));
                if (index.Length != IndexLength)
                    throw new ArgumentException($"Index length must be {IndexLength}.");
                _ = _values.Add(index);
                return index;
            }
            set => _values.Add(value);
        }
        public void Remove(params T[] index)
        {
            if (null == index)
                throw new ArgumentNullException(nameof(index));
            if (index.Length != IndexLength)
                throw new ArgumentException($"Index length must be {IndexLength}.");
            _values.Remove(index);
        }
        public IEnumerator GetEnumerator() => _values.GetEnumerator();
        IEnumerator<T[]> IEnumerable<T[]>.GetEnumerator() => _values.GetEnumerator();
    }
}
