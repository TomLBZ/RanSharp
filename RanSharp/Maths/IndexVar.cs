using System.Collections;
using System.Numerics;
using RanSharp.Performance;

namespace RanSharp.Maths
{
    /// <summary>
    /// A variable with indices. Can be used in symbolic calculations, or as a dictionary key.<br/>
    /// For example:
    /// <code>
    /// IndexVar&lt;int&gt; x = new(2); // Represents an indexed variable based on 2 <see cref="int"/> values.<br/>
    /// List&lt;IndexArray&lt;int&gt;&gt; symbolicPoints = new() { x[0, 0], x[0, 1]}; // Represents a list of 2D points: x_00 and x_01<br/>
    /// Func&lt;IndexArray&lt;int&gt;, PointF&gt; valueMap = (indices) => {/*body*/}; // Represents a function that maps an indexed variable to its value.<br/>
    /// List&lt;PointF&gt; points = points.Select(valueMap).ToList(); // Evaluates the actual values of the points.<br/>
    /// </code>
    /// When the value map is a simple function, the <see cref="IndexVar{T}"/> can be used as a dictionary key:
    /// <code>
    /// Dictionary&lt;IndexArray&lt;int&gt;, PointF&gt; points = new() { { x[1, 2], new PointF(1, 2) }, { x[3, 4], new PointF(3, 4) }, { x[5, 6], new PointF(5, 6) } }; // Represents a list of 2D points.<br/>
    /// </code>
    /// When the value map is very complicated and performance is critical, the <see cref="IndexVar{T}"/> should be used instead of variable values, 
    /// because it avoids the expensive evaluation as much as possible. The value map only needs to be used once after all symbolic calculations has been done.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public readonly struct IndexVar<T> : IEnumerable<IndexArray<T>> where T : struct, INumber<T>
    {
        private readonly HashSet<IndexArray<T>> _values = new();
        /// <summary>
        /// Gets the number of unique indices that has been stored.
        /// </summary>
        public int Count => _values.Count;
        /// <summary>
        /// Gets the list of unique indices that has been stored.
        /// </summary>
        public FastList<IndexArray<T>> Values => new(_values);
        /// <summary>
        /// Gets the length of each index.
        /// </summary>
        public readonly int IndexLength;
        /// <summary>
        /// Initializes a new instance of the <see cref="IndexVar{T}"/> struct using the specified index length.
        /// </summary>
        /// <param name="indexLen"></param>
        public IndexVar(int indexLen) { IndexLength = indexLen; }
        /// <summary>
        /// The read-only pseudo indexer. It is used as a readable short hand for referencing indexed variables. Do not use it as an actual indexer.
        /// <br/>Correct usage:
        /// <code>verticesList.Add(x[0]); // Adds the variable x0 to the list of vertices</code>
        /// Incorrect usage:
        /// <code>x[0] = 5; // Wrong attempt to assign a value to the variable x0</code>
        /// </summary>
        /// <param name="index"></param>
        public IndexArray<T> this[params T[] index]
        {
            get
            {
                if (null == index)
                    throw new ArgumentNullException(nameof(index));
                if (index.Length != IndexLength)
                    throw new ArgumentException($"Index length must be {IndexLength}.");
                IndexArray<T> id = (IndexArray<T>)index;
                _ = _values.Add(id);
                return id;
            }
        }
        /// <summary>
        /// Removes the specified index from the list of unique indices.
        /// </summary>
        /// <param name="index"></param>
        public void Remove(params T[] index)
        {
            if (null == index)
                throw new ArgumentNullException(nameof(index));
            if (index.Length != IndexLength)
                throw new ArgumentException($"Index length must be {IndexLength}.");
            _values.Remove((IndexArray<T>)index);
        }
        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        public IEnumerator GetEnumerator() => _values.GetEnumerator();
        IEnumerator<IndexArray<T>> IEnumerable<IndexArray<T>>.GetEnumerator() => _values.GetEnumerator();
    }
}
