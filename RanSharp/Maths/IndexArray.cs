using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace RanSharp.Maths
{
    /// <summary>
    /// A wrapper on the Array type which overloads the Equals() method and returns true only if all elements are equal.
    /// This struct is intended to be used with the IndexVar&lt;T&gt; type to prevent repeating elements in the indexed variable.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public readonly struct IndexArray<T> where T : struct, INumber<T>
    {
        private readonly T[] data;
        /// <summary>
        /// A constructor for the IndexVar&lt;T&gt; struct based on the provided array of data.
        /// </summary>
        /// <param name="data"></param>
        public IndexArray(T[] data)
        {
            this.data = data;
        }
        /// <summary>
        /// A read-only pseudo indexer. It used to access the elements of the inner array.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public T this[int index]
        {
            get { return data[index]; }
            set { data[index] = value; }
        }
        /// <summary>
        /// Returns true if all elements of both objects are equal in value.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj == null) return false;
            if (obj is not IndexArray<T>) return false;
            IndexArray<T> other = (IndexArray<T>)obj;
            if (data.Length != other.data.Length) return false;
            bool result = true;
            for (int i  = 0; i < data.Length; i++)
                result &= data[i] == other.data[i];
            return result;
        }
        /// <summary>
        /// Gets the hashcode of the inner data array.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return data.GetHashCode();
        }
        /// <summary>
        /// Compars the equality of both operands using the overloaded Equals operator.
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static bool operator ==(IndexArray<T> lhs, IndexArray<T> rhs) => lhs.Equals(rhs);
        /// <summary>
        /// Compars the inequality of both operands using the overloaded Equals operator.
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static bool operator !=(IndexArray<T> lhs, IndexArray<T> rhs) => !lhs.Equals(rhs);
        /// <summary>
        /// Explicits converts an IndexVar&lt;T&gt; to an array of <typeparamref name="T"/>
        /// </summary>
        /// <param name="original"></param>
        public static explicit operator T[](IndexArray<T> original) => original.data;
        /// <summary>
        /// Explicits converts an array of <typeparamref name="T"/> to an IndexVar&lt;T&gt;
        /// </summary>
        /// <param name="values"></param>
        public static explicit operator IndexArray<T>(T[] values) => new(values);
    }
}