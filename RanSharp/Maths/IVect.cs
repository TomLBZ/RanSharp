using System.Numerics;

namespace RanSharp.Maths
{
    /// <summary>
    /// An interface that specifies some basic functions a vector should have.
    /// This interface does not require arithmetic operators to be implemented.
    /// </summary>
    public interface IVect<T> where T : struct, INumber<T>
    {
        /// <summary>
        /// Gets the value at the specified index.
        /// </summary>
        T this[int i] { get; }
        /// <summary>
        /// Gets the length of the vector.
        /// </summary>
        int Length { get; }
        /// <summary>
        /// Gets the sum of all elements.
        /// </summary>
        T Sum();
        /// <summary>
        /// Gets the max value of all elements.
        /// </summary>
        T Max();
        /// <summary>
        /// Gets the min value of all elements.
        /// </summary>
        T Min();
        /// <summary>
        /// Gets the squared magnitude of the vector.
        /// </summary>
        T Mag2();
        /// <summary>
        /// Gets the magnitude of the vector.
        /// </summary>
        T Mag();
    }
}
